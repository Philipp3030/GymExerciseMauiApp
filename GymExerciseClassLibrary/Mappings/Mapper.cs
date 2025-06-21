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

        #region Set
        // To Model
        public static async Task<Set> Map(ApplicationDbContext context, SetViewModel setViewModel)
        {
            // get existing entity
            var existingEntity = await context.Sets.FirstOrDefaultAsync(e => e.Id == setViewModel.Id && setViewModel.Id != 0);

            // if no entity exists, create a new instance
            if (existingEntity == null)
            {
                return new Set
                {
                    Index = setViewModel.Index,
                    Reps = Convert.ToInt32(setViewModel.Reps),
                    Weight = Convert.ToSingle(setViewModel.Weight)
                };
            }
            // if entity exists, update properties of existing entity
            else
            {
                existingEntity.Index = setViewModel.Index;
                existingEntity.Reps = Convert.ToInt32(setViewModel.Reps);
                existingEntity.Weight = Convert.ToSingle(setViewModel.Weight);
                return existingEntity;
            }
        }

        // To ViewModel
        public static SetViewModel Map(Set set)
        {
            return new SetViewModel
            {
                Id = set.Id,
                Index = set.Index,
                Reps = set.Reps.ToString(),
                Weight = set.Weight.ToString(),
                ExerciseId = set.Exercise.Id
            };
        }
        #endregion

        #region Exercise
        // To Model
        public static async Task<Exercise> Map(ApplicationDbContext context, ExerciseViewModel exerciseViewModel)
        {
            // get existing entity
            var existingEntity = await context.Exercises.FirstOrDefaultAsync(e => e.Id == exerciseViewModel.Id && exerciseViewModel.Id != 0);

            // map sets
            var sets = new List<Set>();
            foreach (var set in exerciseViewModel.Sets)
            {
                sets.Add(await Map(context, set));
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
                    AmountOfSets = Convert.ToInt32(exerciseViewModel.AmountOfSets),
                    Sets = sets,
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
                existingEntity.AmountOfSets = Convert.ToInt32(exerciseViewModel.AmountOfSets);
                existingEntity.Sets = sets;
                existingEntity.RepsGoal = Convert.ToInt32(exerciseViewModel.RepsGoal);
                return existingEntity;
            }
        }

        // To ViewModel
        public static ExerciseViewModel Map(Exercise exercise)
        {
            ObservableCollection<int> trainingIds = new();
            foreach (var training in exercise.Trainings)
            {
                trainingIds.Add(training.Id);
            }
            var sets = new ObservableCollection<SetViewModel>();
            foreach (var set in exercise.Sets)
            {
                sets.Add(Map(set));
            }
            return new ExerciseViewModel
            {
                Id = exercise.Id,
                Name = exercise.Name,
                IsActive = exercise.IsActive,
                Musclegroup = Map(exercise.Musclegroup),
                MachineName = exercise.MachineName,
                Description = exercise.Description,
                AmountOfSets = exercise.AmountOfSets.ToString(),
                Sets = sets,
                RepsGoal = exercise.RepsGoal.ToString(),
                TrainingIds = trainingIds
            };
        }
        #endregion

        #region Training
        // To Training
        public static async Task<Training> Map(ApplicationDbContext context, TrainingViewModel trainingViewModel)
        {
            // get existing entity
            var existingEntity = await context.Trainings
                .Include(t => t.Exercises)
                .FirstOrDefaultAsync(e => e.Id == trainingViewModel.Id && trainingViewModel.Id != 0);

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
            ObservableCollection<int> exerciseIds = new();
            foreach (var exercise in training.Exercises)
            {
                exerciseIds.Add(exercise.Id);
            }
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
                ExercisesOfTraining = exercises,
                ExerciseIds = exerciseIds
            };
        }
        #endregion
    }
}
