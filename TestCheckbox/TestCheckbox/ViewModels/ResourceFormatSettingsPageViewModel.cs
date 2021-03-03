using System.Collections.Generic;
using AppBaseNamespace.ViewModels;
using System.Linq;
using Xamarin.Forms;

namespace AppBaseNamespace
{
    internal class ResourceFormatSettingsPageViewModel
    {
        public List<ItemViewModel> Items { get; }
        public Command ChangingCheckBox { get; }
        ResourceFormatSettingsPage checkPage { get; }
        public ResourceFormatSettingsPageViewModel(IEnumerable<string> items, ResourceFormatSettingsPage page)
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