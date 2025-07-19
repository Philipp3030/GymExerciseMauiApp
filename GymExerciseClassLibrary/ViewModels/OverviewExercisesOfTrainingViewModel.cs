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
        private readonly ApplicationDbContext _context;
        private readonly ExerciseService _exerciseService;
        private readonly SetService _setService;
        private readonly ExerciseViewModelService _vmService;
        [ObservableProperty]
        private TrainingViewModel _training;

        public OverviewExercisesOfTrainingViewModel(ApplicationDbContext context, TrainingViewModel training)
        {
            _context = context;
            _training = training;
            _exerciseService = new ExerciseService(context);
            _setService = new SetService(context);
            _vmService = new ExerciseViewModelService();
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
    }
}
