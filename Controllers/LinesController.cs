using Microsoft.AspNetCore.Mvc;
using LifeTravel.Data;
using LifeTravel.Model;
using Microsoft.EntityFrameworkCore;

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
        public IEnumerable<Lines> Get()
        {
            Console.WriteLine("ketuu");
            var AllLines = ReadFromDataBase.Lines.ToList();
            return AllLines;
        }

        [HttpGet("line")]
        public Lines Get(int id)
        {
            Console.WriteLine("tjetraa");
            Lines filteredData;
            filteredData = ReadFromDataBase.Lines.
                Where(x => x.Id == id).FirstOrDefault();
            return filteredData;
        }
    }
}
