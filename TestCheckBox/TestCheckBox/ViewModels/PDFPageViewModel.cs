using AppBaseNamespace;
using Plugin.XamarinFormsSaveOpenPDFPackage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using PCLStorage;
using System.Diagnostics;

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
            //SaveFile(item);
            //SaveFileAsync(item.Path, item.FilePath);
            item.OpenResource = new Command(() => { navigation.PushAsync(new pdfjsPage(item.FilePath)); });
            item.ShareResource = new Command(async () => { await OnButtonClickShareAsync(item.FilePath); });
            Items.Add(item);
           
            fileName = Path.Combine(dir, "test2.pdf");
            PDFPageItemViewModel item2 = new PDFPageItemViewModel()
            {
                Language = "English",
                ResourceName = "Test Resource 2",
                FileName = "test2.pdf",
                Path = "http://www.4training.net/mediawiki/images/8/8b/Baptism.pdf",
                FilePath = fileName
            };
            //SaveFile(item2);
            item2.OpenResource = new Command(() => { navigation.PushAsync(new pdfjsPage(item2.FilePath)); });
            item2.ShareResource = new Command(async () => { await OnButtonClickShareAsync(item2.FilePath); });
            Items.Add(item2);
            
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
                            Debug.WriteLine(text);
                        };
                        var url = new Uri(resource.Path); // Html home page
                        client.Encoding = Encoding.UTF8;
                        client.DownloadDataAsync(url);
                    }
                    catch (Exception e)
                    {

                    }

                }
            }
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

        public async Task OnButtonClickShareAsync(string filePath)
        {           
            
            await Share.RequestAsync(new ShareFileRequest
            {
                Title = "Share",
                File = new ShareFile(filePath)
            });
            
        }

    }    
}
