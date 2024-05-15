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

        public SkinService()
        {
            this.httpClient = new HttpClient();
        }

        public async Task<List<Skin>?> GetSkins()
        {
            // Define the API endpoint URL
            // nog aanpassen dat ook de id kan ingegeven worden
            string apiUrl = "https://www.steamwebapi.com/steam/api/inventory?key=J1BAN31YCBSEJLLG&steam_id=76561198350557801";

            // Use JsonSerializerOptions to handle case insensitivity if needed
            var sourceGenOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

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

                    string descriptionFloat = string.Empty;
                    string descriptionText = string.Empty;


                    // Display the extracted fields
                    Console.WriteLine("Market Name: " + marketName);
                    Console.WriteLine("Item Name: " + itemName);
                    Console.WriteLine("Latest Price: $" + priceLatestSell);
                    Console.WriteLine("Image URL: " + image);
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
                                descriptionText = descriptions[7].GetProperty("value").GetString();
                            }
                            else if(marketName.Contains("Case"))
                            {
                                descriptionText = "is case";
                                //hier moet nog nagekeken worden dat alle values vanaf de 3de tem de 2de laatste oftewel "or an Exceedingly Rare Special Item!" in descriptionText komen
                                //for (int i = 3; descriptions[i].GetProperty("value").GetString().Contains("Exceedingly"); i++)

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
