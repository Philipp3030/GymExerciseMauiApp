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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymExerciseClassLibrary.ViewModels
{
    public partial class UpdateTrainingViewModel : ObservableValidator
    {
        private readonly ApplicationDbContext _context;
        private readonly TrainingService _trainingService;
        private readonly Mapper _mapper;
        [ObservableProperty]
        private ObservableCollection<ExerciseViewModel> _exercisesToChooseFrom = new();
        [ObservableProperty]
        private TrainingViewModel _training;

        public UpdateTrainingViewModel(ApplicationDbContext context, TrainingViewModel training)
        {
            _context = context;
            _trainingService = new TrainingService(context);
            _mapper = new Mapper(context);
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
                                    .Include(e => e.ExerciseIndices)
                                        .ThenInclude(exInd => exInd.Training)
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
                ExercisesToChooseFrom.Add(_mapper.MapToViewModel(exercise, null));
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
            await _trainingService.UpdateTraining(Training);
        }
    }
}
