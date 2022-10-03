namespace LifeTravel.Model
{
    public class Passenger
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public DateTime Birthdate  { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public bool IsAdmin { get; set; }

        public static explicit operator Lines(List<Lines> v)
        {
            throw new NotImplementedException();
        }
    }


}