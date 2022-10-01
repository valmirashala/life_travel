using LifeTravel.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;   

namespace LifeTravel.Data
{
    public class DataBase : DbContext
    {
        public DataBase(DbContextOptions<DataBase> options) : base(options)
        {

        }
        public DbSet<Lines> Lines { get; set; }
    }
}
