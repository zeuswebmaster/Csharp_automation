using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DFoXDotNeBrowser;

namespace AutoCheckoutBot.FootLocker
{
    public abstract class BrowserBase
    {
        public BrowserBase()
        {

            DFoX_DotNetBrowser.DFoXModificaMemoria();
        }
    }
}
