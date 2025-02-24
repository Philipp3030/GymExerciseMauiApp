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
        [ObservableProperty]
        private MusclegroupViewModel _musclegroup = new();

        public AddMusclegroupViewModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [RelayCommand]
        private async Task SaveNewMusclegroup()
        {
            bool hasErrors = Musclegroup.Validate();

            if (!hasErrors)
            {
                try
                {
                    // Save new musclegroup to database
                    _context.Musclegroups.Add(await Mapper.MapMusclegroupViewModelToModel(_context, Musclegroup));
                    await _context.SaveChangesAsync();

                    if (Shell.Current.CurrentState.Location.OriginalString.Contains("//MainPage/D_FAULT_AddMusclegroupPage"))
                    {
                        await Shell.Current.GoToAsync("//MainPage");
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Message: {e.Message}");
                    throw;
                } 
            }
            else
            {
                Musclegroup.CheckForErrorsCommand.Execute(null);
            }

        }
    }
}
