namespace LifeTravel.Model
{
    public class Passanger
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserName { get; set; }
        public bool IsAsmin { get; set; }

        public static explicit operator Passanger(List<Passanger> v)
        {
            throw new NotImplementedException();
        }
    }
}
