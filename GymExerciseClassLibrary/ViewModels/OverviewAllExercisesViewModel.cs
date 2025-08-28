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
        private readonly ExerciseService _exerciseService;
        private readonly SetService _setService;
        private readonly ExerciseViewModelService _vmService;
        private readonly Mapper _mapper;
        [ObservableProperty]
        ObservableCollection<ExerciseViewModel> _exercises = new();

        public OverviewAllExercisesViewModel(ApplicationDbContext context)
        {
            _context = context;
            _exerciseService = new ExerciseService(context);
            _setService = new SetService(context);
            _vmService = new ExerciseViewModelService();
            _mapper = new Mapper(context);
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
                                    .Include(e => e.ExerciseIndices)
                                        .ThenInclude(exInd => exInd.Training)
                                    .ToListAsync();

            foreach (var exercise in exercisesFromDb)
            {
                Exercises.Add(_mapper.MapToViewModel(exercise, null));
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
            await _setService.AddSet(exercise);
        }

        [RelayCommand]
        private async Task DeleteSet(SetViewModel setToRemove)
        {
            var exerciseOfSet = Exercises.FirstOrDefault(e => e.Id == setToRemove.ExerciseId);

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
        private async Task DeleteExercise(ExerciseViewModel exercise)
        {
            await _exerciseService.DeleteExercise(exercise, null, Exercises);
        }
    }
}
