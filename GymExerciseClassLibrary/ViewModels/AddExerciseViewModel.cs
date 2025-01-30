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
using System.Xml.Linq;

namespace GymExerciseClassLibrary.ViewModels
{
    public partial class AddExerciseViewModel : ObservableValidator
    {
        private readonly ApplicationDbContext _context;
        [ObservableProperty]
        private ObservableCollection<MusclegroupViewModel> _musclegroups = new();

        // exercise properties
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "This field is required.")]
        private string _name;
        [ObservableProperty]
        private bool _isActive;
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "This field is required.")]
        private MusclegroupViewModel _selectedMusclegroup = new();
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

        // errormessages
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

        public AddExerciseViewModel() { }
        public AddExerciseViewModel(ApplicationDbContext context)
        {
            _context = context;
            LoadMusclegroupsFromDb();
        }

        private async void LoadMusclegroupsFromDb()
        {
            Musclegroups.Clear();

            var musclegroups = await _context.Musclegroups.ToListAsync();
            foreach (var musclegroup in musclegroups)
            {
                Musclegroups.Add(Mapper.MapMusclegroupToViewModel(musclegroup));
            }
        }

        public void CallLoadMusclegroupsFromDb()
        {
            LoadMusclegroupsFromDb();
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
                    _context.Exercises.Add(Mapper.MapExerciseViewModelToModel(_context, new ExerciseViewModel()
                    {
                        Name = this.Name,
                        IsActive = this.IsActive,
                        Musclegroup = this.SelectedMusclegroup,
                        MachineName = this.MachineName,
                        Description = this.Description,
                        Sets = this.Sets,
                        RepsPrevious = this.RepsPrevious,
                        Reps = this.Reps,
                        RepsGoal = this.RepsGoal
                    }));   // irgendwas mit ids
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Message: {e.Message}\nInner Exception: {e.InnerException?.Message}");
                    throw;
                }
                await Shell.Current.GoToAsync("//MainPage");
            }
            else
            {
                ErrorMessageName = GetErrors(nameof(Name))?.FirstOrDefault()?.ToString();
                ErrorMessageSelectedMusclegroup = GetErrors(nameof(SelectedMusclegroup))?.FirstOrDefault()?.ToString();
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
