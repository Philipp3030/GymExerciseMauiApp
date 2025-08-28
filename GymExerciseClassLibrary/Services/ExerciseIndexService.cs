using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.Mappings;
using GymExerciseClassLibrary.Models;
using GymExerciseClassLibrary.ViewModels;
using GymExerciseClassLibrary.ViewModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymExerciseClassLibrary.Services
{
    public class ExerciseIndexService
    {
        private readonly ApplicationDbContext _context;
        public ExerciseIndexService(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Create
        // move to TrainingViewModelService?
        dsadas
        public void CreateNewExerciseIndexForEachExerciseInTrainingViewModel(TrainingViewModel trainingVm)//, Training trainingDb)
        {
            if (trainingVm.ExercisesOfTraining != null && trainingVm.ExercisesOfTraining.Count > 0)
            {
                int index = 0;
                foreach (var exercise in trainingVm.ExercisesOfTraining)
                {
                    ExerciseIndexViewModel exerciseIndex = new ExerciseIndexViewModel();
                    exerciseIndex.Index = index++;                                              // training.Exercises.IndexOf(exercise); O(n) statt O(n^2)
                    //exerciseIndex.ExerciseId = exercise.Id;
                    //exerciseIndex.TrainingId = trainingDb.Id;
                    exercise.ExerciseIndices.Add(exerciseIndex);
                }
            }
        }
        #endregion
    }
}
