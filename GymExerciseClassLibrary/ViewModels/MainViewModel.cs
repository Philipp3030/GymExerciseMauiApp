using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.Mappings;
using GymExerciseClassLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace GymExerciseClassLibrary.ViewModels
{
    public partial class MainViewModel : ObservableValidator
    {
        private readonly ApplicationDbContext _context;
        [ObservableProperty]
        ObservableCollection<TrainingViewModel> _savedTrainings = new();

        public MainViewModel(ApplicationDbContext context)
        {
            _context = context;
            LoadAllTrainingsFromDb();
        }

        private async void LoadAllTrainingsFromDb()
        {
            SavedTrainings.Clear();

            var trainingsFromDb = await _context.Trainings
                .AsNoTracking()
                .Include(t => t.Exercises)
                    .ThenInclude(e => e.Musclegroup)
                .Include(t => t.Exercises)
                    .ThenInclude(e => e.Sets)
                .ToListAsync();
            foreach (var training in trainingsFromDb)
            {
                SavedTrainings.Add(Mapper.Map(training));
                CheckAmountOfSets(training);
            }
        }

        public void ReloadData()
        {
            LoadAllTrainingsFromDb();
        }

        // for safety
        private void CheckAmountOfSets(Training training)
        {
            foreach (var exercise in training.Exercises)
            {
                if (exercise.AmountOfSets != exercise.Sets.Count)
                {
                    exercise.AmountOfSets = exercise.Sets.Count;
                }
            }
        }

        [RelayCommand]
        private async Task DeleteTraining(TrainingViewModel training)
        {
            try
            {
                // remove from db
                var trainingDb = await _context.Trainings.FirstOrDefaultAsync(t => t.Id == training.Id);
                if (trainingDb == null)
                {
                    return;
                }

                _context.Trainings.Remove(trainingDb);  
                await _context.SaveChangesAsync();

                // remove from view for animation
                SavedTrainings.Remove(training);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Message: {e.Message}");
                throw;
            }
        }
    }
}
