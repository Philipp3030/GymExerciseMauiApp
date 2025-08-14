using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.Mappings;
using GymExerciseClassLibrary.Models;
using GymExerciseClassLibrary.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GymExerciseClassLibrary.ViewModels
{
    public partial class AddExerciseViewModel : ObservableValidator
    {
        private readonly ApplicationDbContext _context;
        private readonly ExerciseService _exerciseService;
        [ObservableProperty]
        private ExerciseViewModel _exercise = new();
        [ObservableProperty]
        private ObservableCollection<MusclegroupViewModel> _musclegroups = new();

        public AddExerciseViewModel(ApplicationDbContext context)
        {
            _context = context;
            _exerciseService = new ExerciseService(context);
            LoadMusclegroupsFromDb();
        }

        private async void LoadMusclegroupsFromDb()
        {
            Musclegroups.Clear();

            var musclegroups = await _context.Musclegroups.ToListAsync();
            foreach (var musclegroup in musclegroups)
            {
                Musclegroups.Add(Mapper.MapToViewModel(musclegroup, null));
            }
        }

        public void CallLoadMusclegroupsFromDb()
        {
            LoadMusclegroupsFromDb();
        }

        [RelayCommand]
        private async Task SaveNewExercise()
        {
            await _exerciseService.SaveNewExercise(Exercise);
        }
    }
}
