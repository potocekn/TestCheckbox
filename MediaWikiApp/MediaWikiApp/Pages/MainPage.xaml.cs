using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppBaseNamespace.ViewModels;
using Xamarin.Forms;

namespace AppBaseNamespace
{
    /// <summary>
    /// Class representing the main page of the application.
    /// </summary>
    public partial class MainPage : ContentPage
    {
        public MainPage(App app, string previouslyChecked)
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel(this, app, Navigation, previouslyChecked);
        }
    }
}
