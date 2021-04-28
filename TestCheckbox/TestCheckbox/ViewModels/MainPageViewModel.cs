using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using AppBaseNamespace.RequestsAndResponses;
using Xamarin.Forms;
using AppBase;

namespace AppBaseNamespace.ViewModels
{
    /// <summary>
    /// Class representing the view model of the main page. Class remembers its title and has commands that redirect main page to settings or resources.
    /// </summary>
    public class MainPageViewModel
    {
        private List<string> textCollection = new List<string> { "English", "Czech", "German"};
        private List<string> languages = new List<string> {  "Czech", "French", "Chinese"};
        private List<string> shortcuts = new List<string> {  "cs", "fr", "zh-Hans"};
        public string previouslyChecked = "";

        List<string> ResourceLanguages { get; set; }
        public string Title { get; }
        public Command LoadCheckboxes { get; }
        public Command GoToSettings { get; }
        public Command GoToResources { get; }

        public MainPageViewModel(Page page, App app,INavigation navigation, string previouslyChecked)
        {
            this.previouslyChecked = previouslyChecked;
            //ResourceLanguages = GetAllLanguages();
            LoadCheckboxes = new Command(() => {
                navigation.PushAsync(new ResourceFormatSettingsPage(app, this));
            });
            GoToSettings = new Command(() => {
                navigation.PushAsync(new SettingsPage(app, this));
            });
            GoToResources = new Command(() => {
                navigation.PushAsync(new ResourcesPage(app, navigation));
            });

        }

        List<string> GetAllLanguages()
        {
            List<string> result = new List<string>();
            Request request = new Request(RequestType.ALL_AVAILABLE_LANGUAGES, null);
            byte[] bytes = SendMessage(System.Text.Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(request)));
            var responseBytesCleaned = CleanMessage(bytes);
            ResponseAllAvailableLanguages response = JsonConvert.DeserializeObject<ResponseAllAvailableLanguages>(responseBytesCleaned);
            if (response.Status == ResponseStatus.OK)
            {
                result = response.Languages;
            }

            return result;
        }
        private static byte[] SendMessage(byte[] messageBytes)
        {
            const int bytesize = 1024 * 1024;
            try  
            {
                System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient("127.0.0.1", 1234); // Create a new connection  
                NetworkStream stream = client.GetStream();

                stream.Write(messageBytes, 0, messageBytes.Length); // Write the bytes  
                messageBytes = new byte[bytesize]; // Clear the message   

                // Receive the stream of bytes  
                stream.Read(messageBytes, 0, messageBytes.Length);

                // Clean up  
                stream.Dispose();
                client.Close();
            }
            catch (Exception e) // Catch exceptions  
            {
                Console.WriteLine(e.Message);
            }

            return messageBytes; // Return response  
        }

        private static string CleanMessage(byte[] bytes)
        {
            string message = Encoding.Unicode.GetString(bytes);

            string messageToPrint = null;
            foreach (var nullChar in message)
            {
                if (nullChar != '\0')
                {
                    messageToPrint += nullChar;
                }
            }
            return messageToPrint;
        }

    }
}
