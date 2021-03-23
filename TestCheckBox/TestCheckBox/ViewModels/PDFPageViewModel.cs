using AppBaseNamespace;
using Plugin.XamarinFormsSaveOpenPDFPackage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppBase.ViewModels
{
    class PDFPageViewModel
    {
        App app { get; set; }

        public List<PDFPageItemViewModel> Items { get; set; }

        public PDFPageViewModel(App app, INavigation navigation)
        {
            this.app = app;
            Items = new List<PDFPageItemViewModel>();
            string dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "English");
            string fileName = Path.Combine(dir, "test.pdf");
            PDFPageItemViewModel item = new PDFPageItemViewModel() {
                Language = "English",
                ResourceName = "Test Resource",
                FileName = "test.pdf",
                Path = "http://www.4training.net/mediawiki/images/a/af/Gods_Story_%28five_fingers%29.pdf",
                FilePath = fileName               
            };
            //SaveFileAsync(item.Path, item.FilePath);
            item.OpenResource = new Command(() => { navigation.PushAsync(new pdfjsPage(item.FilePath)); });
            Items.Add(item);
           //SaveFile(item);
        }


        async void SaveFileAsync(string url, string filePath)
        {
            using (var httpClient = new HttpClient())
            {
                var stream = Task.Run(() => httpClient.GetStreamAsync(url)).Result;

                using (var memoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream);
                    File.WriteAllBytes(filePath, memoryStream.ToArray());
                }
            }
        }

        public async void LoadAndOpenResource(PDFPageItemViewModel resource)
        {
            //var httpClient = new HttpClient();
            //var stream = await httpClient.GetStreamAsync("http://www.4training.net/mediawiki/images/a/af/Gods_Story_%28five_fingers%29.pdf");

            using (var memoryStream = new MemoryStream())
            {
               try
                {
                    if (File.Exists(resource.FilePath))
                    {
                        using (FileStream fileStream = File.OpenRead(resource.FilePath))
                        {
                            MemoryStream memStream = new MemoryStream();
                            memStream.SetLength(fileStream.Length);
                            fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);
                            await CrossXamarinFormsSaveOpenPDFPackage.Current.SaveAndView(resource.FileName, "application/pdf", memoryStream, PDFOpenContext.InApp);
                        }                       

                    }

                }
                catch (Exception e)
               { 
               }

            }
        }

    }    
}
