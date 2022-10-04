using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Travelephant.Model;

namespace Travelephant.Data
{
    public class TravelephantContext : DbContext
    {
        public TravelephantContext (DbContextOptions<TravelephantContext> options)
            : base(options)
        {
        }

        public DbSet<Travelephant.Model.User> User { get; set; } = default!;
        public DbSet<Travelephant.Model.BusInfo> BusInfo { get; set; } = default!;
        public DbSet<Travelephant.Model.Ticket> Ticket { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>()
                .HasKey(c => new { c.UserID, c.BusID });
            modelBuilder.Entity<BusInfo>()
                .HasKey(c => new { c.BusId});
            modelBuilder.Entity<User>()
                .HasKey(c => new { c.UserId });
        }
    }

}
