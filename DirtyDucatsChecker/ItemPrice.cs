using System;

namespace DirtyDucatsChecker
{
    public class ItemPrice
    {
        public string Name { get; set; }

        public double PlatinumPrice { get; set; }

        public double DucatsPrice { get; set; }

        public double DucatsRatio => Math.Round(DucatsPrice / PlatinumPrice, 2);

        public int FuzzySearchScore { get; set; }

        public override string ToString() => $"{Name}|{PlatinumPrice}|{DucatsPrice}|{DucatsRatio}";
    }
}
