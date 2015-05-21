using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EasyWatch.Model
{
    public class DataService : IDataService
    {
        public async Task LadeSerien(Action<ObservableCollection<Serie>, Exception> callback)
        {
            HttpClient client = new HttpClient();
            var tmp = JsonConvert.DeserializeObject<ObservableCollection<Serie>>(await client.GetStringAsync("http://bs.to/api/series"));
            callback(tmp, null);
        }
        public async Task LadeSerienInformationen(Action<SerienInformationen, Exception> callback, Serie selectedSerie)
        {
            HttpClient client = new HttpClient();
            selectedSerie.SerienInformation = JsonConvert.DeserializeObject<SerienInformationen>(await client.GetStringAsync("http://bs.to/api/series/" + selectedSerie.id + "/1"));
            callback(selectedSerie.SerienInformation, null);
        }
        public async Task LadeSerienInformationen(Action<SerienInformationen, Exception> callback, Serie selectedSerie, string selectedStaffel)
        {
            HttpClient client = new HttpClient();
            selectedSerie.SerienInformation = JsonConvert.DeserializeObject<SerienInformationen>(await client.GetStringAsync("http://bs.to/api/series/" + selectedSerie.id + "/" + selectedStaffel));
            callback(selectedSerie.SerienInformation, null);
        }

        public async Task LadeEpisodenInformationen(Action<EpisodenInformationen, Exception> callback, Serie selectedSerie, string selectedStaffel, Epi selectedEpisode)
        {
            HttpClient client = new HttpClient();
            selectedSerie.episodenInformationen = JsonConvert.DeserializeObject<EpisodenInformationen>(await client.GetStringAsync("http://bs.to/api/series/" + selectedSerie.id + "/" + selectedStaffel + "/" + selectedEpisode.epi));
            callback(selectedSerie.episodenInformationen, null);
        }
    }
}