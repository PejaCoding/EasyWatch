namespace EasyWatch.Model
{
    public class Serie
    {
        public SerienInformationen SerienInformation { get; set; }
        public EpisodenInformationen episodenInformationen { get; set; }
        public string series { get; set; }
        public string id { get; set; }

        public override string ToString()
        {
           return series;
            
        }
    }
}
