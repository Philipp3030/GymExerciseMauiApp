using GymExerciseClassLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymExerciseClassLibrary.FrontendServices
{
    public class TrainingViewModelService
    {
        public static void SortExercisesOfTrainingByExerciseIndex(ObservableCollection<ExerciseViewModel> exercisesOfTrainingVm, int trainingsId)
        {
            // check if ExerciseIndex exists, if not return unchanged collection
            foreach (var exerciseVm in exercisesOfTrainingVm)
            {
                if (exerciseVm.ExerciseIndices.FirstOrDefault(exInd => exInd.Exercise.Id == exerciseVm.Id && exInd.Training.Id == trainingsId) == null)
                {
                    return;
                }
            }

            var sorted = exercisesOfTrainingVm
                .OrderBy(e => e.ExerciseIndices.First(exInd => exInd.Exercise.Id == e.Id && exInd.Training.Id == trainingsId).Index)
                .ToList();

            exercisesOfTrainingVm.Clear();
            foreach (var exerciseVm in sorted)
            {
                exercisesOfTrainingVm.Add(exerciseVm);
            }
        }
    }
}
