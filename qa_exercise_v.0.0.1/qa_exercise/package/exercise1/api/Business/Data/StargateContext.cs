using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Utilities;
using System.Data;

namespace StargateAPI.Business.Data
{
    public class StargateContext : DbContext
    {
        public IDbConnection Connection => Database.GetDbConnection();

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<AstronautDuty> AstronautDuties { get; set; }

        public StargateContext(DbContextOptions<StargateContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StargateContext).Assembly);

            SeedData(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private static void SeedData(ModelBuilder modelBuilder)
        {
            //add seed data
            modelBuilder.Entity<Person>()
                .HasData(
                    new Person
                    {
                        Id = 1,
                        Name = "John Doe"
                    }
                );

            modelBuilder.Entity<Account>()
                .HasData(
                    new Account
                    {
                        Id = 1,
                        PersonId = 1,
                        Username = "john.doe",
                        Password = HashHelper.ComputeHash("abc123")
                    }
                );

            modelBuilder.Entity<AstronautDuty>()
                .HasData(
                    new AstronautDuty
                    {
                        Id = 1,
                        PersonId = 1,
                        DutyStartDate = DateTime.Now,
                        DutyEndDate = DateTime.Now.AddYears(2),
                        DutyTitle = "Commander",
                        Rank = "Capt"
                    },
                    new AstronautDuty
                    {
                        Id = 2,
                        PersonId = 1,
                        DutyStartDate = DateTime.Now.AddYears(-2),
                        DutyEndDate = DateTime.Now.AddDays(-1),
                        DutyTitle = "Engineer",
                        Rank = "1LT"
                    },
                    new AstronautDuty
                    {
                        Id = 3,
                        PersonId = 1,
                        DutyStartDate = DateTime.Now.AddYears(-4),
                        DutyEndDate = DateTime.Now.AddYears(-2).AddDays(-1),
                        DutyTitle = "Trainee",
                        Rank = "2LT"
                    }
                );
        }
    }
}
