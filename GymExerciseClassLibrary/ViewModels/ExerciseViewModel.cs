using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

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
        private string? _amountOfSets;
        [ObservableProperty]
        private ObservableCollection<SetViewModel> _sets = new();
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [RegularExpression(@"^\d+$", ErrorMessage = "Only numbers are allowed.")]
        private string? _repsGoal;
        [ObservableProperty]
        private ObservableCollection<int> _trainingIds = new();

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
        private string? _errorMessageAmountOfSets;
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

            ErrorMessageAmountOfSets = GetErrors(nameof(AmountOfSets))?.FirstOrDefault()?.ToString();
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
