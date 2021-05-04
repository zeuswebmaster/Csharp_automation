using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoCheckoutBot.FootLocker.Models;
using Newtonsoft.Json;

namespace AutoCheckoutBot.FootLocker
{
    public class SampleData
    {
        public static CheckoutBillerModel SampleBiller()
        {
            var jsonString = File.ReadAllText("Biller.json");

            return JsonConvert.DeserializeObject<CheckoutBillerModel>(jsonString);

            
        }

        public static List<ProxyModel> Proxies()
        {
            string json = File.ReadAllText("ProxyList.json");
            return JsonConvert.DeserializeObject<List<ProxyModel>>(json);
        }
    }
}
