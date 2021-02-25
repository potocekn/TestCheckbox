using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCheckbox.ViewModels;
using Xamarin.Forms;

namespace TestCheckbox
{
    public partial class MainPage : ContentPage
    {
        public MainPage(App app, string previouslyChecked)
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel(this, app, Navigation, previouslyChecked);
        }
    }
}
