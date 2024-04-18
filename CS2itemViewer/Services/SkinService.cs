using CS2itemViewer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
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

        public async Task<List<Skin>?> GetSkins() //code async uitvoeren zodat mobile app niet hapert, hietdoor kan andere code afgespeeld worden
        {
            var sourceGenOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                TypeInfoResolver = SkinContext.Default
            };

            if (skinList?.Count > 0) { return skinList; }

            //var response = await httpClient.GetAsync("https://www.montemagno.com/monkeys.json");
            var response = await httpClient.GetAsync("https://monkeyfinderapi220240315144750.azurewebsites.net/api/monkeys"); // <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< LOOK HERE

            if (response.IsSuccessStatusCode)
            {
                skinList = await response.Content.ReadFromJsonAsync<List<Skin>>(sourceGenOptions);
            }
            return skinList;
        }
    }
}
