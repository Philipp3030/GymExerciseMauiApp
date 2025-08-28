using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.FrontendServices;
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

namespace GymExerciseClassLibrary.ViewModels
{
    public partial class OverviewExercisesOfTrainingViewModel : ObservableValidator
    {
        private readonly TrainingService _trainingService;
        private readonly ExerciseService _exerciseService;
        private readonly SetService _setService;
        private readonly ExerciseViewModelService _vmService;
        [ObservableProperty]
        private TrainingViewModel _training;

        public OverviewExercisesOfTrainingViewModel(ApplicationDbContext context, TrainingViewModel training)
        {
            _training = training;
            _trainingService = new TrainingService(context);
            _exerciseService = new ExerciseService(context);
            _setService = new SetService(context);
            _vmService = new ExerciseViewModelService();
            SetColorOfExercises();

            TrainingViewModelService.SortExercisesOfTrainingByExerciseIndex(Training.ExercisesOfTraining, Training.Id);
        }

        private void SetColorOfExercises()
        {
            var activeExercises = Training.ExercisesOfTraining.Where(e => e.IsActive).ToList();
            var inactiveExercises = Training.ExercisesOfTraining.Where(e => !e.IsActive).ToList();

            foreach (var exercise in activeExercises)
            {
                exercise.Color = "#808080";
            }

            foreach (var exercise in inactiveExercises)
            {
                exercise.Color = "#800418";
            }
        }

        [RelayCommand]
        private static void ToggleAdvancedOptions(ExerciseViewModel exercise)
        {
            exercise.IsAdvancedOptionsClicked = !exercise.IsAdvancedOptionsClicked;
        }

        [RelayCommand]
        private void ToggleExpand(ExerciseViewModel exercise)
        {
            // test: only 1 exercise can be expanded
            var expandedExercise = Training.ExercisesOfTraining.FirstOrDefault(e => e.IsExpanded == true);
            if (expandedExercise != null && expandedExercise != exercise)
            {
                expandedExercise.IsExpanded = false; // !expandedExercise.IsExpanded;
                exercise.IsExpanded = true;
                expandedExercise.IsAdvancedOptionsClicked = false;
            }
            else
            {
                exercise.IsExpanded = !exercise.IsExpanded;
                exercise.IsAdvancedOptionsClicked = false;
            }
            // default: all exercises can be expanded at once
            //exercise.IsExpanded = !exercise.IsExpanded;
        }

        [RelayCommand]
        private async Task AddSet(ExerciseViewModel exercise)
        {
            await _setService.AddSet(exercise);
        }

        [RelayCommand]
        private async Task DeleteSet(SetViewModel setToRemove)
        {
            var exerciseOfSet = Training.ExercisesOfTraining.FirstOrDefault(e => e.Id == setToRemove.ExerciseId);

            if (exerciseOfSet != null)
            {
                await _setService.DeleteSet(setToRemove, exerciseOfSet); 
            }
        }

        public async Task UpdateExercise(ExerciseViewModel exerciseToUpdate)
        {
            bool isVerified = _vmService.VerifyExercise(exerciseToUpdate); ;           

            if (isVerified == true)
            {
                await _exerciseService.UpdateExercise(exerciseToUpdate);
            }
        }

        [RelayCommand]
        private async Task RemoveExerciseFromTraining(ExerciseViewModel exercise)
        {
            await _exerciseService.RemoveExerciseFromTraining(exercise, Training);
        }

        [RelayCommand]
        private async Task DeleteExercise(ExerciseViewModel exercise)
        {
            await _exerciseService.DeleteExercise(exercise, Training, null);
        }

        [RelayCommand]
        private async Task ResetStatusOfAllExercisesOfTraining()
        {
            foreach (var exercise in Training.ExercisesOfTraining)
            {
                exercise.IsActive = true;
                await _exerciseService.UpdateExercise(exercise);
                _vmService.ResetColorOfExercise(exercise);
                //exercise.Color = Color.FromArgb("#808080");
                //exercise.Color = "#808080";
                Debug.WriteLine($"Color for {exercise.Name} set to {exercise.Color}");
            }
        }

        [RelayCommand]
        private async Task IncreaseIndexOfExercise(ExerciseViewModel exercise)
        {
            bool canUpdate = ExerciseViewModelService.IncreaseIndexOfExerciseInTrainingViewModel(exercise, Training);
            if (canUpdate == true)
            {
                await _trainingService.UpdateTraining(Training);
            }
        }

        [RelayCommand]
        private async Task DecreaseIndexOfExercise(ExerciseViewModel exercise)
        {
            bool canUpdate = ExerciseViewModelService.DecreaseIndexOfExerciseInTrainingViewModel(exercise, Training);
            if (canUpdate == true)
            {
                await _trainingService.UpdateTraining(Training); 
            }
        }
    }
}
