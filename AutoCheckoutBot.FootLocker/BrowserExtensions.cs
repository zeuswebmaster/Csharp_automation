using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoCheckoutBot.FootLocker.Enum;
using AutoCheckoutBot.FootLocker.Models;
using DotNetBrowser;
using DotNetBrowser.DOM;

namespace AutoCheckoutBot.FootLocker
{
    public static class BrowserExtensions
    {
        private static DOMElement Doc(this Browser browser)
        {
            return browser.GetDocument().DocumentElement;
        }

        public static async Task SetValue(this DOMInputElement inputElement, Browser browser, string val, bool clear = false)
        {
            inputElement.Focus();
            inputElement.Click();

            if (clear)
            {
                for (int i = 0; i < 100; i++) //CLEAR
                {

                    var param = new KeyParams(VirtualKeyCode.BACK, (char)8);
                    browser.KeyDown(param);
                    browser.KeyUp(param);

                    await Task.Delay(2);
                }

                await Task.Delay(1);
            }

            foreach (char c in val)
            {


                var param = new KeyParams(VirtualKeyCode.VK_E, c);

                if (c.ToString() == " ")
                    param = new KeyParams(VirtualKeyCode.SPACE, ' ');


                browser.KeyDown(param);
                browser.KeyUp(param);
                await Task.Delay(10);
            }



        }

        public static async Task SetValueSlow(this DOMInputElement inputElement, Browser browser, string val, bool clear = false)
        {


            inputElement.Focus();

            await Task.Delay(100);
            inputElement.Click();
            await Task.Delay(100);


            foreach (char c in val)
            {


                var param = new KeyParams(VirtualKeyCode.VK_E, c);

                if (c.ToString() == " ")
                    param = new KeyParams(VirtualKeyCode.SPACE, ' ');


                browser.KeyDown(param);
                browser.KeyUp(param);
                await Task.Delay(100);
            }



        }

        public static async Task TypeKeys(this Browser browser, string val)
        {
            foreach (char c in val)
            {


                var param = new KeyParams(VirtualKeyCode.VK_E, c);

                if (c.ToString() == " ")
                    param = new KeyParams(VirtualKeyCode.SPACE, ' ');


                browser.KeyDown(param);
                browser.KeyUp(param);
                await Task.Delay(100);
            }
        }

        //c-loading-curtain global-loading
        private static DOMNode AddToCartButton(this Browser browser)
        {
            return browser.GetDocument().DocumentElement.GetElementsByTagName("button").FirstOrDefault(x =>
                (x as DOMElement).HasAttribute("type") && (x as DOMElement).GetAttribute("type") == "submit" &&
                x.TextContent.ToLower().Contains("add to cart"));
        }


        public static bool IsAddToCartButtonAvailable(this Browser browser, ManualResetEvent waitEvent)
        {
            waitEvent.WaitOne();

            return browser.AddToCartButton() != null;
        }

        public static async Task OpenUrl(this Browser browser, string url, ManualResetEvent waitEvent)
        {
            waitEvent.Reset();
            browser.LoadURL(url);
            waitEvent.WaitOne();

            await browser.WaitForReady();
        }

        public static async Task OpenUrl(this Browser browser, string url, ManualResetEvent waitEvent, string containerClassName)
        {
            waitEvent.Reset();
            browser.LoadURL(url);
            Debug.WriteLine("Waiting to load url");
            waitEvent.WaitOne();

            await browser.WaitForReady(containerClassName);
        }

        /// <summary>
        /// Full page 
        /// </summary>
        /// <param name="browser"></param>
        /// <returns></returns>
        static async Task WaitForReady(this Browser browser)
        {
            while (true)
            {
                Debug.WriteLine("waiting for browser ready");
                bool isLoaderExist = false;
                for (int i = 10; i <= 100; i += 10)
                {
                    try
                    {
                        await Task.Delay(i);


                        isLoaderExist = browser.Doc().GetElementByClassName("c-loading-curtain") != null ||
                                        browser.Doc().GetElementByClassName("global-loading") != null ||
                                        browser.Doc().GetElementByClassName("c-loading") != null;

                        Debug.WriteLine($"Is loader exist:{isLoaderExist}");

                        if (isLoaderExist)
                            break;

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }

                }

                if (isLoaderExist)
                    continue;



                Debug.WriteLine("Page is ready");
                return;
            }

        }

