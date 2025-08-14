using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.Mappings;
using GymExerciseClassLibrary.Models;
using GymExerciseClassLibrary.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymExerciseClassLibrary.Services
{
    public class ExerciseService
    {
        private readonly ApplicationDbContext _context;

        public ExerciseService(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Create
        public async Task SaveNewExercise(ExerciseViewModel exercise)
        {
            bool hasErrors = exercise.Validate();

            // Check for errors
            if (!hasErrors)
            {
                try
                {
                    for (int i = 0; i < Convert.ToInt32(exercise.AmountOfSets); i++)
                    {
                        exercise.Sets.Add(new SetViewModel
                        {
                            Index = i + 1
                        });
                    }
                    _context.Exercises.Add(await Mapper.MapToModel(_context, exercise));
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Message: {e.Message}\nStackTrace: {e.StackTrace}Inner Exception: {e.InnerException?.Message}");
                    throw;
                }
                await Shell.Current.GoToAsync("..");    // in codebehind auslagern
            }
            else
            {
                exercise.CheckForErrorsOnSaveCommand.Execute(null);
            }
        }
        #endregion

        #region Update
        // Update
        public async Task UpdateExercise(ExerciseViewModel exercise)
        {
            bool hasErrors = exercise.Validate();

            if (hasErrors)
            {
                exercise.CheckForErrorsOnSaveCommand.Execute(null);
                return;
            }

            try
            {
                _context.Exercises.Update(await Mapper.MapToModel(_context, exercise));
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Message: {e.Message}\nStackTrace: {e.StackTrace}Inner Exception: {e.InnerException?.Message}");
                throw;
            }
        }

        #endregion

        #region Remove
        // Remove from training
        public async Task RemoveExerciseFromTraining(ExerciseViewModel exercise, TrainingViewModel training)
        {
            // remove relation of exercise to this training
            var exerciseDb = await _context.Exercises.FirstOrDefaultAsync(e => e.Id == exercise.Id);
            var trainingDb = await _context.Trainings
                .Include(t => t.Exercises)
                .FirstOrDefaultAsync(t => t.Id == training.Id);

            if (exerciseDb == null || trainingDb == null)
            {
                return;
            }

            try
            {
                trainingDb.Exercises.Remove(exerciseDb);
                _context.Update(trainingDb);
                await _context.SaveChangesAsync();

                // remove from view for animation
                training.ExercisesOfTraining.Remove(exercise);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Message: {e.Message}\nStackTrace: {e.StackTrace}Inner Exception: {e.InnerException?.Message}");
                throw;
            }
        }
        #endregion

        #region Delete
        // delete
        public async Task DeleteExercise(ExerciseViewModel exercise, TrainingViewModel? training, ObservableCollection<ExerciseViewModel>? exercises)
        {
            var exerciseDb = await _context.Exercises.FirstOrDefaultAsync(e => e.Id == exercise.Id);

            if (exerciseDb == null)
            {
                return;
            }

            try
            {
                _context.Exercises.Remove(exerciseDb);
                await _context.SaveChangesAsync();

                // remove from view for animation
                if (exercises != null)
                {
                    exercises.Remove(exercise);
                }

                if (training != null)
                {
                    training.ExercisesOfTraining.Remove(exercise);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Message: {e.Message}\nStackTrace: {e.StackTrace}Inner Exception: {e.InnerException?.Message}");
                throw;
            }
        }
        #endregion
    }
}
