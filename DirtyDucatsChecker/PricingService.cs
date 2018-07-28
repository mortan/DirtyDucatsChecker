using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DirtyDucatsChecker
{
    public class PricingService
    {
        private Dictionary<string, ItemPrice> itemPrices = new Dictionary<string, ItemPrice>();

        public PricingService()
        {
            WebClient webClient = new WebClient();
            string json = webClient.DownloadString("https://tenno.zone/data/");
            var data = TennoZoneData.FromJson(json);

            foreach (var part in data.Parts)
            {
                var itemPrice = new ItemPrice { Name = part.Name, DucatsPrice = part.Ducats ?? 0 };

                var price = data.Prices.FirstOrDefault(x => x.PartId == part.Id);
                if (price != null)
                {
                    itemPrice.PlatinumPrice = price.PriceInfo.Median;
                    itemPrices.Add(part.Name.ToLower(), itemPrice);
                }
            }
        }

        public List<ItemPrice> GetPricesByNames(IEnumerable<string> names)
        {
            var result = new List<ItemPrice>();
            foreach (var name in names)
            {
                if (itemPrices.TryGetValue(name.ToLower(), out ItemPrice itemPrice))
                {
                    result.Add(itemPrice);
                }
            }

            return result;
        }
    }
}
