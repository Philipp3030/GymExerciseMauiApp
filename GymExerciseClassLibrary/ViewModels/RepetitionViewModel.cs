using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymExerciseClassLibrary.ViewModels
{
    public partial class RepetitionViewModel : ObservableValidator
    {
        // model properties
        [ObservableProperty]
        private int _id;
        [ObservableProperty]
        private int _set;
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [RegularExpression(@"^\d+$", ErrorMessage = "Only numbers are allowed.")]
        private string? _reps;
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "Weight must be in format \"15.25\" or \"15\"")]
        private string? _weight;

        // error messages
        [ObservableProperty]
        private string? _errorMessageSet;
        [ObservableProperty]
        private string? _errorMessageReps;
        [ObservableProperty]
        private string? _errorMessageWeight;

        public bool Validate()
        {
            ValidateAllProperties();
            return HasErrors;
        }

        [RelayCommand]
        private void CheckForErrors()
        {
            ValidateAllProperties();

            ErrorMessageReps = GetErrors(nameof(Reps))?.FirstOrDefault()?.ToString();
            ErrorMessageWeight = GetErrors(nameof(Weight))?.FirstOrDefault()?.ToString();
        }
    }
}
