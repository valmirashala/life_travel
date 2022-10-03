using Microsoft.AspNetCore.Mvc;
using LifeTravel.Data;
using LifeTravel.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace LifeTravel.Controllers
{
    public class LinesController : ControllerBase
    {
        private readonly DataBase ReadFromDataBase;

        public LinesController(DataBase dbContext)
        {
            ReadFromDataBase = dbContext;
        }

        [HttpGet("lines")]
        public List<Lines> Get()
        {
            var AllLines = ReadFromDataBase.Lines.ToList();
            return AllLines;
        }

        [HttpGet("line")]
        public IEnumerable<Lines> Get(string StartCiti, string EndCiti)
        {
            var filteredData = ReadFromDataBase.Lines.
                Where(x => (x.StartCiti == StartCiti || x.StopOne == StartCiti || x.StopTwo == StartCiti)
                && (x.EndCiti == EndCiti || x.StopOne == EndCiti || x.StopTwo == EndCiti)).ToList();
            if (filteredData.Count == 0)
            {
                string cs = "Server=localhost;Database=LIFETravel;Trusted_Connection=True;Encrypt=false";
                SqlConnection con = new SqlConnection(cs);
                string querry = "select * from lines where StartCiti in" +
                    "(select EndCiti from Lines where StartCiti = '" + StartCiti + "')";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = querry;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();

                var firstFilter = ReadFromDataBase.Lines.Where(x =>
                (x.StartCiti == StartCiti || x.StopOne == StartCiti || x.StopTwo == StartCiti)
                && (x.EndCiti == reader["startciti"])).ToList();
                var secondFilter = ReadFromDataBase.Lines.Where(x =>
                (x.StartCiti == reader["startciti"]) &&
                (x.StopOne == EndCiti || x.StopTwo == EndCiti || x.EndCiti == EndCiti)).ToList();

                con.Close();

                return firstFilter.Union(secondFilter);
            }
            return filteredData;
        }
    }
}