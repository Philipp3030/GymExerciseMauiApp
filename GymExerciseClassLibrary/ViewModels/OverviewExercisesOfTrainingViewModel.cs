using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.Mappings;
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
    public partial class OverviewExercisesOfTrainingViewModel : ObservableValidator
    {
        private readonly ApplicationDbContext _context;
        [ObservableProperty]
        private TrainingViewModel _training;

        public OverviewExercisesOfTrainingViewModel(ApplicationDbContext context, TrainingViewModel training)
        {
            _context = context;
            _training = training;
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
            var exerciseDb = await _context.Exercises
                .Include(e => e.Sets)
                .FirstOrDefaultAsync(e => e.Id == exercise.Id);

            if (exerciseDb == null)
            {
                return;
            }
            // increase exerciseDb.Sets by 1 and save to db
            int temp = exerciseDb.AmountOfSets;
            temp += 1;
            exerciseDb.AmountOfSets = temp;

            exerciseDb.Sets.Add(new Set
            {
                Index = temp,
                Reps = 0,
                Weight = 0
            });
            //_context.Exercises.Update(exerciseDb); // not needed because the exerciseDb object is being tracked.
            await _context.SaveChangesAsync();

            // updating view
            exercise.Sets.Clear();
            foreach (var set in exerciseDb.Sets)
            {
                exercise.Sets.Add(Mapper.Map(set));
            }
            exercise.AmountOfSets = exerciseDb.AmountOfSets.ToString();
        }

        [RelayCommand]
        private async Task RemoveSet(SetViewModel setToRemove)
        {
            // remove setToRemove from db
            var setDb = await _context.Sets
                .Include(s => s.Exercise)
                .FirstOrDefaultAsync(s => s.Id == setToRemove.Id);
            if (setDb == null)
            {
                return;
            }
            _context.Sets.Remove(setDb);

            // decrease AmountOfSets by 1 and readjust Index
            Exercise exerciseOfSet = setDb.Exercise;
            if (exerciseOfSet == null)
            {
                return;
            }
            foreach (var set in exerciseOfSet.Sets)
            {
                if (set.Index > setToRemove.Index)
                {
                    set.Index--;
                }
            }
            exerciseOfSet.AmountOfSets--;
            _context.Exercises.Update(exerciseOfSet);
            await _context.SaveChangesAsync();

            // updating view
            if (setToRemove.ExerciseId == null)
            {
                return;
            }
            var exercise = Training.ExercisesOfTraining.FirstOrDefault(e => e.Id == setToRemove.ExerciseId);
            if (exercise == null)
            {
                return;
            }
            exercise.Sets.Remove(setToRemove);
            exercise.AmountOfSets = exerciseOfSet.AmountOfSets.ToString();

            foreach (var set in exerciseOfSet.Sets)
            {
                var currentSet = exercise.Sets.FirstOrDefault(s => s.Id == set.Id);
                if (currentSet != null)
                {
                    currentSet.Index = set.Index;
                }
            }
            // TODO:
            // - popup: sicher dass du den Satz löschen willst?
        }

        public async Task UpdateExercise(ExerciseViewModel exerciseToUpdate)
        {
            try
            {
                Exercise? exercise = await Mapper.Map(_context, exerciseToUpdate);
                if (exercise != null)
                {
                    _context.Exercises.Update(exercise);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Message: {e.Message}\nStackTrace: {e.StackTrace}Inner Exception: {e.InnerException?.Message}");
                throw;
            }
        }

        [RelayCommand]
        private async Task RemoveExerciseFromTraining(ExerciseViewModel exercise)
        {
            try
            {
                // remove from db but only the relation to this Training
                var exerciseDb = await _context.Exercises.FirstOrDefaultAsync(e => e.Id == exercise.Id);
                var trainingDb = await _context.Trainings
                    .Include(t => t.Exercises)
                    .FirstOrDefaultAsync(t => t.Id == Training.Id);

                if (exerciseDb == null || trainingDb == null)
                {
                    return;
                }

                trainingDb.Exercises.Remove(exerciseDb);
                _context.Update(trainingDb);
                await _context.SaveChangesAsync();

                // remove from view for animation
                Training.ExercisesOfTraining.Remove(exercise);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Message: {e.Message}\nStackTrace: {e.StackTrace}Inner Exception: {e.InnerException?.Message}");
                throw;
            }
        }

        [RelayCommand]
        private async Task DeleteExercise(ExerciseViewModel exercise)
        {
            try
            {
                // remove from db but only the relation to this Training
                var exerciseDb = await _context.Exercises.FirstOrDefaultAsync(e => e.Id == exercise.Id);

                if (exerciseDb == null)
                {
                    return;
                }

                _context.Exercises.Remove(exerciseDb);
                await _context.SaveChangesAsync();

                // remove from view for animation
                Training.ExercisesOfTraining.Remove(exercise);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Message: {e.Message}\nStackTrace: {e.StackTrace}Inner Exception: {e.InnerException?.Message}");
                throw;
            }
        }
    }
}
