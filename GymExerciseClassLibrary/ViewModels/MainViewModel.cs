using CommunityToolkit.Mvvm.ComponentModel;
using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.Mappings;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

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
                .Include(t => t.Exercises)
                .ThenInclude(e => e.Musclegroup)
                .ToListAsync();
            foreach (var training in trainingsFromDb)
            {
                SavedTrainings.Add(Mapper.MapTrainingToViewModel(training));
            }
        }

        public void ReloadData()
        {
            LoadAllTrainingsFromDb();
        }
    }
}
