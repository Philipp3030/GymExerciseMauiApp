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
        private readonly ExerciseIndexService _exerciseIndexService;

        public TrainingService(ApplicationDbContext context)
        {
            _context = context;
            _exerciseIndexService = new ExerciseIndexService(context);
        }

        #region Create
        public async Task SaveNewTraining(TrainingViewModel trainingVm)
        {
            bool hasErrors = trainingVm.Validate();

            // Check if "Name" is empty
            if (!hasErrors)
            {
                try
                {
                    _exerciseIndexService.CreateNewExerciseIndexForEachExerciseInTrainingViewModel(trainingVm);
                    Training trainingDb = await Mapper.MapToModel(_context, trainingVm);
                    _context.Trainings.Add(trainingDb);
                    await _context.SaveChangesAsync();

                    //foreach (var exerciseIndex in await _context.ExerciseIndices.Where(e => e.TrainingId == 0).ToListAsync()) 
                    //{
                    //    exerciseIndex.TrainingId = trainingDb.Id;
                    //    _context.ExerciseIndices.Update(exerciseIndex);
                    //}
                    //await _context.SaveChangesAsync();
                    Mapper.MapToViewModel(trainingDb, trainingVm);





                    // save new empty training to get Id of training
                    //Training trainingDb = new Training();
                    //_context.Trainings.Add(trainingDb);
                    //await _context.SaveChangesAsync();

                    //// oder: bei der ToggleSelection von Add-/Update-Training immmer das ExerciseIndexViewModel erzeugen + an das ExerciseViewModel anheften
                    //// bzw wieder entfernen/löschen. Dann könnte der Mapper die


                    //// update ViewModel with id
                    //trainingVm.Id = trainingDb.Id;

                    //// save actual data of new training
                    ////_exerciseIndexService.CreateNewExerciseIndexForEachExerciseInTrainingViewModel(trainingVm, trainingDb);
                    
                    //trainingDb = await Mapper.MapToModel(_context, trainingVm);
                    //await _context.SaveChangesAsync();

                    //// update ViewModel
                    //Mapper.MapToViewModel(trainingDb, trainingVm);
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
        public async Task UpdateTraining(TrainingViewModel training)
        {
            bool hasErrors = training.Validate();

            if (!hasErrors)
            {
                try
                {
                    _context.Trainings.Update(await Mapper.MapToModel(_context, training));
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
