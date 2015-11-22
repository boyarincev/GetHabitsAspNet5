using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GetHabitsAspNet5App.Models.DomainModels
{
    public class GetHabitsContext: DbContext
    {
        public GetHabitsContext(DbContextOptions<GetHabitsContext> options)
            :base(options)
        {

        }

        public DbSet<Habit> Habits { get; set; }
        public DbSet<Checkin> Checkins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ForSqlServerUseIdentityColumns();
            base.OnModelCreating(modelBuilder);
        }
    }
}
