using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace EasyWatch.Model
{
    public interface IDataService
    {
        Task LadeSerien(Action<ObservableCollection<Serie>, Exception> callback);
        Task LadeSerienInformationen(Action<SerienInformationen, Exception> callback, Serie selectedSerie);
        Task LadeSerienInformationen(Action<SerienInformationen, Exception> callback, Serie selectedSerie, string selectedStaffel);
        Task LadeEpisodenInformationen(Action<EpisodenInformationen, Exception> callback, Serie selectedSerie,string selectedStaffel, Epi selectedEpisode);
    }
}
