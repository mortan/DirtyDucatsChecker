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
        #region Performance improvement by RandomStrangler

        private Dictionary<string, ItemPrice> itemPrices = new Dictionary<string, ItemPrice>(StringComparer.OrdinalIgnoreCase);

        #endregion

        #region Constructor Region

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
                    itemPrices.Add(part.Name, itemPrice);
                }
            }
        }

        #endregion

        #region Public Methods 

        #region GetPricesByNames Method

        public List<ItemPrice> GetPricesByNames(IEnumerable<string> names)
        {
            #region Method Body
            var result = new List<ItemPrice>();
            foreach (var name in names)
            {
                if (itemPrices.TryGetValue(name, out ItemPrice itemPrice))
                {
                    result.Add(itemPrice);
                }
            }

            return result;
            #endregion
        }

        #endregion

        #endregion
    }
}
