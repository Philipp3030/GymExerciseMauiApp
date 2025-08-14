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
    public partial class MusclegroupViewModel : ObservableValidator
    {
        // model properties
        [ObservableProperty]
        private int _id;
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "This field is empty.")]
        private string? _name;

        // error messages
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
