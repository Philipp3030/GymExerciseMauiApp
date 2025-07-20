using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.Mappings;
using GymExerciseClassLibrary.Models;
using GymExerciseClassLibrary.Services;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GymExerciseClassLibrary.ViewModels
{
    public partial class AddTrainingViewModel : ObservableValidator
    {
        private readonly ApplicationDbContext _context;
        private readonly TrainingService _trainigService;
        [ObservableProperty]
        private ObservableCollection<ExerciseViewModel> _exercisesToChooseFrom = new();
        [ObservableProperty]
        private TrainingViewModel _training = new();

        public AddTrainingViewModel(ApplicationDbContext context)
        {
            _context = context;
            _trainigService = new TrainingService(context);
            LoadAllExercisesFromDb();
        }

        private async void LoadAllExercisesFromDb()
        {
            ExercisesToChooseFrom.Clear();

            var exercisesFromDb = await _context.Exercises
                                    .Include(e => e.Musclegroup)
                                    .Include(e => e.Sets)
                                    .ToListAsync();
            foreach (var exercise in exercisesFromDb)
            {
                ExercisesToChooseFrom.Add(Mapper.Map(exercise));
            }
        }

        public void ToggleSelection(ExerciseViewModel exercise)
        {
            if (exercise.IsSelected)
            {
                // Add the exercise to the selected list if it's selected
                if (!Training.ExercisesOfTraining.Contains(exercise))
                    Training.ExercisesOfTraining.Add(exercise);
            }
            else
            {
                // Remove the exercise from the selected list if it's deselected
                if (Training.ExercisesOfTraining.Contains(exercise))
                    Training.ExercisesOfTraining.Remove(exercise);
            }
        }

        [RelayCommand]
        private async Task SaveNewTraining()
        {
            await _trainigService.SaveNewTraining(Training);
        }
    }
}
