using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Stylet;

namespace DirtyDucatsChecker.Pages
{
    public class ShellViewModel : Screen
    {
        private readonly PricingService pricingService;
        private ClipboardMonitor monitor;
        private readonly int levenDistanceThreshold = 5;

        public BindableCollection<ItemPrice> Items { get; set; } = new BindableCollection<ItemPrice>();
        public ICollectionView ItemView { get; set; }
        public bool UseFuzzySearch { get; set; } = true;
        public string SearchText { get; set; }
        public string ClipboardText { get; set; }

        public ShellViewModel(PricingService pricingService)
        {
            this.pricingService = pricingService;

            Items.AddRange(pricingService.GetAllPrices());
            ItemView = CollectionViewSource.GetDefaultView(Items);
            ItemView.SortDescriptions.Clear();
            ItemView.SortDescriptions.Add(new SortDescription(nameof(ItemPrice.FuzzySearchScore), ListSortDirection.Ascending));
            ItemView.SortDescriptions.Add(new SortDescription(nameof(ItemPrice.DucatsRatio), ListSortDirection.Descending));
            ItemView.Filter = ItemPriceBySearchTextFilter;

            ToggleClipboardMonitor();
        }

        public void ToggleClipboardMonitor()
        {
            if (monitor == null)
            {
                monitor = new ClipboardMonitor();
                monitor.ClipboardData += ClipboardChangeHandler;
            }
            else
            {
                monitor.Close();
                monitor.ClipboardData -= ClipboardChangeHandler;
                monitor = null;
            }
        }

        private void ClipboardChangeHandler(object sender, RoutedEventArgs e)
        {
            if (monitor.ClipboardContainsText)
            {
                ClipboardText = monitor.ClipboardText;
            }
        }

        private bool ItemPriceBySearchTextFilter(object item)
        {
            ItemPrice itemPrice = item as ItemPrice;
            if (itemPrice == null)
            {
                return false;
            }

            itemPrice.FuzzySearchScore = 0;

            if (string.IsNullOrEmpty(SearchText))
            {
                return true;
            }

            if (UseFuzzySearch)
            {
                int levenDistance = Levenshtein.Calculate(SearchText.ToLower(), itemPrice.Name.ToLower());
                itemPrice.FuzzySearchScore = levenDistance;

                return levenDistance <= levenDistanceThreshold;
            }

            return itemPrice.Name.Equals(SearchText, StringComparison.OrdinalIgnoreCase);
        }

        private bool ItemPriceByClipboardTextFilter(object item)
        {
            ItemPrice itemPrice = item as ItemPrice;
            if (itemPrice == null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(ClipboardText))
            {
                return true;
            }

            itemPrice.FuzzySearchScore = 0;

            var lines = ClipboardText.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                if (UseFuzzySearch)
                {
                    int levenDistance = Levenshtein.Calculate(line, itemPrice.Name.ToLower());
                    itemPrice.FuzzySearchScore = levenDistance;

                    if (levenDistance <= levenDistanceThreshold)
                    {
                        return true;
                    }
                }
                else
                {
                    bool matchesOrdinal = itemPrice.Name.Equals(SearchText, StringComparison.OrdinalIgnoreCase);
                    if (matchesOrdinal)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(UseFuzzySearch):
                case nameof(SearchText):
                {
                    ItemView.Filter = ItemPriceBySearchTextFilter;
                    ItemView.Refresh();
                    break;
                }
                case nameof(ClipboardText):
                {
                    ItemView.Filter = ItemPriceByClipboardTextFilter;
                    ItemView.Refresh();
                    break;
                }
            }
        }

        protected override void OnClose()
        {
            if (monitor != null)
            {
                monitor.Close();
                monitor.ClipboardData -= ClipboardChangeHandler;
                monitor = null;
            }
        }
    }
}
