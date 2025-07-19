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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymExerciseClassLibrary.ViewModels
{
    public partial class OverviewAllExercisesViewModel : ObservableValidator
    {
        private readonly ApplicationDbContext _context;
        private readonly ExerciseService _service;
        private readonly ExerciseViewModelService _vmService;
        [ObservableProperty]
        ObservableCollection<ExerciseViewModel> _exercises = new();

        public OverviewAllExercisesViewModel(ApplicationDbContext context)
        {
            _context = context;
            _service = new ExerciseService(context);
            _vmService = new ExerciseViewModelService(context);
        }

        // executes onAppearing; constructor does NOT
        // maybe implement on all ViewModels?
        public async Task InitializeAsync()
        {
            await LoadExercisesFromDb();
        }

        private async Task LoadExercisesFromDb()
        {
            Exercises.Clear();

            var exercisesFromDb = await _context.Exercises
                                    .Include(e => e.Musclegroup)
                                    .Include(e => e.Sets)
                                    .ToListAsync();

            foreach (var exercise in exercisesFromDb)
            {
                Exercises.Add(Mapper.Map(exercise));
            }
        }

        [RelayCommand]
        private void ToggleExpand(ExerciseViewModel exercise)
        {
            // test: only 1 exercise can be expanded
            var expandedExercise = Exercises.FirstOrDefault(e => e.IsExpanded == true);
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
        }

        [RelayCommand]
        private async Task RemoveSet(SetViewModel setToRemove)
        {
        }

        public async Task UpdateExercise(ExerciseViewModel exerciseToUpdate)
        {
            bool isVerified = _vmService.VerifyExercise(exerciseToUpdate); ;

            if (isVerified == true)
            {
                await _service.UpdateExercise(exerciseToUpdate);
            }
        }

        [RelayCommand]
        private async Task DeleteExercise(ExerciseViewModel exercise)
        {
            await _service.DeleteExercise(exercise, null, Exercises);
        }
    }
}
