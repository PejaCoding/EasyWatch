using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using EasyWatch.Model;
using GalaSoft.MvvmLight;

namespace EasyWatch.ViewModel
{
    public class MainViewModel : ViewModelBase
    {


        private readonly IDataService _dataService;
        private ObservableCollection<Serie> _serienListe;
        private Serie _selectedSerie;
        private List<string> _staffelListe = new List<string>();
        private string _selectedStaffel;
        private List<Epi> _episodenListe = new List<Epi>();
        private Epi _selectedEpisode;
        private EpisodenInformationen _selectEpisodenInformationen { get; set; }
        private List<string> _directLinks = new List<string>();




        /// <summary>
        /// Sets and gets the SerienListe property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<Serie> SerienListe
        {
            get
            {
                return _serienListe;
            }
            set
            {
                if (_serienListe == value)
                {
                    return;
                }
                _serienListe = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Sets and gets the SelectedSerie property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Serie SelectedSerie
        {
            get
            {
                return _selectedSerie;
            }
            set
            {
                if (_selectedSerie == value)
                {
                    return;
                }
                _selectedSerie = value;

                _dataService.LadeSerienInformationen(
                    (item, error) =>
                    {
                        EpisodenListe = item.epi;
                        List<string> tmp = new List<string>();
                        if (int.Parse(SelectedSerie.SerienInformation.series.movies) > 0)
                        {
                            tmp.Add("Filme");
                        }
                        for (int i = 0; i < int.Parse(SelectedSerie.SerienInformation.series.seasons); i++)
                        {
                            tmp.Add("Staffel " + (i + 1));
                        }
                        StaffelListe = tmp;

                    }, SelectedSerie);
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Sets and gets the StaffelListe property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public List<string> StaffelListe
        {
            get { return _staffelListe; }
            set
            {
                _staffelListe = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Sets and gets the SelectedStaffel property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string SelectedStaffel
        {
            get { return _selectedStaffel; }
            set
            {
                // movies = staffel 0
                var staffel = value == "Filme" ? "0" : value.Replace("Staffel ", "");
                _selectedStaffel = staffel;
                _dataService.LadeSerienInformationen(
                   (item, error) =>
                   {
                       EpisodenListe = item.epi;
                   }, SelectedSerie, staffel);
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Sets and gets the EpisodenListe property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public List<Epi> EpisodenListe
        {
            get { return _episodenListe; }
            set
            {
                if (_episodenListe == value)
                {
                    return;
                }
                _episodenListe = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Sets and gets the SelectedEpisode property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Epi SelectedEpisode
        {
            get { return _selectedEpisode; }
            set
            {

                _selectedEpisode = value;
                _dataService.LadeEpisodenInformationen(
                (item, error) =>
                {
                    SelectEpisodenInformationen = item;
                }, SelectedSerie, SelectedStaffel, value);
            }
        }

        /// <summary>
        /// Sets and gets the SelectEpisodenInformationen property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public EpisodenInformationen SelectEpisodenInformationen
        {
            get { return _selectEpisodenInformationen; }
            set
            {
                _selectEpisodenInformationen = value;
                RaisePropertyChanged();
                List<string> tmp = new List<string>();
                foreach (var HosterLink in value.VLinks)
                {
                    if (HosterLink.hoster == "Streamcloud")
                    {
                        _dataService.LadeHosterInformationen(async (item, exception) =>
                        {
                          string directlink = await  Hoster.StreamCloud(item.fullurl);
                            tmp.Add(directlink);
                        },HosterLink.id);
                    }
                    else if (HosterLink.hoster == "Vivo")
                    {
                        _dataService.LadeHosterInformationen(async (item, exception) =>
                        {
                            string directlink = await Hoster.Vivo(item.fullurl);
                            tmp.Add(directlink);
                        }, HosterLink.id);
                    }
                    
                }
                Directlinks = tmp;
            }
        }


        public List<string> Directlinks
        {
            get { return _directLinks; }
            set
            {
                _directLinks = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataService dataService)
        {
            _dataService = dataService;
            _dataService.LadeSerien(
                (item, error) =>
                {
                    SerienListe = item;
                });
        }
    }
}