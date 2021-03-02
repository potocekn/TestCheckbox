using System.Collections.Generic;
using AppBaseNamespace.ViewModels;
using System.Linq;
using Xamarin.Forms;

namespace AppBaseNamespace
{
    internal class CheckPageViewModel
    {
        public List<ItemViewModel> Items { get; }
        public Command ChangingCheckBox { get; }
        CheckPage checkPage { get; }
        public CheckPageViewModel(IEnumerable<string> items, CheckPage page)
        {
            Items = items
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => new ItemViewModel() {
                    IsChecked = false,
                    Value = x,
                    CheckedChangedCommand = new Command(() => {
                        page.DisplayAlert("","Checked changed","OK");
                    })
                })
                .ToList();
            ChangingCheckBox = new Command(() => {
                page.DisplayAlert("Test title", "Test body", "OK");
            });
            checkPage = page;
        }

     
    }
}