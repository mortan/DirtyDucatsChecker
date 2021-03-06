using System;
using System.Linq;
using System.Windows;
using Stylet;

namespace DirtyDucatsChecker.Pages
{
    public class ShellViewModel : Screen
    {
        private readonly PricingService pricingService;
        private ClipboardMonitor monitor;

        public BindableCollection<ItemPrice> Items { get; set; }

        public ShellViewModel(PricingService pricingService)
        {
            Items = new BindableCollection<ItemPrice>();

            monitor = new ClipboardMonitor();
            monitor.ClipboardData += (object sender, RoutedEventArgs e) =>
            {
                if (monitor.ClipboardContainsText)
                {
                    var lines = monitor.ClipboardText.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    Items.Clear();
                    Items.AddRange(pricingService.GetPricesByNames(lines).OrderByDescending(x => x.DucatsRatio));
                }
            };
        }

        protected override void OnClose()
        {
            monitor.Close();
        }
    }
}
