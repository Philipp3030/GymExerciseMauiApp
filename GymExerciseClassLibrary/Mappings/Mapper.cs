using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.Models;
using GymExerciseClassLibrary.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace GymExerciseClassLibrary.Mappings
{
    public static class Mapper
    {
        #region Musclegroup
        // To Model
        public static async Task<Musclegroup> Map(ApplicationDbContext context, MusclegroupViewModel musclegroupViewModel)
        {
            // get existing entity
            var existingEntity = await context.Musclegroups.FirstOrDefaultAsync(e => e.Id == musclegroupViewModel.Id && musclegroupViewModel.Id != 0);

            // if no entity exists, create a new instance
            if (existingEntity == null)
            {
                return new Musclegroup
                {
                    Name = musclegroupViewModel.Name
                };
            }
            // if entity exists, update properties of existing entity
            else
            {
                existingEntity.Name = musclegroupViewModel.Name;
                return existingEntity;
            }
        }

        // To ViewModel
        public static MusclegroupViewModel Map(Musclegroup musclegroup)
        {
            return new MusclegroupViewModel
            {
                Id = musclegroup.Id,
                Name = musclegroup.Name
            };
        }
        #endregion

        #region Repetition
        // To Model
        public static async Task<Repetition> Map(ApplicationDbContext context, RepetitionViewModel repetitionViewModel)
        {
            // get existing entity
            var existingEntity = await context.Repetitions.FirstOrDefaultAsync(e => e.Id == repetitionViewModel.Id && repetitionViewModel.Id != 0);

            // if no entity exists, create a new instance
            if (existingEntity == null)
            {
                return new Repetition
                {
                    Set = repetitionViewModel.Set,
                    Reps = Convert.ToInt32(repetitionViewModel.Reps),
                    Weight = Convert.ToSingle(repetitionViewModel.Reps)
                };
            }
            // if entity exists, update properties of existing entity
            else
            {
                existingEntity.Set = repetitionViewModel.Set;
                existingEntity.Reps = Convert.ToInt32(repetitionViewModel.Reps);
                existingEntity.Weight = Convert.ToSingle(repetitionViewModel.Reps);
                return existingEntity;
            }
        }

        // To ViewModel
        public static RepetitionViewModel Map(Repetition repetition)
        {
            return new RepetitionViewModel
            {
                Id = repetition.Id,
                Set = repetition.Set,
                Reps = repetition.Reps.ToString(),
                Weight = repetition.Weight.ToString()
            };
        }
        #endregion

        #region Exercise
        // To Model
        public static async Task<Exercise> Map(ApplicationDbContext context, ExerciseViewModel exerciseViewModel)
        {
            // get existing entity
            var existingEntity = await context.Exercises.FirstOrDefaultAsync(e => e.Id == exerciseViewModel.Id && exerciseViewModel.Id != 0);

            // map reps
            var reps = new List<Repetition>();
            foreach (var rep in exerciseViewModel.Reps)
            {
                reps.Add(await Map(context, rep));
            }

            // if no entity exists, create a new instance
            if (existingEntity == null)
            {

                return new Exercise
                {
                    Name = exerciseViewModel.Name,
                    IsActive = exerciseViewModel.IsActive,
                    Musclegroup = await Map(context, exerciseViewModel.Musclegroup),
                    MachineName = exerciseViewModel.MachineName,
                    Description = exerciseViewModel.Description,
                    Sets = Convert.ToInt32(exerciseViewModel.Sets),
                    Reps = reps,
                    RepsGoal = Convert.ToInt32(exerciseViewModel.RepsGoal)
                };
            }
            // if entity exists, update properties of existing entity
            else
            {
                existingEntity.Name = exerciseViewModel.Name;
                existingEntity.IsActive = exerciseViewModel.IsActive;
                existingEntity.Musclegroup = await Map(context, exerciseViewModel.Musclegroup);
                existingEntity.MachineName = exerciseViewModel.MachineName;
                existingEntity.Description = exerciseViewModel.Description;
                existingEntity.Sets = Convert.ToInt32(exerciseViewModel.Sets);
                existingEntity.Reps = reps;
                existingEntity.RepsGoal = Convert.ToInt32(exerciseViewModel.RepsGoal);
                return existingEntity;
            }
        }

        // To ViewModel
        public static ExerciseViewModel Map(Exercise exercise)
        {
            var reps = new ObservableCollection<RepetitionViewModel>();
            foreach (var rep in exercise.Reps)
            {
                reps.Add(Map(rep));
            }
            return new ExerciseViewModel
            {
                Id = exercise.Id,
                Name = exercise.Name,
                IsActive = exercise.IsActive,
                Musclegroup = Map(exercise.Musclegroup),
                MachineName = exercise.MachineName,
                Description = exercise.Description,
                Sets = exercise.Sets.ToString(),
                Reps = reps,
                RepsGoal = exercise.RepsGoal.ToString()
            };
        }
        #endregion

        #region Training
        // To Training
        public static async Task<Training> Map(ApplicationDbContext context, TrainingViewModel trainingViewModel)
        {
            // get existing entity
            var existingEntity = await context.Trainings.FirstOrDefaultAsync(e => e.Id == trainingViewModel.Id && trainingViewModel.Id != 0);

            // map exercises
            var exercises = new List<Exercise>();
            foreach (var exercise in trainingViewModel.ExercisesOfTraining)
            {
                exercises.Add(await Map(context, exercise));
            }

            // if no entity exists, create a new instance
            if (existingEntity == null)
            {

                return new Training
                {
                    Name = trainingViewModel.Name,
                    Description = trainingViewModel.Description,
                    Exercises = exercises

                };
            }
            // if entity exists, update properties of existing entity
            else
            {
                existingEntity.Name = trainingViewModel.Name;
                existingEntity.Description = trainingViewModel.Description;
                existingEntity.Exercises = exercises;
                return existingEntity;
            }
        }

        // To ViewModel
        public static TrainingViewModel Map(Training training)
        {
            var exercises = new ObservableCollection<ExerciseViewModel>();
            foreach (var exercise in training.Exercises)
            {
                exercises.Add(Map(exercise));
            }
            return new TrainingViewModel
            {
                Id = training.Id,
                Name = training.Name,
                Description = training.Description,
                ExercisesOfTraining = exercises
            };
        }
        #endregion
    }
}
