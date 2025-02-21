using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.Models;
using GymExerciseClassLibrary.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymExerciseClassLibrary.Mappings
{
    public static class Mapper
    {
        public static ExerciseViewModel MapExerciseToViewModel(Exercise exercise)
        {
            var reps = new ObservableCollection<RepetitionViewModel>();
            foreach (var rep in exercise.Reps)
            {
                reps.Add(MapRepetitionToViewModel(rep));
            }
            ExerciseViewModel newExerciseVM = new ExerciseViewModel
            {
                Id = exercise.Id,
                Name = exercise.Name,
                IsActive = exercise.IsActive,
                Musclegroup = MapMusclegroupToViewModel(exercise.Musclegroup),
                MachineName = exercise.MachineName,
                Description = exercise.Description,
                Sets = exercise.Sets.ToString(),
                //RepsPrevious = exercise.RepsPrevious.ToString(),
                Reps = reps,
                RepsGoal = exercise.RepsGoal.ToString()
            };
            return newExerciseVM;
        }

        public static async Task<Exercise> MapExerciseViewModelToModel(ApplicationDbContext context, ExerciseViewModel exercise)
        {
            var reps = new List<Repetition>();
            foreach (var rep in exercise.Reps)
            {
                reps.Add(await MapRepetitionViewModelToModel(context, rep));
            }
            var existingEntity = await context.Exercises.FirstOrDefaultAsync(e => e.Id == exercise.Id && exercise.Id != 0);
            if (existingEntity != null)
            {
                return existingEntity;
            }
            else
            {
                return new Exercise
                {
                    Name = exercise.Name,
                    IsActive = exercise.IsActive,
                    Musclegroup = await MapMusclegroupViewModelToModel(context, exercise.Musclegroup),
                    MachineName = exercise.MachineName,
                    Description = exercise.Description,
                    Sets = Convert.ToInt32(exercise.Sets),
                    //RepsPrevious = Convert.ToInt32(exerciseVM.RepsPrevious),
                    Reps = reps,
                    RepsGoal = Convert.ToInt32(exercise.RepsGoal)
                };
            }
        }

        public static TrainingViewModel MapTrainingToViewModel(Training training)
        {
            var exercises = new ObservableCollection<ExerciseViewModel>();
            foreach (var exercise in training.Exercises)
            {
                exercises.Add(MapExerciseToViewModel(exercise));
            }
            TrainingViewModel newTrainingVM = new TrainingViewModel
            {
                Id = training.Id,
                Name = training.Name,
                Description = training.Description,
                ExercisesOfTraining = exercises
            };
            return newTrainingVM;
        }

        public static async Task<Training> MapTrainingViewModelToModelAsync(ApplicationDbContext context, TrainingViewModel trainingVM)
        {
            var existingEntity = await context.Trainings.FirstOrDefaultAsync(e => e.Id == trainingVM.Id && trainingVM.Id != 0);
            if (existingEntity != null)
            {
                return existingEntity;
            }
            else
            {
                Training trainingToSaveToDb = new Training
                {
                    Name = trainingVM.Name,
                    Description = trainingVM.Description
                };

                foreach (var exercise in trainingVM.ExercisesOfTraining)
                {
                    trainingToSaveToDb.Exercises.Add(await MapExerciseViewModelToModel(context, exercise));
                }

                return trainingToSaveToDb;

                //return new Training
                //{
                //    Name = trainingVM.Name,
                //    Description = trainingVM.Description
                //};
            }
        }

        public static MusclegroupViewModel MapMusclegroupToViewModel(Musclegroup musclegroup)
        {
            MusclegroupViewModel newMusclegroupVM = new MusclegroupViewModel
            {
                Id = musclegroup.Id,
                Name = musclegroup.Name
            };
            return newMusclegroupVM;
        }

        public static async Task<Musclegroup> MapMusclegroupViewModelToModel(ApplicationDbContext context, MusclegroupViewModel musclegroupVM)
        {
            var existingEntity = await context.Musclegroups.FirstOrDefaultAsync(e => e.Id == musclegroupVM.Id && musclegroupVM.Id != 0);
            if (existingEntity != null)
            {
                return existingEntity;
            }
            else
            {
                return new Musclegroup
                {
                    Name = musclegroupVM.Name
                };
            }
        }

        public static RepetitionViewModel MapRepetitionToViewModel(Repetition repetition)
        {
            RepetitionViewModel newRepetition = new RepetitionViewModel
            {
                Id = repetition.Id,
                Set = repetition.Set,
                Count = repetition.Count.ToString()
            };
            return newRepetition;
        }

        public static async Task<Repetition> MapRepetitionViewModelToModel(ApplicationDbContext context, RepetitionViewModel repetition)
        {
            var existingEntity = await context.Repetitions.FirstOrDefaultAsync(e => e.Id == repetition.Id && repetition.Id != 0);
            if (existingEntity != null)
            {
                return existingEntity;
            }
            else
            {
                return new Repetition
                {
                    Set = repetition.Set,
                    Count = Convert.ToInt32(repetition.Count)
                };
            }
        }
    }
}
