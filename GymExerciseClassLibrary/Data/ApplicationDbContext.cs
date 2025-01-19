using GymExerciseClassLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymExerciseClassLibrary.Data
{
    public class ApplicationDbContext : DbContext
    {
        // dotnet ef migrations add Initial --startup-project .\GymExerciseClassLibrary --project .\GymExerciseClassLibrary

        // Constructor with no argument is required and it is used when adding/removing migrations from class library
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
            Database.Migrate();

        }

        // It is required to override this method when adding/removing migrations from class library
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite();

        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Training> Trainings { get; set; }
        public DbSet<Musclegroup> Musclegroups { get; set; }

        public async void SeedData()
        {
            if (!Musclegroups.Any())
            {
                Musclegroups.AddRange(
                [
                new Musclegroup { Name = "Chest" },
                new Musclegroup { Name = "Back" },
                new Musclegroup { Name = "Legs" }
                ]);
            }
            await SaveChangesAsync();
        }
    }
}
