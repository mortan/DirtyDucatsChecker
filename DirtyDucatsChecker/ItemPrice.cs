using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirtyDucatsChecker
{
    public class ItemPrice
    {
        public string Name { get; set; }

        public double PlatinumPrice { get; set; }

        public double DucatsPrice { get; set; }

        public double DucatsRatio => Math.Round(DucatsPrice / PlatinumPrice, 2);

        public override string ToString() => $"{Name}|{PlatinumPrice}|{DucatsPrice}|{DucatsRatio}";
    }
}
