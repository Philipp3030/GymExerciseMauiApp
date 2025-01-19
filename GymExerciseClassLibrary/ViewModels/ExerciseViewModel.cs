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
        private ObservableCollection<ExerciseViewModel> _exerciseVMs = new();
        [ObservableProperty]
        private MusclegroupViewModel _musclegroupVM;
        [ObservableProperty]
        private bool _isSelected;
        [ObservableProperty]
        private string? _errorMessageName;
        [ObservableProperty]
        private string? _errorMessageSets;
        [ObservableProperty]
        private string? _errorMessageRepsPrevious;
        [ObservableProperty]
        private string? _errorMessageReps;
        [ObservableProperty]
        private string? _errorMessageRepsGoal;
        [ObservableProperty]
        private string? _errorMessageSelectedMusclegroup;

        // model properties
        [ObservableProperty]
        private int _id;
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "This field is required.")]
        private string _name;
        [ObservableProperty]
        private bool _isActive;
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "This field is required.")]
        private MusclegroupViewModel _selectedMusclegroupVM;
        [ObservableProperty]
        private string? _machineName;
        [ObservableProperty]
        private string? _description;
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [RegularExpression(@"^\d+$", ErrorMessage = "Only numbers are allowed.")]
        private string? _sets;
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [RegularExpression(@"^\d+$", ErrorMessage = "Only numbers are allowed.")]
        private string? _repsPrevious;
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [RegularExpression(@"^\d+$", ErrorMessage = "Only numbers are allowed.")]
        private string? _reps;
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [RegularExpression(@"^\d+$", ErrorMessage = "Only numbers are allowed.")]
        private string? _repsGoal;
        
        
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
                ExerciseVMs.Add(Mapper.MapExerciseToViewModel(exercise));
            }
        }

        [RelayCommand]
        private async Task SaveNewExercise()
        {
            ValidateAllProperties();

            // Check for errors
            if (!HasErrors)
            {
                try
                {
                    _context.Exercises.Add(Mapper.MapExerciseViewModelToModel(_context, this));   // irgendwas mit ids
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Message: {e.Message}\nInner Exception: {e.InnerException.Message}");
                    throw;
                }
                await Shell.Current.GoToAsync("//MainPage");
            }
            else
            {
                ErrorMessageName = GetErrors(nameof(Name))?.FirstOrDefault()?.ToString();
                ErrorMessageSelectedMusclegroup = GetErrors(nameof(SelectedMusclegroupVM))?.FirstOrDefault()?.ToString();
            }
        }

        [RelayCommand]
        private void CheckForErrors()
        {
            ValidateAllProperties();
            
            ErrorMessageSets = GetErrors(nameof(Sets))?.FirstOrDefault()?.ToString();
            ErrorMessageRepsPrevious = GetErrors(nameof(RepsPrevious))?.FirstOrDefault()?.ToString();
            ErrorMessageReps = GetErrors(nameof(Reps))?.FirstOrDefault()?.ToString();
            ErrorMessageRepsGoal = GetErrors(nameof(RepsGoal))?.FirstOrDefault()?.ToString();
        }
    }
}
