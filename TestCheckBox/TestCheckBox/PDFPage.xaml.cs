using AppBase.ViewModels;
using AppBaseNamespace;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
                       
            //SaveFile(item);
            BindingContext = new PDFPageViewModel(app, Navigation);
        }

        private void SaveFile(PDFPageItemViewModel resource)
        {
            string dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), resource.Language);

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            string fileName = Path.Combine(dir, resource.FileName);
            if (!File.Exists(fileName))
            {
                File.Create(fileName);
                resource.FilePath = fileName;
                using (var client = new WebClient())
                {
                    try
                    {
                        //client.DownloadFile(resource.Path, fileName);
                        client.DownloadStringCompleted += (s, e) => {
                            var text = e.Result; // get the downloaded text                            
                            File.WriteAllText(fileName, text); // writes to local storage
                            Console.WriteLine(text);
                        };
                        var url = new Uri(resource.Path); // Html home page
                        client.Encoding = Encoding.UTF8;
                        client.DownloadStringAsync(url);
                    }
                    catch (Exception e)
                    {

                    }

                }
            }
        }
    }
}