using AppBase.ViewModels;
using AppBaseNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppBase
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PDFPage : ContentPage
    {
        public PDFPage(App app)
        {
            InitializeComponent();
            BindingContext = new PDFPageViewModel(app);
        }
    }
}