        /// <summary>
        /// Wait for specific container
        /// </summary>
        /// <param name="browser"></param>
        /// <returns></returns>
        static async Task WaitForReady(this Browser browser, string containerClassName)
        {
            while (true)
            {
                Debug.WriteLine("waiting for browser ready");
                bool isLoaderExist = false;
                for (int i = 10; i <= 100; i += 10)
                {
                    try
                    {
                        await Task.Delay(i);

                        var container = browser.Doc().GetElementByClassName(containerClassName);



                        Debug.WriteLine($"Is Container null: {container == null}");


                        if (container == null)
                            continue;


                        isLoaderExist = container.GetElementByClassName("c-loading-curtain") != null ||
                                        container.GetElementByClassName("global-loading") != null ||
                                        container.GetElementByClassName("c-loading") != null;

                        Debug.WriteLine($"Is loader exist:{isLoaderExist}");
                        Debug.WriteLine($"Is add to cart available: {browser.AddToCartButton() != null}");

                        if (isLoaderExist)
                            break;

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }

                }

                if (isLoaderExist)
                    continue;



                Debug.WriteLine("Page is ready");
                return;
            }

        }

        public static List<DOMNode> ProductStyles(this Browser browser, ManualResetEvent waitEvent)
        {
            waitEvent.WaitOne();
            var stylesContainer = browser.GetDocument().DocumentElement.GetElementByClassName("ProductStyles");

            if (stylesContainer == null)
                return new System.Collections.Generic.List<DOMNode>();

            return stylesContainer.GetElementsByName("style");
        }

        public static List<DOMNode> AvailableSizes(this Browser browser, ManualResetEvent waitEvent)
        {
            waitEvent.WaitOne();
            var stylesContainer = browser.GetDocument().DocumentElement.GetElementByClassName("ProductSize-group");

            if (stylesContainer == null)
                return new System.Collections.Generic.List<DOMNode>();

            return stylesContainer.GetElementsByName("size").Where(x => !(x as DOMElement).HasAttribute("disabled")).ToList();
        }

        public static void AddToCart(this Browser browser)
        {
            browser.AddToCartButton().Click();

        }

        public static async Task<NavigationResultErrorCode> NavigateGuestCheckout(this Browser browser, ManualResetEvent waitEvent)
        {
            await browser.OpenUrl("https://www.footlocker.com/checkout", waitEvent);

            bool cartIsEmpty = browser.Doc().GetElementByClassName("OrderSummary")?.GetElementByClassName("total") == null;

            if (cartIsEmpty)
                return NavigationResultErrorCode.CartIsEmpty;

            return NavigationResultErrorCode.Success;
        }

        public static async Task<NavigationResultErrorCode> FillCheckoutForm(this Browser browser, CheckoutBillerModel checkoutModel)
        {
            bool cartIsEmpty = browser.Doc().GetElementByClassName("OrderSummary")?.GetElementByClassName("total") == null;

            if (cartIsEmpty)
                return NavigationResultErrorCode.CartIsEmpty;

            await browser.WaitForReady();

            //CONTACT INFO - 1
            await Fill_Checkout_ContactInfo(browser, checkoutModel);

            //PACKAGE OPTIONS - 2
            await Fill_Checkout_PackageOptions(browser, checkoutModel);

            //PAYMENT - 3
           // await Fill_Checkout_Payment(browser, checkoutModel);

            return NavigationResultErrorCode.Success;
        }

