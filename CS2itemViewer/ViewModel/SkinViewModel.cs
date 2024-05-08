using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CS2itemViewer;
using CS2itemViewer.Model;
using CS2itemViewer.Services;
using CS2itemViewer.ViewModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CS2itemViewer.ViewModel
{
    public partial class SkinViewModel : BaseViewModel
    {
        public ObservableCollection<Skin> Skins { get; } = new();
        ISkinService SkinService;
        IConnectivity connectivity;

        public SkinViewModel(ISkinService skinService, IConnectivity connectivity)
        {
            Title = "CS Item Viewer";

           this.SkinService = skinService;
           this.connectivity = connectivity;
        }

        [ObservableProperty]
        bool isRefreshing;

        [RelayCommand]

        public async void GetSkins()
        {

            // Define the API endpoint URL
            string apiUrl = "https://www.steamwebapi.com/steam/api/inventory?key=0BZBWV7TVZUYRB8J&steam_id=76561198269412096";

            // Make an HTTP request to the API
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode(); // Throw an exception if the request fails
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Parse the JSON response
                    JArray items = JArray.Parse(responseBody);

                    // Iterate over each item in the response
                    foreach (JToken item in items)
                    {
                        // Extract the required fields
                        string marketName = item["marketname"].ToString();
                        string itemName = item["itemname"].ToString();
                        double priceLatest = Convert.ToDouble(item["pricelatest"]);
                        string imageUrl = item["image"].ToString();

                        // Display the extracted fields
                        Console.WriteLine("Market Name: " + marketName);
                        Console.WriteLine("Item Name: " + itemName);
                        Console.WriteLine("Latest Price: $" + priceLatest);
                        Console.WriteLine("Image URL: " + imageUrl);
                        Console.WriteLine();
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine("Error making HTTP request: " + ex.Message);
                }
                catch (JsonException ex)
                {
                    Console.WriteLine("Error parsing JSON response: " + ex.Message);
                }
            }

        }
        [RelayCommand]
        async Task GoToDetails(Skin skin)
        {
            if (skin == null)
                return;

            await Shell.Current.GoToAsync(nameof(DetailsPage), true, new Dictionary<string, object>
            {
                {"Skin", skin }
            });
        }






    }
}
