using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.Mappings;
using GymExerciseClassLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymExerciseClassLibrary.ViewModels
{
    public partial class TrainingUpdateViewModel : ObservableValidator
    {
        private readonly ApplicationDbContext _context;
        [ObservableProperty]
        private ObservableCollection<ExerciseViewModel> _exercisesToChooseFrom = new();
        [ObservableProperty]
        private TrainingViewModel _training;

        public TrainingUpdateViewModel(ApplicationDbContext context, TrainingViewModel training)
        {
            _context = context;
            _training = training;
            LoadExercisesToChooseFrom();
        }

        private async void LoadExercisesToChooseFrom()
        {
            ExercisesToChooseFrom.Clear();
            var allExercises = await _context.Exercises
                                    //.AsNoTracking()
                                    .Include(e => e.Musclegroup)
                                    .Include(e => e.Sets)
                                    .ToListAsync();

            // add current ExercisesOfTraining to ExercisesToChooseFrom
            foreach (var exercise in Training.ExercisesOfTraining)
            {
                ExercisesToChooseFrom.Add(exercise);
            }

            // load all remaining exercises from database and add to ExercisesToChooseFrom
            foreach (var exercise in allExercises)
            {
                if (Training.ExerciseIds.Contains(exercise.Id))
                {
                    continue;
                }
                ExercisesToChooseFrom.Add(Mapper.Map(exercise));
            }

            foreach (var exercise in ExercisesToChooseFrom)
            {
                foreach (var e in Training.ExercisesOfTraining)
                {
                    if (exercise.Id == e.Id)
                    {
                        exercise.IsSelected = true;
                    }
                }
            }
        }

        public void ToggleSelection(ExerciseViewModel exercise)
        {
            ExerciseViewModel? exerciseToRemove;
            if (exercise.IsSelected)
            {
                // Add the exercise to the selected list if it's selected
                if (!Training.ExerciseIds.Contains(exercise.Id))
                {
                    Training.ExercisesOfTraining.Add(exercise);
                    Training.ExerciseIds.Add(exercise.Id);
                }
            }
            else
            {
                // Remove the exercise from the selected list if it's deselected
                if (Training.ExerciseIds.Contains(exercise.Id))
                {    
                    exerciseToRemove = Training.ExercisesOfTraining.FirstOrDefault(e => e.Id == exercise.Id);
                    Training.ExercisesOfTraining.Remove(exercise);
                    Training.ExerciseIds.Remove(exercise.Id);
                }
            }
        }

        [RelayCommand]
        private async Task UpdateTraining()
        {
            bool hasErrors = Training.Validate();

            if (!hasErrors)
            {
                try
                {
                    _context.Trainings.Update(await Mapper.Map(_context, Training));
                    await _context.SaveChangesAsync();
                    await Shell.Current.GoToAsync("//MainPage");
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Message: {e.Message}\nInner Exception: {e.InnerException?.Message}");
                    throw;
                }
            }
            else
            {
                Training.CheckForErrorsCommand.Execute(null);
            }
        }
    }
}