        private static async Task Fill_Checkout_PackageOptions(Browser browser, CheckoutBillerModel checkoutModel)
        {
            void _tab()
            {
                KeyParams paramers = new KeyParams(VirtualKeyCode.TAB, ' ');
                browser.KeyDown(paramers);
                browser.KeyUp(paramers);
                Debug.WriteLine("Tab");
            }

            var container = browser.GetDocument().GetElementById("step2");

            var editButton = _getEditButton(); //container.GetElementsByTagName("button").Where(x => x.TextContent.ToLower().Contains("edit")).FirstOrDefault();

            bool editButtonExist = editButton != null;

            if (editButtonExist)
                (editButton as DOMElement).Click();


            var domStreetAddress = browser.GetDocument().GetElementById("ShippingAddress_text_line1");
            await (domStreetAddress as DOMInputElement).SetValue(browser, checkoutModel.StreetAddress, editButtonExist);

            var domUnitNumDom = browser.GetDocument().GetElementById("ShippingAddress_text_line2"); //OPTIONAL FIELD
            await (domUnitNumDom as DOMInputElement).SetValue(browser, checkoutModel.UnitNumber, editButtonExist);


            var domPostalCode = browser.GetDocument().GetElementById("ShippingAddress_text_postalCode");
            await (domPostalCode as DOMInputElement).SetValue(browser, checkoutModel.Zipcode, editButtonExist);

            await browser.WaitForReady(); //TO PREFILL THE CITY AND STATE


            var saveAndContinueButton = container
                .GetElementsByTagName("button").Where(x =>
                    x.TextContent.ToLower().Contains("save & continue")).FirstOrDefault();

            saveAndContinueButton.Click();

            await browser.WaitForReady();


            //FOR PAYMENT CARD

            await Task.Delay(100);
            _tab();
            await Task.Delay(100);
            _tab();
            await Task.Delay(100);
            _tab();

            //await browser.TypeKeys(checkoutModel.CCNumber);
            //_tab();
            //await Task.Delay(100);
            //await browser.TypeKeys(checkoutModel.Card_MM);
            //_tab();
            //await Task.Delay(100);
            //await browser.TypeKeys(checkoutModel.Card_YY);
            //_tab();
            //await Task.Delay(100);
            //await browser.TypeKeys(checkoutModel.Card_CSC);

            DOMNode _getEditButton()
            {
                return container.GetElementsByTagName("button").Where(x => x.TextContent.ToLower().Contains("edit")).FirstOrDefault(); ;
            }

        }

        private static async Task Fill_Checkout_Payment(Browser browser, CheckoutBillerModel checkoutModel)
        {
            var container = browser.GetDocument().GetElementById("step3");

            browser.GetDocument().GetElementById("PaymentGateway_radio_gateway_creditcard").Click();

            foreach (var frameId in browser.GetFramesIds())
            {
                var frameDom = browser.GetDocument(frameId);
                var input = frameDom.DocumentElement.GetElementByTagName("input");

                if (input != null)
                {
                    input.Focus();
                    input.Click();
                }


                if (input != null && input.HasAttribute("id") && input.GetAttribute("id") == "encryptedCardNumber")
                    await (input as DOMInputElement).SetValueSlow(browser, checkoutModel.CCNumber);


                if (input != null && input.HasAttribute("id") && input.GetAttribute("id") == "encryptedExpiryMonth")
                    await (input as DOMInputElement).SetValueSlow(browser, checkoutModel.Card_MM);


                if (input != null && input.HasAttribute("id") && input.GetAttribute("id") == "encryptedExpiryYear")
                    await (input as DOMInputElement).SetValueSlow(browser, checkoutModel.Card_YY);

                if (input != null && input.HasAttribute("id") && input.GetAttribute("id") == "encryptedSecurityCode")
                    await (input as DOMInputElement).SetValueSlow(browser, checkoutModel.Card_CSC);

            }


            await browser.WaitForReady();

        }

        private static async Task Fill_Checkout_ContactInfo(Browser browser, CheckoutBillerModel checkoutModel)
        {

            var container = browser.GetDocument().GetElementById("step1");

            var editContactInfoButton = container.GetElementsByTagName("button").Where(x => x.TextContent.ToLower().Contains("edit")).FirstOrDefault();

            bool editButtonExist = editContactInfoButton != null;

            if (editButtonExist)
                (editContactInfoButton as DOMElement).Click();

            var firstnameDom = browser.GetDocument().GetElementById("ContactInfo_text_firstName");
            await (firstnameDom as DOMInputElement).SetValue(browser, checkoutModel.FirstName, editButtonExist);

            var lastnameDom = browser.GetDocument().GetElementById("ContactInfo_text_lastName");
            await (lastnameDom as DOMInputElement).SetValue(browser, checkoutModel.LastName, editButtonExist);



            var emailDom = browser.GetDocument().GetElementById("ContactInfo_email_email");
            await (emailDom as DOMInputElement).SetValue(browser, checkoutModel.Email, editButtonExist);


            var phoneDom = browser.GetDocument().GetElementById("ContactInfo_tel_phone");
            await (phoneDom as DOMInputElement).SetValue(browser, checkoutModel.Telephone, editButtonExist);


            var contactInfoSaveAndContinueButton = container
                .GetElementsByTagName("button").Where(x =>
                    x.TextContent.ToLower().Contains("save & continue")).FirstOrDefault();

            contactInfoSaveAndContinueButton.Click();

            await browser.WaitForReady();
        }


    }
}
