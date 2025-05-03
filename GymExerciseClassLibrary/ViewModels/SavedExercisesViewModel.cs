using CommunityToolkit.Mvvm.ComponentModel;
using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.Mappings;
using GymExerciseClassLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymExerciseClassLibrary.ViewModels
{
    public partial class SavedExercisesViewModel : ObservableValidator
    {
        private readonly ApplicationDbContext _context;
        [ObservableProperty]
        ObservableCollection<ExerciseViewModel> _exercises = new();

        public SavedExercisesViewModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // executes onAppearing; constructor does NOT
        // maybe implement on all ViewModels?
        public async Task InitializeAsync()
        {
            await LoadExercisesFromDb();
        }

        private async Task LoadExercisesFromDb()
        {
            Exercises.Clear();

            var exercisesFromDb = await _context.Exercises
                                    .Include(e => e.Musclegroup)
                                    .Include(e => e.Reps)
                                    .ToListAsync();

            foreach (var exercise in exercisesFromDb)
            {
                Exercises.Add(Mapper.Map(exercise));
            }
        }
    }
}
