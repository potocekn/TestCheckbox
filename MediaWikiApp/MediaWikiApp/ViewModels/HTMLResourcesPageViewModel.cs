using AppBase.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppBase.ViewModels
{
    /// <summary>
    /// Class representing the view model of the page that contains list of all html resources.
    /// The list of resources is created based on the records in the database.
    /// </summary>
    class HTMLResourcesPageViewModel
    {
        public List<HtmlResource> Items { get; set; }

        public HTMLResourcesPageViewModel(INavigation navigation, List<HtmlRecord> records)
        {
            Items = new List<HtmlResource>();
            foreach (var item in records)
            {
                var resource = new HtmlResource {
                    PageName = item.PageName,
                    OpenResource = new Command(() => { navigation.PushAsync(new HTMLPage(item)); })
                };

                Items.Add(resource);
            }
        }        
    }
}
