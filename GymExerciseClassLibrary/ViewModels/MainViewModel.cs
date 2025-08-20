using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.Mappings;
using GymExerciseClassLibrary.Models;
using GymExerciseClassLibrary.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace GymExerciseClassLibrary.ViewModels
{
    public partial class MainViewModel : ObservableValidator
    {
        private readonly ApplicationDbContext _context;
        private readonly TrainingService _trainingService;
        private readonly Mapper _mapper;
        [ObservableProperty]
        ObservableCollection<TrainingViewModel> _savedTrainings = new();

        public MainViewModel(ApplicationDbContext context)
        {
            _context = context;
            _trainingService = new TrainingService(context);
            _mapper = new Mapper(context);
            //LoadAllTrainingsFromDb();   // stattdessen var main = new MainViewModel();
            // await main. LoadAllTrainingsFromDb(); onAppearing()
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
                .Include(t => t.Exercises)
                    .ThenInclude(e => e.ExerciseIndices)
                .ToListAsync();
            foreach (var training in trainingsFromDb)
            {
                SavedTrainings.Add(_mapper.MapToViewModel(training, null));
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
            await _trainingService.DeleteTraining(training, SavedTrainings);
        }
    }
}
