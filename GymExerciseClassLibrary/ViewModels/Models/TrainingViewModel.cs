using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.Mappings;
using GymExerciseClassLibrary.Models;
using GymExerciseClassLibrary.ViewModels.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace GymExerciseClassLibrary.ViewModels
{
    public partial class TrainingViewModel : ObservableValidator
    {
        // model properties
        [ObservableProperty]
        private int _id;
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "This field is required.")]
        private string? _name;
        [ObservableProperty]
        private string? _description;
        [ObservableProperty]
        private ObservableCollection<ExerciseViewModel> _exercisesOfTraining = new();
        [ObservableProperty]
        private ObservableCollection<int> _exerciseIds = new();
        [ObservableProperty]
        private ObservableCollection<ExerciseIndexViewModel> _exerciseIndices = new();

        // additional properties
        [ObservableProperty]
        private string? _errorMessageName;

        public bool Validate()
        {
            ValidateAllProperties();
            return HasErrors;
        }

        [RelayCommand]
        private void CheckForErrors()
        {
            ValidateAllProperties();

            ErrorMessageName = GetErrors(nameof(Name))?.FirstOrDefault()?.ToString();
        }
    }
}
