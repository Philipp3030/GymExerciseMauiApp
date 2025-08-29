using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.FrontendServices;
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
        private readonly ExerciseIndexService _exerciseIndexService;
        private readonly Mapper _mapper;

        public TrainingService(ApplicationDbContext context)
        {
            _context = context;
            _exerciseIndexService = new ExerciseIndexService(context);
            _mapper = new Mapper(context);
        }

        // OnLoad: make sure "ExerciseIndices" are created for each Training - noch unsicher
        #region Verify

        #endregion

        #region Create
        public async Task SaveNewTraining(TrainingViewModel trainingVm)
        {
            bool hasErrors = trainingVm.Validate();

            // Check if "Name" is empty
            if (!hasErrors)
            {
                try
                {
                    TrainingViewModelService.CreateNewExerciseIndexForEachExerciseInTrainingViewModel(trainingVm);
                    Training trainingDb = await _mapper.MapToModel(trainingVm);
                    _context.Trainings.Add(trainingDb);
                    await _context.SaveChangesAsync();

                    _mapper.MapToViewModel(trainingDb, trainingVm);
                    TrainingViewModelService.SortExercisesOfTrainingByExerciseIndex(trainingVm.ExercisesOfTraining, trainingVm.Id);
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
                trainingVm.CheckForErrorsCommand.Execute(null);
            }
        }
        #endregion

        #region Update
        public async Task UpdateTraining(TrainingViewModel trainingVm)
        {
            bool hasErrors = trainingVm.Validate();

            if (!hasErrors)
            {
                try
                {
                    _context.Trainings.Update(await _mapper.MapToModel(trainingVm));
                    await _context.SaveChangesAsync();

                    TrainingViewModelService.SortExercisesOfTrainingByExerciseIndex(trainingVm.ExercisesOfTraining, trainingVm.Id);
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Message: {e.Message}\nStackTrace: {e.StackTrace}Inner Exception: {e.InnerException?.Message}");
                    throw;
                }
            }
            else
            {
                trainingVm.CheckForErrorsCommand.Execute(null);
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
