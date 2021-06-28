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
    /// <summary>
    /// Class used for the step of the first run configuration that is responsible for choosing the languages of resources.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResourceLanguagesFirstRunPage : ContentPage
    {
        public ResourceLanguagesFirstRunPage(App app, List<string> languages)
        {
            InitializeComponent();
            BindingContext = new ResourceLanguagesFirstRunPageViewModel(app, Navigation, languages);
        }

        /// <summary>
        /// Method used when a checkbox changes its checked status.
        /// </summary>
        /// <param name="sender">Checkbox that changed status</param>
        /// <param name="e">Event arguments</param>
        void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            (BindingContext as ResourceLanguagesFirstRunPageViewModel).OnCheckBoxCheckedChanged(sender, e);
        }

        /// <summary>
        /// Method used for checking a checkbox when the label next to is was clicked on.
        /// </summary>
        /// <param name="sender">Label that was clicked on</param>
        /// <param name="e">Event arguments</param>
        public void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            (BindingContext as ResourceLanguagesFirstRunPageViewModel).TapGestureRecognizer_Tapped(sender, e);
        }
    }
}