using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.Models;
using GymExerciseClassLibrary.ViewModels;
using GymExerciseClassLibrary.ViewModels.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace GymExerciseClassLibrary.Mappings
{
    public static class Mapper
    {
        #region ExerciseIndex
        // To Model
        public static async Task<ExerciseIndex> MapToModel(ApplicationDbContext context, ExerciseIndexViewModel exerciseIndexViewModel)
        {
            // get existing entity
            var existingEntity = await context.ExerciseIndices.FirstOrDefaultAsync(exInd => exInd.Id == exerciseIndexViewModel.Id && exerciseIndexViewModel.Id != 0);

            // if no entity exists
            if (existingEntity == null)
            {
                return new ExerciseIndex
                {
                    Index = exerciseIndexViewModel.Index,
                    ExerciseId = exerciseIndexViewModel.ExerciseId,
                    TrainingId = exerciseIndexViewModel.TrainingId
                };
            }
            // if entity exists, update properties of existing entity
            else
            {
                existingEntity.Index = exerciseIndexViewModel.Index;
                existingEntity.ExerciseId = exerciseIndexViewModel.ExerciseId;
                existingEntity.TrainingId = exerciseIndexViewModel.TrainingId;
                return existingEntity;
            }
        }

        // To ViewModel
        public static ExerciseIndexViewModel MapToViewModel(ExerciseIndex exerciseIndex, ExerciseIndexViewModel? exerciseIndexViewModel)
        {
            // if no entity exists, create a new instance
            if (exerciseIndexViewModel == null)
            {
                return new ExerciseIndexViewModel
                {
                    Id = exerciseIndex.Id,
                    Index = exerciseIndex.Index,
                    ExerciseId = exerciseIndex.ExerciseId,
                    TrainingId = exerciseIndex.TrainingId
                };
            }
            // if entity exists, update properties of existing entity
            else
            {
                exerciseIndexViewModel.Id = exerciseIndex.Id;
                exerciseIndexViewModel.Index = exerciseIndex.Index;
                exerciseIndexViewModel.ExerciseId = exerciseIndex.ExerciseId;
                exerciseIndexViewModel.TrainingId = exerciseIndex.TrainingId;
                return exerciseIndexViewModel;
            }
        }
        #endregion

        #region Musclegroup
        // To Model
        public static async Task<Musclegroup> MapToModel(ApplicationDbContext context, MusclegroupViewModel musclegroupViewModel)
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
        public static MusclegroupViewModel MapToViewModel(Musclegroup musclegroup, MusclegroupViewModel? musclegroupViewModel)
        {
            if (musclegroupViewModel == null)   // create new instance of MusclegroupViewModel
            {
                return new MusclegroupViewModel
                {
                    Id = musclegroup.Id,
                    Name = musclegroup.Name
                }; 
            }
            else // update existing instance of MusclegroupViewModel
            {
                musclegroupViewModel.Id = musclegroup.Id;
                musclegroupViewModel.Name = musclegroup.Name;
                return musclegroupViewModel;
            }
        }
        #endregion

        #region Set
        // To Model
        public static async Task<Set> MapToModel(ApplicationDbContext context, SetViewModel setViewModel)
        {
            // get existing entity
            var existingEntity = await context.Sets.FirstOrDefaultAsync(e => e.Id == setViewModel.Id && setViewModel.Id != 0);

            // if no entity exists, create a new instance
            if (existingEntity == null)
            {
                return new Set
                {
                    Index = setViewModel.Index,
                    Reps = setViewModel.Reps == string.Empty || setViewModel.Reps == null ? null : Convert.ToInt32(setViewModel.Reps),
                    Weight = setViewModel.Weight == string.Empty || setViewModel.Weight == null ? null : Convert.ToInt32(setViewModel.Weight)
                };
            }
            // if entity exists, update properties of existing entity
            else
            {
                existingEntity.Index = setViewModel.Index;
                existingEntity.Reps = setViewModel.Reps == string.Empty || setViewModel.Reps == null ? null : Convert.ToInt32(setViewModel.Reps);
                existingEntity.Weight = setViewModel.Weight == string.Empty || setViewModel.Weight == null ? null : Convert.ToInt32(setViewModel.Weight);
                return existingEntity;
            }
        }

        // To ViewModel
        public static SetViewModel MapToViewModel(Set set, SetViewModel? setViewModel)
        {
            // if no entity exists, create a new instance
            if (setViewModel == null)
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
            // if entity exists, update properties of existing entity
            else
            {
                setViewModel.Id = set.Id;
                setViewModel.Index = set.Index;
                setViewModel.Reps = set.Reps.ToString();
                setViewModel.Weight = set.Weight.ToString();
                setViewModel.ExerciseId = set.Exercise.Id;
                return setViewModel;
            }
        }
        #endregion

        #region Exercise
        // To Model
        public static async Task<Exercise> MapToModel(ApplicationDbContext context, ExerciseViewModel exerciseViewModel)
        {
            // get existing entity
            var existingEntity = await context.Exercises.FirstOrDefaultAsync(e => e.Id == exerciseViewModel.Id && exerciseViewModel.Id != 0);

            // map sets
            var sets = new List<Set>();
            foreach (var set in exerciseViewModel.Sets)
            {
                sets.Add(await MapToModel(context, set));
            }

            // map exerciseIndices
            var exerciseIndices = new List<ExerciseIndex>();
            foreach (var exerciseIndex in exerciseViewModel.ExerciseIndices)
            {
                exerciseIndices.Add(await MapToModel(context, exerciseIndex));
            }

            // map musclegroup
            var musclegroup = exerciseViewModel.Musclegroup != null ? await MapToModel(context, exerciseViewModel.Musclegroup) : null;

            // if no entity exists, create a new instance
            if (existingEntity == null)
            {
                return new Exercise
                {
                    Name = exerciseViewModel.Name,
                    IsActive = exerciseViewModel.IsActive,
                    Musclegroup = musclegroup,
                    MachineName = exerciseViewModel.MachineName,
                    Description = exerciseViewModel.Description,
                    AmountOfSets = exerciseViewModel.AmountOfSets == string.Empty || exerciseViewModel.AmountOfSets == null ? null : Convert.ToInt32(exerciseViewModel.AmountOfSets),
                    Sets = sets,
                    RepsGoal = exerciseViewModel.RepsGoal == string.Empty || exerciseViewModel.RepsGoal == null ? null : Convert.ToInt32(exerciseViewModel.RepsGoal),
                    ExerciseIndices = exerciseIndices
                };
            }
            // if entity exists, update properties of existing entity
            else
            {
                existingEntity.Name = exerciseViewModel.Name;
                existingEntity.IsActive = exerciseViewModel.IsActive;
                existingEntity.Musclegroup = musclegroup;
                existingEntity.MachineName = exerciseViewModel.MachineName;
                existingEntity.Description = exerciseViewModel.Description;
                existingEntity.AmountOfSets = exerciseViewModel.AmountOfSets == string.Empty || exerciseViewModel.AmountOfSets == null ? null : Convert.ToInt32(exerciseViewModel.AmountOfSets);
                existingEntity.Sets = sets;
                existingEntity.RepsGoal = exerciseViewModel.RepsGoal == string.Empty || exerciseViewModel.RepsGoal == null ? null : Convert.ToInt32(exerciseViewModel.RepsGoal);
                existingEntity.ExerciseIndices = exerciseIndices;
                return existingEntity;
            }
        }

        // To ViewModel
        public static ExerciseViewModel MapToViewModel(Exercise exercise, ExerciseViewModel? exerciseViewModel)
        {
            ObservableCollection<int> trainingIds = new ObservableCollection<int>(
                exercise.Trainings.Select(t => t.Id) ?? []
            );

            ObservableCollection<ExerciseIndexViewModel> exerciseIndices = new ObservableCollection<ExerciseIndexViewModel>();
            foreach (var exerciseIndex in exercise.ExerciseIndices)
            {
                if (exerciseIndex == null) continue;
                var exerciseIndexViewModel = MapToViewModel(exerciseIndex, exerciseViewModel?.ExerciseIndices?.FirstOrDefault(exIndVM => exIndVM.Id == exerciseIndex.Id));
                if (exerciseIndexViewModel != null)
                {
                    exerciseIndices.Add(exerciseIndexViewModel);
                }
            }

            ObservableCollection<SetViewModel> sets = new ObservableCollection<SetViewModel>();
            foreach (var set in exercise.Sets)
            {
                if (set == null) continue;
                var setViewModel = MapToViewModel(set, exerciseViewModel?.Sets?.FirstOrDefault(s => s.Id == set.Id));
                if (setViewModel != null)
                {
                    sets.Add(setViewModel);
                }
            }

            MusclegroupViewModel? musclegroup = null;
            if (exercise.Musclegroup != null)
            {
                musclegroup = MapToViewModel(exercise.Musclegroup, exerciseViewModel?.Musclegroup); 
            }

            // if no entity exists, create a new instance
            if (exerciseViewModel == null)
            {
                return new ExerciseViewModel
                {
                    Id = exercise.Id,
                    Name = exercise.Name,
                    IsActive = exercise.IsActive,
                    Musclegroup = musclegroup,
                    MachineName = exercise.MachineName,
                    Description = exercise.Description,
                    AmountOfSets = exercise.AmountOfSets.ToString(),
                    Sets = sets,
                    RepsGoal = exercise.RepsGoal.ToString(),
                    TrainingIds = trainingIds,
                    ExerciseIndices = exerciseIndices
                }; 
            }
            // if entity exists, update properties of existing entity
            else
            {
                exerciseViewModel.Id = exercise.Id;
                exerciseViewModel.Name = exercise.Name;
                exerciseViewModel.IsActive = exercise.IsActive;
                exerciseViewModel.Musclegroup = musclegroup;
                exerciseViewModel.MachineName = exercise.MachineName;
                exerciseViewModel.Description = exercise.Description;
                exerciseViewModel.AmountOfSets = exercise.AmountOfSets.ToString();
                exerciseViewModel.Sets = sets;
                exerciseViewModel.RepsGoal = exercise.RepsGoal.ToString();
                exerciseViewModel.TrainingIds = trainingIds;
                exerciseViewModel.ExerciseIndices = exerciseIndices;
                return exerciseViewModel;
            }
        }
        #endregion

        #region Training
        // To Training
        public static async Task<Training> MapToModel(ApplicationDbContext context, TrainingViewModel trainingViewModel)
        {
            // get existing entity
            var existingEntity = await context.Trainings.FirstOrDefaultAsync(e => e.Id == trainingViewModel.Id && trainingViewModel.Id != 0);

            // map exercises
            var exercises = new List<Exercise>();
            foreach (var exercise in trainingViewModel.ExercisesOfTraining)
            {
                exercises.Add(await MapToModel(context, exercise));
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
        public static TrainingViewModel MapToViewModel(Training training, TrainingViewModel? trainingViewModel)
        {
            ObservableCollection<int> exerciseIds = new ObservableCollection<int>(
                training.Exercises.Select(t => t.Id) ?? []
            );

            ObservableCollection<ExerciseViewModel> exercises = new ObservableCollection<ExerciseViewModel>();
            foreach (var exercise in training.Exercises)
            {
                if (exercise == null) continue;
                var exerciseViewModel = MapToViewModel(exercise, trainingViewModel?.ExercisesOfTraining?.FirstOrDefault(exVM => exVM.Id == exercise.Id));
                if (exerciseViewModel != null)
                {
                    exercises.Add(exerciseViewModel);
                }
            }

            // if no entity exists, create a new instance
            if (trainingViewModel == null)
            {
                return new TrainingViewModel
                {
                    Id = training.Id,
                    Name = training.Name,
                    Description = training.Description,
                    ExercisesOfTraining = exercises,
                    ExerciseIds = exerciseIds
                }; 
            }
            // if entity exists, update properties of existing entity
            else
            {
                trainingViewModel.Id = training.Id;
                trainingViewModel.Name = training.Name;
                trainingViewModel.Description = training.Description;
                trainingViewModel.ExercisesOfTraining = exercises;
                trainingViewModel.ExerciseIds = exerciseIds;
                return trainingViewModel;
            }
        }
        #endregion
    }
}
