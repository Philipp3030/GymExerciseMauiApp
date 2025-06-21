using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.Enums;
using GymExerciseClassLibrary.Mappings;
using GymExerciseClassLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.ObjectModel;
using System.Diagnostics;

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
            Exercise = exercise;
        }

        public async Task InitializeAsync()
        {
            int musclegroupId = Exercise.Musclegroup.Id;
            await LoadMusclegroupsFromDbAsync();
            Exercise.Musclegroup = Musclegroups.FirstOrDefault(m => m.Id == musclegroupId); // set value for picker to find entity
        }

        private async Task LoadMusclegroupsFromDbAsync()
        {
            Musclegroups.Clear();

            var musclegroups = await _context.Musclegroups.ToListAsync();
            foreach (var musclegroup in musclegroups)
            {
                Musclegroups.Add(Mapper.Map(musclegroup));
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
                    _context.Exercises.Update(await Mapper.Map(_context, Exercise));
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
