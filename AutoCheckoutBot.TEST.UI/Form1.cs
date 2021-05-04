using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotNetBrowser;
using DotNetBrowser.DOM;
using DotNetBrowser.Events;
using DotNetBrowser.WinForms;
using AutoCheckoutBot.FootLocker;

namespace AutoCheckoutBot.TEST.UI
{
    public partial class Form1 : Form
    {
        private readonly Browser _browser;
        ManualResetEvent waitEvent = new ManualResetEvent(false);
        public Form1()
        {
            InitializeComponent();


            BrowserView browserView = new WinFormsBrowserView();
            var browserViewControl = (Control) browserView;
            _browser = browserView.Browser;
            browserViewControl.Dock = DockStyle.Fill;
            panel_browser_container.Controls.Add(browserViewControl);
            
            // Create Browser instance.
            // Register an event that will be fired when web page's frame has been completely loaded.
            _browser.FinishLoadingFrameEvent += delegate (object sender, FinishLoadingEventArgs e)
            {
                //Console.Out.WriteLine("FinishLoadingFrame: URL = " + e.ValidatedURL + ", IsMainFrame = " + e.IsMainFrame);
                if (e.IsMainFrame)
                {
                    Console.Out.WriteLine("========================== READY ======================");
                    waitEvent.Set();
                }
            };
            ////// Register an event that will be fired when web page's frame has been started loading.
            ////_browser.StartLoadingFrameEvent += delegate (object sender, StartLoadingArgs e)
            ////{
            ////    Console.Out.WriteLine("StartLoadingFrame: URL = " + e.ValidatedURL + ", IsMainFrame = " + e.IsMainFrame);
            ////};
            ////// Register an event that will be fired when document of main frame has been completely loaded.
            ////_browser.DocumentLoadedInMainFrameEvent += delegate (object sender, LoadEventArgs e)
            ////{
            ////    Console.Out.WriteLine("DocumentLoadedInMainFrame");
            ////};



          // _browser.OpenUrl("https://www.footlocker.com/product/fila-disruptor-ii-mens/00705422.html", waitEvent).Wait();
            //browserView.Browser.LoadURL("https://www.footlocker.com/product/fila-disruptor-ii-mens/00705422.html");
        }
        private async void btnReloadBrowser_Click(object sender, EventArgs e)
        {
          await  _browser.OpenUrl("https://www.footlocker.com/product/fila-disruptor-ii-mens/00705422.html", waitEvent);
            //_browser.LoadURL("https://www.footlocker.com/product/fila-disruptor-ii-mens/00705422.html");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private async void btnTest1_Click(object sender, EventArgs e)
        {
            await _browser.OpenUrl("https://www.footlocker.com/product/fila-disruptor-ii-mens/00705422.html", waitEvent, "Page-body");

            bool isAddToCartAvaialble =   _browser.IsAddToCartButtonAvailable(waitEvent);

            var styles = _browser.ProductStyles(waitEvent);
            styles[1].Click();

            var sizes = _browser.AvailableSizes(waitEvent);
            sizes[1].Click();

            _browser.AddToCart();

         var result = await  _browser.NavigateGuestCheckout(waitEvent);

            MessageBox.Show(result.ToString());
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            using (var browserView = new WinFormsBrowserView())
            {
                var browser = browserView.Browser;
                ManualResetEvent xWaitEvent = new ManualResetEvent(false);

                browser.FinishLoadingFrameEvent += delegate (object s, FinishLoadingEventArgs eventArgs)
                {
                    //Console.Out.WriteLine("FinishLoadingFrame: URL = " + e.ValidatedURL + ", IsMainFrame = " + e.IsMainFrame);
                    if (eventArgs.IsMainFrame)
                    {
                        Console.Out.WriteLine("========================== READY ======================");
                        xWaitEvent.Set();
                    }
                };

                await browser.OpenUrl("https://www.footlocker.com/product/fila-disruptor-ii-mens/00705422.html", xWaitEvent);

                bool isAddToCartAvaialble = browser.IsAddToCartButtonAvailable(xWaitEvent);

                var styles = browser.ProductStyles(xWaitEvent);
                styles[1].Click();

                var sizes = browser.AvailableSizes(xWaitEvent);
                sizes[1].Click();

                browser.AddToCart();

                var result = await browser.NavigateGuestCheckout(xWaitEvent);

                MessageBox.Show(result.ToString());

            }
        }
    }
}
