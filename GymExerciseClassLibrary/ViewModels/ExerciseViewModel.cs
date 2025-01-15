using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.Mappings;
using GymExerciseClassLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymExerciseClassLibrary.ViewModels
{
    public partial class ExerciseViewModel : ObservableValidator
    {
        private readonly ApplicationDbContext _context;

        [ObservableProperty]
        private ObservableCollection<Exercise> _exercises = new();
        //[ObservableProperty]
        //private ObservableCollection<MusclegroupViewModel> _musclegroupVMs = new();
        [ObservableProperty]
        private int _id;
        [ObservableProperty]
        private string _name;
        [ObservableProperty]
        private bool _isActive;
        [ObservableProperty]
        private MusclegroupViewModel _musclegroupVM;
        [ObservableProperty]
        private string? _machineName;
        [ObservableProperty]
        private string? _description;
        [ObservableProperty]
        private int? _sets;
        [ObservableProperty]
        private int? _repsPrevious;
        [ObservableProperty]
        private int? _reps;
        [ObservableProperty]
        private int? _repsGoal;
        [ObservableProperty]
        private bool _isSelected;

        public ExerciseViewModel() { }  
        public ExerciseViewModel(ApplicationDbContext context)
        {
            _context = context;
            MusclegroupVM = new MusclegroupViewModel(_context);
            LoadExercises();
        }

        private async void LoadExercises()
        { 
            var exercises = await _context.Exercises.ToListAsync();
            foreach (var exercise in exercises)
            {
                Exercises.Add(exercise);
            }
        }

        [RelayCommand]
        private async Task AddMusclegroup()
        {
            try
            {
                // Check if "Name" is empty
                if (!string.IsNullOrWhiteSpace(MusclegroupVM.Name))
                {
                    Musclegroup musclegroup = new Musclegroup
                    {
                        Name = MusclegroupVM.Name
                    };

                    // Save new musclegroup to database
                    _context.Musclegroups.Add(musclegroup);
                    await _context.SaveChangesAsync();

                    // Create new musclegroup to update view
                    MusclegroupVM = new MusclegroupViewModel(_context);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Message: {e.Message}");
                throw;
            }
        }

        //[RelayCommand]
        //private async Task SaveNewExercise()
        //{
        //    ValidateAllProperties();

        //    // Check if "Name" is empty
        //    if (!HasErrors)
        //    {
        //        // Create list of exercises for new Training entity to save to database
        //        List<Exercise> exercisesOfTraining = new List<Exercise>();
        //        foreach (var exerciseVM in SelectedExerciseVMs)
        //        {
        //            exercisesOfTraining.Add(Mapper.MapExerciseViewModelToModel(exerciseVM));
        //        }

        //        // Create "Training" entity and save to database
        //        Training newTraining = Mapper.MapTrainingViewModelToModel(
        //            new TrainingViewModel
        //            {
        //                Name = this.Name,
        //                Description = this.Description
        //            });
        //        newTraining.Exercises = exercisesOfTraining;
        //        _context.Trainings.Add(newTraining);
        //        await _context.SaveChangesAsync();

        //        // Clear all
        //        SelectedExerciseVMs.Clear();
        //        Name = string.Empty;
        //        Description = string.Empty;
        //        //NewTrainingVM = new TrainingViewModel(); 

        //        // Update all
        //        LoadAllExercises();
        //        LoadTrainings();
        //        await Shell.Current.GoToAsync("//MainPage");
        //    }
        //    else
        //    {
        //        ErrorMessage = GetErrors(nameof(Name))?.FirstOrDefault()?.ToString();
        //    }
        //}
    }
}
