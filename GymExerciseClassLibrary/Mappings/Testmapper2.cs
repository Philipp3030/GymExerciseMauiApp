using GymExerciseClassLibrary.Models;
using GymExerciseClassLibrary.ViewModels;
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
        public async Task<Training> MapTrainingVmToModelTest2(TrainingViewModel trainingVm)
        {
            Training training = await _context.Trainings
                .Include(t => t.Exercises)
                .Include(t => t.ExerciseIndices)
                .FirstOrDefaultAsync(t => trainingVm.Id != 0 && t.Id == trainingVm.Id) ?? new Training();

            training.Name = trainingVm.Name;
            training.Description = trainingVm.Description;

            #region exercises
            List<Exercise> exercises = new List<Exercise>();
            if (trainingVm.ExercisesOfTraining != null && trainingVm.ExercisesOfTraining.Any())
            {
                foreach (var exerciseVm in trainingVm.ExercisesOfTraining)
                {
                    Exercise exercise = await _context.Exercises
                        .Include(e => e.Musclegroup)
                        .Include(e => e.Sets)
                        .FirstOrDefaultAsync(e => exerciseVm.Id != 0 && e.Id == exerciseVm.Id) ?? new Exercise();
                    
                    exercise.Name = exerciseVm.Name;
                    exercise.IsActive = exerciseVm.IsActive;
                    exercise.MachineName = exerciseVm.MachineName;
                    exercise.Description = exerciseVm.Description;
                    exercise.AmountOfSets = string.IsNullOrWhiteSpace(exerciseVm.AmountOfSets) ? null : Convert.ToInt32(exerciseVm.AmountOfSets);
                    exercise.RepsGoal = string.IsNullOrWhiteSpace(exerciseVm.RepsGoal) ? null : Convert.ToInt32(exerciseVm.RepsGoal);

                    // vergleiche bestehende musclegroup von bestehenden Exercises mit zugehörigem ViewModel
                    if (exerciseVm.Musclegroup != null && exerciseVm.Musclegroup.Id != 0)
                    {
                        Musclegroup musclegroup = await _context.Musclegroups
                            .FirstOrDefaultAsync(m => m.Id == exerciseVm.Musclegroup.Id) ?? new Musclegroup();
                        
                        musclegroup.Name = exerciseVm.Musclegroup.Name;
                        exercise.Musclegroup = musclegroup;
                    }

                    // vergleiche bestehende sets von bestehenden Exercises mit zugehörigem ViewModel
                    if (exerciseVm.Sets != null && exerciseVm.Sets.Any())
                    {
                        List<Set> sets = new List<Set>();
                        foreach (var setVm in exerciseVm.Sets)
                        {
                            Set set = await _context.Sets.FirstOrDefaultAsync(s => setVm.Id != 0 && s.Id == setVm.Id) ?? new Set();

                            set.Index = setVm.Index;
                            set.Reps = string.IsNullOrWhiteSpace(setVm.Reps) ? null : Convert.ToInt32(setVm.Reps);
                            set.Weight = string.IsNullOrWhiteSpace(setVm.Weight) ? null : Convert.ToSingle(setVm.Weight);
                            set.Exercise = exercise;

                            sets.Add(set);
                        }
                        exercise.Sets = sets; 
                    }
                    exercises.Add(exercise);
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
                    ExerciseIndex exerciseIndex = await _context.ExerciseIndices
                        .Include(exInd => exInd.Exercise)
                        .Include(exInd => exInd.Training)
                        .FirstOrDefaultAsync(exInd => exIndexVm.Id != 0 && exInd.Id == exIndexVm.Id) ?? new ExerciseIndex();

                    // ausprobieren, ob klappt
                    if (training.Exercises.FirstOrDefault(e => e.Id == exIndexVm.Exercise.Id) is Exercise exercise)
                    {
                        exerciseIndex.Index = exIndexVm.Index;
                        exerciseIndex.Training = training;
                        exerciseIndex.Exercise = exercise;
                    }
                    exerciseIndices.Add(exerciseIndex);
                }
            }
            training.ExerciseIndices = exerciseIndices;
            #endregion

            return training;
        }
    }
}
