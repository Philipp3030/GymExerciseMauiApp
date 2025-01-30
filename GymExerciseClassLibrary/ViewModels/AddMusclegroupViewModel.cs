using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.Mappings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymExerciseClassLibrary.ViewModels
{
    public partial class AddMusclegroupViewModel : ObservableValidator
    {
        private readonly ApplicationDbContext _context;

        // musclegroup property
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "This field is empty.")]
        private string _name;

        // errormessage
        [ObservableProperty]
        private string? _errorMessageName;

        public AddMusclegroupViewModel() { }
        public AddMusclegroupViewModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [RelayCommand]
        private async Task SaveNewMusclegroup()
        {
            try
            {
                // Check if "Name" is empty
                if (!string.IsNullOrWhiteSpace(Name))
                {
                    // Save new musclegroup to database
                    _context.Musclegroups.Add(Mapper.MapMusclegroupViewModelToModel(_context, new MusclegroupViewModel
                    {
                        Name = this.Name
                    }));
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Message: {e.Message}");
                throw;
            }
        }
    }
}
