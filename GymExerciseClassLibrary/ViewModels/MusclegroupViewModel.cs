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
        }

        //[RelayCommand]
        //private async Task AddMusclegroup()
        //{
        //    try
        //    {
        //        // Check if "Name" is empty
        //        if (!string.IsNullOrWhiteSpace(Name))
        //        {
        //            // Save new musclegroup to database
        //            _context.Musclegroups.Add(Mapper.MapMusclegroupViewModelToModel(_context, this));
        //            await _context.SaveChangesAsync();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine($"Message: {e.Message}");
        //        throw;
        //    }
        //}
    }
}
