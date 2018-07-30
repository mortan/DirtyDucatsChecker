using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace DirtyDucatsChecker
{
    public class PricingService
    {
        private List<ItemPrice> itemPrices = new List<ItemPrice>();
        private readonly int levenDistanceThreshold = 5;

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
                    itemPrices.Add(itemPrice);
                }
            }
        }

        public IReadOnlyCollection<ItemPrice> GetAllPrices()
        {
            return itemPrices.AsReadOnly();
        }

        public List<ItemPrice> GetPricesByNames(IEnumerable<string> names, bool useFuzzySearch)
        {
            var result = new List<ItemPrice>();
            foreach (var name in names)
            {
                List<ItemPrice> matches;
                if (useFuzzySearch)
                {
                    matches = new List<ItemPrice>();
                    foreach (var itemPrice in itemPrices)
                    {
                        int levenDistance = Levenshtein.Calculate(name.ToLower(), itemPrice.Name.ToLower());
                        if (levenDistance <= 5)
                        {
                            itemPrice.FuzzySearchScore = levenDistance;
                            matches.Add(itemPrice);
                        }
                    }
                }
                else
                {
                    matches = itemPrices.Where(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList();
                    matches.ForEach(x => x.FuzzySearchScore = 0);
                }

                if (matches.Any())
                {
                    result.AddRange(matches);
                }
            }

            return result;
        }

        public List<ItemPrice> GetPricesByNamesFuzzy(IEnumerable<string> names)
        {
            var result = new List<ItemPrice>();
            foreach (var name in names)
            {
                foreach (var itemPrice in itemPrices)
                {
                    int lastMatchValue = int.MaxValue;
                    int levenDistance = Levenshtein.Calculate(name.ToLower(), itemPrice.Name.ToLower());
                    if (levenDistance < levenDistanceThreshold && levenDistance < lastMatchValue)
                    {

                    }
                }
            }
            
        }

    }
}
