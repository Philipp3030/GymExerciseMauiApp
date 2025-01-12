using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymExerciseClassLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            //NewTrainingVM = new TrainingViewModel();
            LoadTrainings();
            LoadAllExercises();
        }
        private async void LoadTrainings() // zu Viewmodeln machen??? wie bei AllExercises
        {
            AllTrainingVMs.Clear();

            var trainingsFromDb = await _context.Trainings.ToListAsync();
            foreach (var training in trainingsFromDb)
            {
                TrainingViewModel trainingVM = MapTrainingToViewModel(training);
                AllTrainingVMs.Add(trainingVM);
            }
        }

        private async void LoadAllExercises()
        {
            AllExerciseVMs.Clear();
            
            var exercisesFromDb = await _context.Exercises.ToListAsync();
            foreach (var exercise in exercisesFromDb)
            {
                ExerciseViewModel exerciseVM = MapExerciseToViewModel(exercise);
                exerciseVM.IsSelected = false; // Default value
                AllExerciseVMs.Add(exerciseVM); 
            }
        }

        private ExerciseViewModel MapExerciseToViewModel(Exercise exercise)
        {
            ExerciseViewModel newExerciseVM = new ExerciseViewModel
            {
                Id = exercise.Id,
                Name = exercise.Name,
                IsActive = exercise.IsActive,
                Musclegroup = exercise.Musclegroup,
                MachineName = exercise.MachineName,
                Description = exercise.Description,
                Sets = exercise.Sets,
                RepsPrevious = exercise.RepsPrevious,
                Reps = exercise.Reps,
                RepsGoal = exercise.RepsGoal
            };
            return newExerciseVM;
        }

        private Exercise MapExerciseViewModelToModel(ExerciseViewModel exerciseVM)
        {
            Exercise newExercise = new Exercise
            {
                Id = exerciseVM.Id,
                Name = exerciseVM.Name,
                IsActive = exerciseVM.IsActive,
                Musclegroup = exerciseVM.Musclegroup,
                MachineName = exerciseVM.MachineName,
                Description = exerciseVM.Description,
                Sets = exerciseVM.Sets,
                RepsPrevious = exerciseVM.RepsPrevious,
                Reps = exerciseVM.Reps,
                RepsGoal = exerciseVM.RepsGoal
            };
            return newExercise;
        }

        private Training MapTrainingViewModelToModel(TrainingViewModel trainingVM)
        {
            Training newTraining = new Training
            {
                Name = trainingVM.Name,
                Description = trainingVM.Description
            };
            return newTraining;
        }

        private TrainingViewModel MapTrainingToViewModel(Training training)
        {
            TrainingViewModel newTrainingVM = new TrainingViewModel
            {
                Name = training.Name,
                Description = training.Description
            };
            return newTrainingVM;
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

        //[RelayCommand]
        //private void AddNewExerciseToNewTraining()
        //{
        //    if (!string.IsNullOrEmpty(ExerciseName)) // Make sure the exercise name is not empty
        //    {
        //        var newExercise = new Exercise
        //        {
        //            Name = ExerciseName,
        //            Musclegroup = "Default" // You can allow the user to select the muscle group as well
        //        };

        //        ExercisesOfTraining.Add(newExercise); // Add the new exercise to the ObservableCollection
        //        ExerciseName = string.Empty; // Reset the ExerciseName
        //    }
        //}

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
                    exercisesOfTraining.Add(MapExerciseViewModelToModel(exerciseVM));
                }

                // Create "Training" entity and save to database
                Training newTraining = MapTrainingViewModelToModel(
                    new TrainingViewModel
                    {
                        Name = this.Name,
                        Description = this.Description
                    });
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
