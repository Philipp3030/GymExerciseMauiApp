using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.Mappings;
using GymExerciseClassLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace GymExerciseClassLibrary.ViewModels
{
    public partial class TrainingViewModel : ObservableValidator
    {
        private readonly ApplicationDbContext _context;
        [ObservableProperty]
        private ObservableCollection<TrainingViewModel> _allTrainingVMs = new();
        [ObservableProperty]
        private ObservableCollection<ExerciseViewModel> _selectedExerciseVMs = new();
        [ObservableProperty]
        private ObservableCollection<ExerciseViewModel> _allExerciseVMs = new();
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "This field is required.")]
        private string _name;
        [ObservableProperty]
        private string? _description;
        [ObservableProperty]
        private string? _errorMessage;

        public TrainingViewModel() { }
        public TrainingViewModel(ApplicationDbContext context)
        {
            _context = context;
            LoadTrainings();
            LoadAllExercises();
        }
        private async void LoadTrainings()
        {
            AllTrainingVMs.Clear();

            var trainingsFromDb = await _context.Trainings.ToListAsync();
            foreach (var training in trainingsFromDb)
            {
                AllTrainingVMs.Add(Mapper.MapTrainingToViewModel(training));
            }
        }

        private async void LoadAllExercises()
        {
            AllExerciseVMs.Clear();
            
            var exercisesFromDb = await _context.Exercises.ToListAsync();
            foreach (var exercise in exercisesFromDb)
            {
                ExerciseViewModel exerciseVM = Mapper.MapExerciseToViewModel(exercise);
                exerciseVM.IsSelected = false; // Default value
                AllExerciseVMs.Add(exerciseVM); 
            }
        }

        // handle selection of exercises on "AddTrainingPage"
        public void ToggleSelection(ExerciseViewModel exercise)
        {
            if (exercise.IsSelected)
            {
                // Add the exercise to the selected list if it's selected
                if (!SelectedExerciseVMs.Contains(exercise))
                    SelectedExerciseVMs.Add(exercise);
            }
            else
            {
                // Remove the exercise from the selected list if it's deselected
                if (SelectedExerciseVMs.Contains(exercise))
                    SelectedExerciseVMs.Remove(exercise);
            }
        }

        [RelayCommand]
        private async Task SaveNewTraining()
        {
            ValidateAllProperties();            

            // Check if "Name" is empty
            if (!HasErrors)
            {
                // Create list of exercises for new Training entity to save to database
                List<Exercise> exercisesOfTraining = new List<Exercise>();
                foreach (var exerciseVM in SelectedExerciseVMs)
                {
                    exercisesOfTraining.Add(Mapper.MapExerciseViewModelToModel(exerciseVM));
                }

                // Create "Training" entity and save to database
                Training newTraining = Mapper.MapTrainingViewModelToModel(this);
                    //new TrainingViewModel
                    //{
                    //    Name = this.Name,
                    //    Description = this.Description
                    //});
                newTraining.Exercises = exercisesOfTraining;
                _context.Trainings.Add(newTraining);
                await _context.SaveChangesAsync();

                // Clear all
                SelectedExerciseVMs.Clear();
                Name = string.Empty;
                Description = string.Empty;
                //NewTrainingVM = new TrainingViewModel(); 

                // Update all
                LoadAllExercises();
                LoadTrainings();
                await Shell.Current.GoToAsync("//MainPage"); 
            }
            else
            {
                ErrorMessage = GetErrors(nameof(Name))?.FirstOrDefault()?.ToString();
            }
        }

        //[RelayCommand]
        //private async Task NavigateToThisTraining()
        //{

        //}
    }
}
