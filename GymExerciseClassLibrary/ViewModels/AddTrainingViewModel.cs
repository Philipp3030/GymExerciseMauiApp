using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.Mappings;
using GymExerciseClassLibrary.Models;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GymExerciseClassLibrary.ViewModels
{
    public partial class AddTrainingViewModel : ObservableValidator
    {
        private readonly ApplicationDbContext _context;
        [ObservableProperty]
        private ObservableCollection<ExerciseViewModel> _exercisesToChooseFrom = new();
        [ObservableProperty]
        private bool _isSelected;

        // training properties
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "This field is required.")]
        private string _name;
        [ObservableProperty]
        private string? _description;
        [ObservableProperty]
        private ObservableCollection<ExerciseViewModel> _selectedExercises = new();

        // error message
        [ObservableProperty]
        private string? _errorMessageForName;

        public AddTrainingViewModel(ApplicationDbContext context)
        {
            _context = context;
            LoadAllExercisesFromDb();
        }

        private async void LoadAllExercisesFromDb()
        {
            ExercisesToChooseFrom.Clear();

            var exercisesFromDb = await _context.Exercises
                .Include(e => e.Musclegroup)
                .ToListAsync();
            foreach (var exercise in exercisesFromDb)
            {
                ExercisesToChooseFrom.Add(Mapper.MapExerciseToViewModel(exercise));
            }
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

        [RelayCommand]
        private async Task SaveNewTraining()
        {
            ValidateAllProperties();

            // Check if "Name" is empty
            if (!HasErrors)
            {
                // Create list of exercises for new Training entity to save to database
                List<Exercise> exercisesOfTraining = new List<Exercise>();
                foreach (var exerciseVM in SelectedExercises)
                {
                    exercisesOfTraining.Add(Mapper.MapExerciseViewModelToModel(_context, exerciseVM));
                }

                // Create "Training" entity and save to database
                Training newTraining = Mapper.MapTrainingViewModelToModel(_context, new TrainingViewModel
                {
                    Name = this.Name,
                    Description = this.Description
                });
                newTraining.Exercises = exercisesOfTraining;

                try
                {
                    _context.Trainings.Add(newTraining);
                    await _context.SaveChangesAsync();

                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Message: {e.Message}");
                    throw;
                }
                await Shell.Current.GoToAsync("//MainPage");
            }
            else
            {
                ErrorMessageForName = GetErrors(nameof(Name))?.FirstOrDefault()?.ToString();
            }
        }
    }
}
