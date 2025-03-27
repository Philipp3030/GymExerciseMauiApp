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
        private string? _count;
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "Only numbers are allowed.")]
        private string? _weight;

        // error messages
        [ObservableProperty]
        private string? _errorMessageSet;
        [ObservableProperty]
        private string? _errorMessageCount;
        [ObservableProperty]
        private string? _errorMessageWeight;

        [RelayCommand]
        private void CheckForErrors()
        {
            ValidateAllProperties();

            ErrorMessageCount = GetErrors(nameof(Count))?.FirstOrDefault()?.ToString();
            ErrorMessageWeight = GetErrors(nameof(Weight))?.FirstOrDefault()?.ToString();
        }
    }
}
