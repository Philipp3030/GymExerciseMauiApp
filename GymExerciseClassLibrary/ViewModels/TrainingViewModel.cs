using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymExerciseClassLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace GymExerciseClassLibrary.ViewModels
{
    public partial class TrainingViewModel : ObservableObject
    {
        private readonly ApplicationDbContext _context;
        [ObservableProperty]
        private ObservableCollection<ExerciseViewModel> _selectedExercises = new();
        [ObservableProperty]
        private ObservableCollection<ExerciseViewModel> _allExercises = new();
        [ObservableProperty]
        private string _name;
        [ObservableProperty]
        private string _description;

        public int TrainingId { get; set; }
        public List<Training> Trainings { get; set; } = new List<Training>();
        public List<Exercise> ExercisesOfTraining { get; set; } = new List<Exercise>();
        //public string ExerciseName { get; set; } = string.Empty;
        public Training NewTraining { get; set; }

        public TrainingViewModel(ApplicationDbContext context)
        {
            _context = context;
            NewTraining = new Training();
            Name = string.Empty;
            LoadTrainings();
            LoadAllExercises();
        }
        private async void LoadTrainings()
        {
            var trainings = await _context.Trainings.ToListAsync();
            foreach (var training in trainings)
            {
                Trainings.Add(training);
            }
        }

        private async void LoadAllExercises()
        {
            var exercisesFromDb = await _context.Exercises.ToListAsync();

            // Option2: MAPPEN (Automapper) zu ExerciseViewModel, um IsSelected zu verwenden

            AllExercises.Clear();
            foreach (var exercise in exercisesFromDb)
            {
                AllExercises.Add(new ExerciseViewModel(_context)
                {
                    Id = exercise.Id,
                    Name = exercise.Name,
                    Musclegroup = exercise.Musclegroup,
                    IsSelected = false // Default value
                });
            }
        }

        [RelayCommand]
        private void OnExerciseCheckedChangedCommand(ExerciseViewModel exercise)
        {
            //var checkbox = (CheckBox)sender;
            //var exercise = (ExerciseViewModel)checkbox.BindingContext;

            // Call the ToggleSelection method to update SelectedExercises
            ToggleSelection(exercise);
        }


        public void ToggleSelection(ExerciseViewModel exercise)
        {
            if (exercise.IsSelected)
            {
                // Add the exercise to the selected list if it's selected
                if (!SelectedExercises.Contains(exercise))
                    SelectedExercises.Add(exercise);
            }
            else
            {
                // Remove the exercise from the selected list if it's deselected
                if (SelectedExercises.Contains(exercise))
                    SelectedExercises.Remove(exercise);
            }
        }

        //[RelayCommand]
        private async Task AddExistingExerciseToNewTraining()
        {
            foreach (var exerciseViewModel in SelectedExercises)
            {
                if (exerciseViewModel.IsSelected)
                {
                    ExercisesOfTraining.Add(await _context.Exercises.FirstOrDefaultAsync(e => e.Id == exerciseViewModel.Id));
                }
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
            Debug.WriteLine("Test");
            await AddExistingExerciseToNewTraining();
            NewTraining.Name = Name;
            NewTraining.Description = Description;
            NewTraining.Exercises = ExercisesOfTraining;//.ToList(); // Associate the exercises with the training
            _context.Trainings.Add(NewTraining);
            await _context.SaveChangesAsync();


            Name = string.Empty;
            Description = string.Empty;
            ExercisesOfTraining.Clear(); // Clear the exercises collection after saving
            SelectedExercises.Clear();
            NewTraining = new Training(); // Reset the NewTraining object after saving
            LoadAllExercises();
            LoadTrainings();
            await Shell.Current.GoToAsync("//MainPage");
        }
    }
}
