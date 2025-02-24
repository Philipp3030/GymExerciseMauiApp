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
        private MusclegroupViewModel _musclegroup;
        [ObservableProperty]
        private string? _machineName;
        [ObservableProperty]
        private string? _description;
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [RegularExpression(@"^\d+$", ErrorMessage = "Only numbers are allowed.")]
        private string? _sets;
        [ObservableProperty]
        private ObservableCollection<RepetitionViewModel> _reps = new();
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [RegularExpression(@"^\d+$", ErrorMessage = "Only numbers are allowed.")]
        private string? _repsGoal;

        // additional properties
        [ObservableProperty]
        private bool _isSelected = false;
        [ObservableProperty]
        private bool _isExpanded = false;
        [ObservableProperty]
        private bool _isAdvancedOptionsClicked = false;

        // error messages
        [ObservableProperty]
        private string? _errorMessageName;
        [ObservableProperty]
        private string? _errorMessageMusclegroup;
        [ObservableProperty]
        private string? _errorMessageSets;
        [ObservableProperty]
        private string? _errorMessageRepsGoal;

        public bool Validate()
        {
            ValidateAllProperties();
            return HasErrors;
        }

        [RelayCommand]
        private void CheckForErrors()
        {
            ValidateAllProperties();

            ErrorMessageSets = GetErrors(nameof(Sets))?.FirstOrDefault()?.ToString();
            ErrorMessageRepsGoal = GetErrors(nameof(RepsGoal))?.FirstOrDefault()?.ToString();
        }

        [RelayCommand]
        private void CheckForErrorsOnSave()
        {
            ValidateAllProperties();

            ErrorMessageName = GetErrors(nameof(Name))?.FirstOrDefault()?.ToString();
            ErrorMessageMusclegroup = GetErrors(nameof(Musclegroup))?.FirstOrDefault()?.ToString();
        }
    }
}
