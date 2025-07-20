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
    public class TrainingService
    {
        private readonly ApplicationDbContext _context;

        public TrainingService(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Create
        public async Task SaveNewTraining(TrainingViewModel training)
        {
            bool hasErrors = training.Validate();

            // Check if "Name" is empty
            if (!hasErrors)
            {
                try
                {
                    _context.Trainings.Add(await Mapper.Map(_context, training));
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Message: {e.Message}\nStackTrace: {e.StackTrace}Inner Exception: {e.InnerException?.Message}");
                    throw;
                }
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                training.CheckForErrorsCommand.Execute(null);
            }
        }
        #endregion

        #region Update
        public async Task UpdateTraining(TrainingViewModel training)
        {
            bool hasErrors = training.Validate();

            if (!hasErrors)
            {
                try
                {
                    _context.Trainings.Update(await Mapper.Map(_context, training));
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Message: {e.Message}\nStackTrace: {e.StackTrace}Inner Exception: {e.InnerException?.Message}");
                    throw;
                }
            }
            else
            {
                training.CheckForErrorsCommand.Execute(null);
            }
        }
        #endregion

        #region Delete
        public async Task DeleteTraining(TrainingViewModel training, ObservableCollection<TrainingViewModel> savedTrainings)
        {
            try
            {
                // remove from db
                var trainingDb = await _context.Trainings.FirstOrDefaultAsync(t => t.Id == training.Id);
                if (trainingDb == null)
                {
                    return;
                }

                _context.Trainings.Remove(trainingDb);
                await _context.SaveChangesAsync();

                // remove from view for animation
                savedTrainings.Remove(training);
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
