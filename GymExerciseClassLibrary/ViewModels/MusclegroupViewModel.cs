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
        private readonly ApplicationDbContext _context;
        [ObservableProperty]
        private ObservableCollection<MusclegroupViewModel> _options = new();

        // model properties
        [ObservableProperty]
        private int _id;
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "This field is empty.")]
        private string _name;


        public MusclegroupViewModel() { }
        public MusclegroupViewModel(ApplicationDbContext context)
        {
            _context = context;
            LoadOptions();
        }

        private async void LoadOptions()
        {
            Options.Clear();

            var musclegroups = await _context.Musclegroups.ToListAsync();
            foreach (var musclegroup in musclegroups)
            {
                Options.Add(Mapper.MapMusclegroupToViewModel(musclegroup)); 
            }
        }

        [RelayCommand]
        private async Task AddMusclegroup()
        {
            try
            {
                // Check if "Name" is empty
                if (!string.IsNullOrWhiteSpace(Name))
                {
                    // Save new musclegroup to database
                    _context.Musclegroups.Add(Mapper.MapMusclegroupViewModelToModel(_context, this));//musclegroup);
                    await _context.SaveChangesAsync();

                    LoadOptions();
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
