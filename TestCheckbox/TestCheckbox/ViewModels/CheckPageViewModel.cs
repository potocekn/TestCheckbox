using System.Collections.Generic;
using TestCheckbox.ViewModels;
using System.Linq;
using Xamarin.Forms;

namespace TestCheckbox
{
    internal class CheckPageViewModel
    {
        public List<ItemViewModel> Items { get; }
        public Command ChangingCheckBox { get; }
        public CheckPageViewModel(IEnumerable<string> items, CheckPage page)
        {
            Items = items
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => new ItemViewModel() {
                    IsChecked = false,
                    Value = x
                })
                .ToList();
            ChangingCheckBox = new Command(() => {
                page.DisplayAlert("Test title", "Test body", "OK");
            });
        }
    }
}