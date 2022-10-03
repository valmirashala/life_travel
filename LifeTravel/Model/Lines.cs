namespace LifeTravel.Model
{
    public class Lines
    {
        public int Id { get; set; }
        public string StartCiti { get; set; }
        public string StartTime { get; set; }
        public string StopOne { get; set; }
        public string StopOneTime { get; set; }
        public string StopTwo { get; set; }
        public string StopTwoTime { get; set; }
        public string EndCiti { get; set; }
        public string EndTime { get; set; }

        public static explicit operator Lines(List<Lines> v)
        {
            throw new NotImplementedException();
        }
    }


}
