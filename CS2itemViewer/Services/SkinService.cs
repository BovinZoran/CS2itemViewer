using CS2itemViewer.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CS2itemViewer.Services
{
    public class SkinService : ISkinService
    {
        List<Skin>? skinList = new();
        HttpClient httpClient;
        //private string _steamID = "76561198268749335";  //Standaard Zoran's Steam-ID
        private string _steamID = "76561199561947824";

        public SkinService()
        {
            this.httpClient = new HttpClient();
        }

        public void UpdateSteamID(string newSteamID)
        {
            _steamID = newSteamID;
            // Clear skinList so it will be reloaded with the new steamID
            skinList.Clear();
        }

        public async Task<List<Skin>?> GetSkins()
        {
            //J1BAN31YCBSEJLLG, 35HX3C23UD6M37JF
            string apiKey = "35HX3C23UD6M37JF";
            //string steamID = "76561198350557801";//ragnar
            //string steamID = "76561199561947824";//bot
            // Define the API endpoint URL
            // nog aanpassen dat ook de id kan ingegeven worden
            string apiUrl = $"https://www.steamwebapi.com/steam/api/inventory?key={apiKey}&steam_id={_steamID}";
            // https://www.steamwebapi.com/steam/api/inventory?key=J1BAN31YCBSEJLLG&steam_id=76561199561947824
            // Use JsonSerializerOptions to handle case insensitivity if needed

            //var sourceGenOptions = new JsonSerializerOptions//waarom dit
            //{
            //    PropertyNameCaseInsensitive = true
            //};

            // Check if the skinList is already populated
            if (skinList?.Count > 0)
            {
                return skinList;
            }

            try
            {
                // Make an HTTP GET request and deserialize the response to a dynamic object
                var response = await httpClient.GetStringAsync(apiUrl);
                var jsonDocument = JsonDocument.Parse(response);
                var skinsArray = jsonDocument.RootElement.EnumerateArray();

                foreach (var skinElement in skinsArray)
                {
                    var marketName = skinElement.GetProperty("marketname").GetString();
                    var image = skinElement.GetProperty("image").GetString();
                    var itemName = skinElement.GetProperty("itemname").GetString();
                    var priceLatestSell = skinElement.GetProperty("pricelatestsell").GetDouble();
                    var color = skinElement.GetProperty("color").GetString();

                    string descriptionFloat = string.Empty;
                    string descriptionText = string.Empty;

                    // Display the extracted fields
                    Console.WriteLine("Market Name: " + marketName);
                    Console.WriteLine("Item Name: " + itemName);
                    Console.WriteLine("Latest Price: $" + priceLatestSell);
                    Console.WriteLine("Image URL: " + image);
                    Console.WriteLine("COLOR ===> " + color);
                    Console.WriteLine();

                    if (skinElement.TryGetProperty("descriptions", out JsonElement descriptionsElement) && descriptionsElement.ValueKind == JsonValueKind.Array)
                    {
                        var descriptions = descriptionsElement.EnumerateArray().ToList();
                        if (descriptions.Count > 0)
                        {
                            descriptionFloat = descriptions[0].GetProperty("value").GetString();
                        }
                        if (descriptions.Count > 2)
                        {
                            if (marketName.StartsWith("StatTrak"))
                            {
                                descriptionText += descriptions[4].GetProperty("value").GetString() + "\n";
                                descriptionText += descriptions[7].GetProperty("value").GetString() + "\n";
                            }
                            else if(marketName.Contains("Case"))
                            {
                                for (int i = 3; i <= 21; i++)
                                {
                                    // Check if index is within bounds
                                    if (i < descriptions.Count)
                                    {
                                        string propertyValue = descriptions[i].GetProperty("value").GetString();
                                        if (propertyValue != null)
                                        {
                                            descriptionText += propertyValue+"\n";
                                        } 
                                    }
                                    else
                                    {
                                        // Handle the case where the index is out of range
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                descriptionText = descriptions[2].GetProperty("value").GetString();
                            }
                        }
                    }

                    // Add skin to the list with required fields
                    skinList.Add(new Skin
                    {
                        MarketName = marketName,
                        Image = image,
                        ItemName = itemName,
                        PriceLatestSell = priceLatestSell,
                        Color = color,
                        DescriptionFloat = descriptionFloat,
                        DescriptionText = descriptionText
                    }); 
                }

                return skinList;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Error making HTTP request: " + ex.Message);
            }
            catch (NotSupportedException ex)
            {
                Console.WriteLine("The content type is not supported: " + ex.Message);
            }
            catch (JsonException ex)
            {
                Console.WriteLine("Error parsing JSON response: " + ex.Message);
            }

            return null;
        }
    }
}
