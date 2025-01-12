using CommunityToolkit.Mvvm.ComponentModel;
using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.Mappings;
using GymExerciseClassLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymExerciseClassLibrary.ViewModels
{
    public partial class MusclegroupViewModel : ObservableValidator
    {
        private readonly ApplicationDbContext _context;
        [ObservableProperty]
        private int _id;
        [ObservableProperty]
        private string _name;
        [ObservableProperty]
        private ObservableCollection<MusclegroupViewModel> _options = new();


        public MusclegroupViewModel() { }
        public MusclegroupViewModel(ApplicationDbContext context)
        {
            _context = context;
            LoadMusclegroups();
        }

        private async void LoadMusclegroups()
        {
            var musclegroups = await _context.Musclegroups.ToListAsync();
            foreach (var musclegroup in musclegroups)
            {
                Options.Add(Mapper.MapMusclegroupToViewModel(musclegroup)); 
            }
        }
    }
}
