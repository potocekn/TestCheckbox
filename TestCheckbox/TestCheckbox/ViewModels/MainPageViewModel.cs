using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using TestCheckbox.RequestsAndResponses;
using Xamarin.Forms;

namespace TestCheckbox.ViewModels
{
    public class MainPageViewModel
    {
        private string[] textCollection = new string[] { "English", "Czech", "German"};
        private string[] languages = new string[] { "English", "German", "Czech", "French", "Chinese"};
        List<string> ResourceLanguages { get; set; }
        public string Title { get; }
        public Command LoadCheckboxes { get; }
        public Command GoToSettings { get; }        

        public MainPageViewModel(Page page, App app,INavigation navigation)
        {
            //ResourceLanguages = GetAllLanguages();
            LoadCheckboxes = new Command(() => {
                navigation.PushAsync(new CheckPage(textCollection));
            });
            GoToSettings = new Command(() => {
                navigation.PushAsync(new SettingsPage(languages));
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
