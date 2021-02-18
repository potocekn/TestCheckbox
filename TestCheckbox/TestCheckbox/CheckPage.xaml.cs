using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TestCheckbox
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CheckPage : ContentPage
    {
        void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            this.DisplayAlert("", "Checked changed", "OK");
        }
        public CheckPage(IEnumerable<string> items)
        {
            InitializeComponent();
            BindingContext = new CheckPageViewModel(items, this);
        }
    }
}