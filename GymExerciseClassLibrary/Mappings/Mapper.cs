using GymExerciseClassLibrary.Models;
using GymExerciseClassLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymExerciseClassLibrary.Mappings
{
    public static class Mapper
    {
        public static ExerciseViewModel MapExerciseToViewModel(Exercise exercise)
        {
            ExerciseViewModel newExerciseVM = new ExerciseViewModel
            {
                Id = exercise.Id,
                Name = exercise.Name,
                IsActive = exercise.IsActive,
                Musclegroup = exercise.Musclegroup,
                MachineName = exercise.MachineName,
                Description = exercise.Description,
                Sets = exercise.Sets,
                RepsPrevious = exercise.RepsPrevious,
                Reps = exercise.Reps,
                RepsGoal = exercise.RepsGoal
            };
            return newExerciseVM;
        }

        public static Exercise MapExerciseViewModelToModel(ExerciseViewModel exerciseVM)
        {
            Exercise newExercise = new Exercise
            {
                Id = exerciseVM.Id,
                Name = exerciseVM.Name,
                IsActive = exerciseVM.IsActive,
                Musclegroup = exerciseVM.Musclegroup,
                MachineName = exerciseVM.MachineName,
                Description = exerciseVM.Description,
                Sets = exerciseVM.Sets,
                RepsPrevious = exerciseVM.RepsPrevious,
                Reps = exerciseVM.Reps,
                RepsGoal = exerciseVM.RepsGoal
            };
            return newExercise;
        }
        public static Training MapTrainingViewModelToModel(TrainingViewModel trainingVM)
        {
            Training newTraining = new Training
            {
                Name = trainingVM.Name,
                Description = trainingVM.Description
            };
            return newTraining;
        }

        public static TrainingViewModel MapTrainingToViewModel(Training training)
        {
            TrainingViewModel newTrainingVM = new TrainingViewModel
            {
                Name = training.Name,
                Description = training.Description
            };
            return newTrainingVM;
        }
    }
}
