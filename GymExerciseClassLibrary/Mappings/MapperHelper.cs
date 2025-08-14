//using GymExerciseClassLibrary.Data;
//using GymExerciseClassLibrary.Models;
//using GymExerciseClassLibrary.ViewModels;
//using GymExerciseClassLibrary.ViewModels.Models;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace GymExerciseClassLibrary.Mappings
//{
//    public static class MapperHelper
//    {
//        #region Exercise
//        // To Model
//        public static async Task<Exercise> MapBasicToModel(ApplicationDbContext context, ExerciseViewModel exerciseViewModel)
//        {
//            // get existing entity
//            var existingEntity = await context.Exercises.FirstOrDefaultAsync(e => e.Id == exerciseViewModel.Id && exerciseViewModel.Id != 0);

//            // if no entity exists
//            if (existingEntity == null)
//            {
//                throw new Exception("No existing entity found.");
//            }
//            return existingEntity;
//        }

//        // To ViewModel
//        public static ExerciseViewModel MapBasicToViewModel(Exercise exercise)
//        {
//            return new ExerciseViewModel { Id = exercise.Id };
//        }
//        #endregion

//        #region Training
//        // To Training
//        public static async Task<Training> MapBasicToModel(ApplicationDbContext context, TrainingViewModel trainingViewModel)
//        {
//            // get existing entity
//            var existingEntity = await context.Trainings.FirstOrDefaultAsync(e => e.Id == trainingViewModel.Id && trainingViewModel.Id != 0);

//            // if no entity exists
//            if (existingEntity == null)
//            {
//                throw new Exception("No existing entity found.");
//            }
//            return existingEntity;
//        }

//        // To ViewModel
//        public static TrainingViewModel MapBasicToViewModel(Training training)
//        {
//            return new TrainingViewModel { Id = training.Id };
//        }
//        #endregion
//    }
//}
