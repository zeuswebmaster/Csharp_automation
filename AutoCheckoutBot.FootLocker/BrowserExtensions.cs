using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoCheckoutBot.FootLocker.Enum;
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

        static async Task WaitForReady(this Browser browser)
        {
            while (true)
            {
                Debug.WriteLine("waiting for browser ready");
                bool isLoaderExist = false;
                for (int i = 10; i <= 100; i+=10)
                {
                    try
                    {
                        await Task.Delay(i);


                        isLoaderExist = browser.Doc().GetElementByClassName("c-loading-curtain") != null ||
                                        browser.Doc().GetElementByClassName("global-loading") != null || 
                                        browser.Doc().GetElementByClassName("c-loading") != null;

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
            await browser.OpenUrl("https://www.footlocker.com/checkout",waitEvent);

            bool cartIsEmpty = browser.Doc().GetElementByClassName("OrderSummary")?.GetElementByClassName("total") == null;

            if (cartIsEmpty)
                return NavigationResultErrorCode.CartIsEmpty;
            
            return NavigationResultErrorCode.Success;
        }

    }
}
