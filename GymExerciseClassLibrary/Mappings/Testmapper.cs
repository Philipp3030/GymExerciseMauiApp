using GymExerciseClassLibrary.Models;
using GymExerciseClassLibrary.ViewModels;
using GymExerciseClassLibrary.ViewModels.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymExerciseClassLibrary.Mappings
{
    public partial class Mapper // Change_0210
    {
        #region To Model
        private async Task<Training> MapTrainingVmToModel(TrainingViewModel trainingVm)
        {
            // map basic properties
            Training training = await MapBasicPropertiesOfTrainingVmToModel(trainingVm);

            #region exercises
            List<Exercise> exercises = new List<Exercise>();
            if (trainingVm.ExercisesOfTraining != null && trainingVm.ExercisesOfTraining.Any())
            {
                foreach (var exerciseVm in trainingVm.ExercisesOfTraining)
                {
                    exercises.Add(await MapExerciseVmToModel(exerciseVm, training));
                }
            }
            training.Exercises = exercises;
            #endregion

            #region exerciseIndices
            List<ExerciseIndex> exerciseIndices = new List<ExerciseIndex>();
            if (trainingVm.ExerciseIndices != null && trainingVm.ExerciseIndices.Any())
            {
                foreach (var exIndexVm in trainingVm.ExerciseIndices)
                {
                    var existingExercise = exercises.FirstOrDefault(e => e.Id == exIndexVm.Id);
                    if (exIndexVm.Exercise != null && exIndexVm.Exercise.Id != 0 && existingExercise != null)
                    {
                        exerciseIndices.Add(await MapExerciseIndexVmToModel(exIndexVm, existingExercise, training));
                    }
                }
            }
            training.ExerciseIndices = exerciseIndices;
            #endregion

            return training;
        }

        private async Task<Training> MapBasicPropertiesOfTrainingVmToModel(TrainingViewModel trainingVm)
        {
            var existingTraining = await _context.Trainings
                .Include(t => t.Exercises)  //then include....
                .Include(t => t.ExerciseIndices)
                .FirstOrDefaultAsync(t => trainingVm.Id != 0 && t.Id == trainingVm.Id);

            if (existingTraining == null)
            {
                return new Training()
                {
                    Name = trainingVm.Name,
                    Description = trainingVm.Description
                };
            }
            else
            {
                existingTraining.Name = trainingVm.Name;
                existingTraining.Description = trainingVm.Description;
                return existingTraining;
            }
        }

        private async Task<Exercise> MapExerciseVmToModel(ExerciseViewModel exerciseVm, Training training)
        {
            Exercise exercise = await MapBasicPropertiesOfExerciseVmToModel(exerciseVm);

            #region sets of exercise
            List<Set> sets = new List<Set>();
            if (exerciseVm.Sets != null && exerciseVm.Sets.Any())
            {
                foreach (var setVm in exerciseVm.Sets)
                {
                    sets.Add(await MapSetVmToModel(setVm, exercise));
                }
            }
            exercise.Sets = sets;
            #endregion

            //#region exerciseIndices of exercise
            //List<ExerciseIndex> exerciseIndices = new List<ExerciseIndex>();
            //if (exerciseVm.ExerciseIndices != null && exerciseVm.ExerciseIndices.Any())
            //{
            //    foreach (var exIndexVm in exerciseVm.ExerciseIndices)
            //    {
            //        exerciseIndices.Add(await MapExerciseIndexVmToModel_Training(exIndexVm, exercise, training));
            //    }
            //}
            //exercise.ExerciseIndices = exerciseIndices;
            //#endregion

            #region musclegroup of exercise
            if (exerciseVm.Musclegroup != null)
            {
                exercise.Musclegroup = await MapMusclegroupVmToModel(exerciseVm.Musclegroup);
            }
            #endregion

            return exercise;
        }

        private async Task<Exercise> MapBasicPropertiesOfExerciseVmToModel(ExerciseViewModel exerciseVm)
        {
            var existingExercise = await _context.Exercises
                .Include(e => e.Musclegroup)
                .Include(e => e.Sets)
                .FirstOrDefaultAsync(e => exerciseVm.Id != 0 && e.Id == exerciseVm.Id);

            if (existingExercise == null)
            {
                return new Exercise()
                {
                    Name = exerciseVm.Name,
                    IsActive = exerciseVm.IsActive,
                    MachineName = exerciseVm.MachineName,
                    Description = exerciseVm.Description,
                    AmountOfSets = string.IsNullOrWhiteSpace(exerciseVm.AmountOfSets) ? null : Convert.ToInt32(exerciseVm.AmountOfSets),
                    RepsGoal = string.IsNullOrWhiteSpace(exerciseVm.RepsGoal) ? null : Convert.ToInt32(exerciseVm.RepsGoal)
                };
            }
            else
            {
                existingExercise.Name = exerciseVm.Name;
                existingExercise.IsActive = exerciseVm.IsActive;
                existingExercise.MachineName = exerciseVm.MachineName;
                existingExercise.Description = exerciseVm.Description;
                existingExercise.AmountOfSets = string.IsNullOrWhiteSpace(exerciseVm.AmountOfSets) ? null : Convert.ToInt32(exerciseVm.AmountOfSets);
                existingExercise.RepsGoal = string.IsNullOrWhiteSpace(exerciseVm.RepsGoal) ? null : Convert.ToInt32(exerciseVm.RepsGoal);
                return existingExercise;
            }
        }

        private async Task<Set> MapSetVmToModel(SetViewModel setVm, Exercise exercise)
        {
            var existingSet = await _context.Sets.FirstOrDefaultAsync(s => setVm.Id != 0 && s.Id == setVm.Id);

            if (existingSet == null)
            {
                return new Set
                {
                    Index = setVm.Index,
                    Reps = string.IsNullOrWhiteSpace(setVm.Reps) ? null : Convert.ToInt32(setVm.Reps),
                    Weight = string.IsNullOrWhiteSpace(setVm.Weight) ? null : Convert.ToSingle(setVm.Weight),
                    Exercise = exercise
                };
            }
            else
            {
                existingSet.Index = setVm.Index;
                existingSet.Reps = string.IsNullOrWhiteSpace(setVm.Reps) ? null : Convert.ToInt32(setVm.Reps);
                existingSet.Weight = string.IsNullOrWhiteSpace(setVm.Weight) ? null : Convert.ToSingle(setVm.Weight);
                existingSet.Exercise = exercise;
                return existingSet;
            }
        }
       
        private async Task<ExerciseIndex> MapExerciseIndexVmToModel(ExerciseIndexViewModel exIndexVm, Exercise exercise, Training training)
        {
            var existingExIndex = await _context.ExerciseIndices.FirstOrDefaultAsync(e => exIndexVm.Id != 0 && e.Id == exIndexVm.Id);

            if (existingExIndex != null)
            {
                existingExIndex.Index = exIndexVm.Index;
                existingExIndex.Exercise = exercise;
                existingExIndex.Training = training;
                return existingExIndex;
            }
            else
            {
                return new ExerciseIndex()
                {
                    Index = exIndexVm.Index,
                    Exercise = exercise,
                    Training = training
                };
            }
        }

        private async Task<Musclegroup> MapMusclegroupVmToModel(MusclegroupViewModel musclegroupVm)
        {
            var existingMusclegroup = await _context.Musclegroups.FirstOrDefaultAsync(m => musclegroupVm.Id != 0 && m.Id == musclegroupVm.Id);

            if (existingMusclegroup == null)
            {
                return new Musclegroup
                {
                    Name = musclegroupVm.Name
                };
            }
            else
            {
                existingMusclegroup.Name = musclegroupVm.Name;
                return existingMusclegroup;
            }
        }
        #endregion

    }
}
