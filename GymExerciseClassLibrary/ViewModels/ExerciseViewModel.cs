using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
    public partial class ExerciseViewModel : ObservableObject
    {
        private readonly ApplicationDbContext _context;

        [ObservableProperty]
        private ObservableCollection<Exercise> _exercises = new();
        [ObservableProperty]
        private int _id;
        [ObservableProperty]
        private string _musclegroup;
        [ObservableProperty]
        private string _name;
        [ObservableProperty]
        private bool _isSelected;

        public ExerciseViewModel(ApplicationDbContext context)
        {
            _context = context;
            LoadExercises();
        }

        private async void LoadExercises()
        {
            var exercises = await _context.Exercises.ToListAsync();
            foreach (var exercise in exercises)
            {
                Exercises.Add(exercise);
            }
        }
    }
}
