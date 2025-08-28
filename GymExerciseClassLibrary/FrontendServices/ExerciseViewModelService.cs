using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.ViewModels;
using GymExerciseClassLibrary.ViewModels.Models;
using Microsoft.Maui.Handlers;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymExerciseClassLibrary.FrontendServices
{
    public class ExerciseViewModelService
    {
        public ExerciseViewModelService()
        {
        }

        #region VerifyExercise
        public bool VerifyExercise(ExerciseViewModel exercise)
        {
            if (!exercise.CheckForErrorsCommand.CanExecute(null))
            {
                return false;
            }
            exercise.CheckForErrorsCommand.Execute(null);
            if (exercise.HasErrors == true)
            {
                return false;
            }

            // check all sets for errors
            foreach (var s in exercise.Sets)
            {
                if (!s.CheckForErrorsCommand.CanExecute(null))
                {
                    return false;
                }
                s.CheckForErrorsCommand.Execute(null);
                if (s.HasErrors == true)
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region ResetColorOfExercise
        public void ResetColorOfExercise(ExerciseViewModel exercise)
        {
            exercise.Color = "#808080";
        }
        #endregion

        #region ChangeOrderOfExerciseInTraining
        public static bool IncreaseIndexOfExerciseInTrainingViewModel(ExerciseViewModel exerciseVm, TrainingViewModel trainingVm)
        {
            var exerciseIndexToChange = exerciseVm.ExerciseIndices.FirstOrDefault(exInd => exInd.Training.Id == trainingVm.Id);
            if (exerciseIndexToChange == null)
            {
                return false;
            }
            
            int oldIndex = exerciseIndexToChange.Index;
            exerciseIndexToChange.Index++;

            // if index is out of range, return
            if (!(trainingVm.ExercisesOfTraining.Count > exerciseIndexToChange.Index && exerciseIndexToChange.Index > 0))
            {
                exerciseIndexToChange.Index = oldIndex;
                return false;
            }

            ExerciseIndexViewModel? previousExerciseIndexOnThisPosition = null;
            foreach (var exercise in trainingVm.ExercisesOfTraining)
            {
                previousExerciseIndexOnThisPosition = exercise.ExerciseIndices
                    .FirstOrDefault(exInd => exInd.Training.Id == trainingVm.Id && exInd.Index == exerciseIndexToChange.Index && exInd.Id != exerciseIndexToChange.Id);
                if (previousExerciseIndexOnThisPosition != null)
                {
                    break;
                }
            }

            if (previousExerciseIndexOnThisPosition != null)
            {
                previousExerciseIndexOnThisPosition.Index = oldIndex;
            }

            return true;
        }

        public static bool DecreaseIndexOfExerciseInTrainingViewModel(ExerciseViewModel exerciseVm, TrainingViewModel trainingVm)
        {
            var exerciseIndexToChange = exerciseVm.ExerciseIndices.FirstOrDefault(exInd => exInd.Training.Id == trainingVm.Id);
            if (exerciseIndexToChange == null)
            {
                return false;
            }

            int oldIndex = exerciseIndexToChange.Index;
            exerciseIndexToChange.Index--;

            // if index is out of range, return
            if (!(trainingVm.ExercisesOfTraining.Count > exerciseIndexToChange.Index && exerciseIndexToChange.Index >= 0))
            {
                exerciseIndexToChange.Index = oldIndex;
                return false;
            }

            ExerciseIndexViewModel? previousExerciseIndexOnThisPosition = null;
            foreach (var exercise in trainingVm.ExercisesOfTraining)
            {
                previousExerciseIndexOnThisPosition = exercise.ExerciseIndices
                    .FirstOrDefault(exInd => exInd.Training.Id == trainingVm.Id && exInd.Index == exerciseIndexToChange.Index && exInd.Id != exerciseIndexToChange.Id);
                if (previousExerciseIndexOnThisPosition != null)
                {
                    break;
                }
            }

            if (previousExerciseIndexOnThisPosition != null)
            {
                previousExerciseIndexOnThisPosition.Index = oldIndex;
            }

            return true;
        }
        #endregion
    }
}
