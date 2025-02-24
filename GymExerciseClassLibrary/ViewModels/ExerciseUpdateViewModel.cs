using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.Mappings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymExerciseClassLibrary.ViewModels
{
    public partial class ExerciseUpdateViewModel : ObservableValidator
    {
        private ApplicationDbContext _context;
        [ObservableProperty]
        private ExerciseViewModel _exercise;
        [ObservableProperty]
        private ObservableCollection<MusclegroupViewModel> _musclegroups = new();

        public ExerciseUpdateViewModel(ApplicationDbContext context, ExerciseViewModel exercise)
        {
            _context = context;
            _exercise = exercise;
            LoadMusclegroupsFromDb();
        }

        private async void LoadMusclegroupsFromDb()
        {
            Musclegroups.Clear();

            var musclegroups = await _context.Musclegroups.ToListAsync();
            foreach (var musclegroup in musclegroups)
            {
                Musclegroups.Add(Mapper.MapMusclegroupToViewModel(musclegroup));
            }
        }

        [RelayCommand]
        private async Task UpdateExercise()
        {
            bool hasErrors = Exercise.Validate();

            if (!hasErrors)
            {
                try
                {
                    _context.Exercises.Update(await Mapper.MapExerciseViewModelToModel(_context, Exercise));
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Message: {e.Message}\nInner Exception: {e.InnerException?.Message}");
                    throw;
                }
            }
            else
            {
                Exercise.CheckForErrorsOnSaveCommand.Execute(null);
            }
        }
    }
}
