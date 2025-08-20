using GymExerciseClassLibrary.Models;
using GymExerciseClassLibrary.ViewModels;
using GymExerciseClassLibrary.ViewModels.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace GymExerciseClassLibrary.Mappings
{
    public partial class Mapper
    {
        #region To Model        
        private async Task<Exercise> MapExerciseVmToModel_Exercise(ExerciseViewModel exerciseVm)
        {
            Exercise exercise = await MapBasicPropertiesOfExerciseVmToModel_Exercise(exerciseVm);

            #region sets of exercise
            List<Set> sets = new List<Set>();
            if (exerciseVm.Sets != null && exerciseVm.Sets.Any())
            {
                foreach (var setVm in exerciseVm.Sets)
                {
                    sets.Add(await MapSetVmToModel_Exercise(setVm, exercise));
                }
            }
            exercise.Sets = sets;
            #endregion

            #region exerciseIndices of exercise
            List<ExerciseIndex> exerciseIndices = new List<ExerciseIndex>();
            if (exerciseVm.ExerciseIndices != null && exerciseVm.ExerciseIndices.Any())
            {
                foreach (var exIndexVm in exerciseVm.ExerciseIndices)
                {
                    var exIndex = await MapExerciseIndexVmToModel_Exercise(exIndexVm, exercise);
                    if (exIndex != null)
                    {
                        exerciseIndices.Add(exIndex); 
                    }
                }
            }
            exercise.ExerciseIndices = exerciseIndices;
            #endregion

            #region musclegroup of exercise
            if (exerciseVm.Musclegroup != null)
            {
                exercise.Musclegroup = await MapMusclegroupVmToModel_Exercise(exerciseVm.Musclegroup);
            }
            #endregion

            return exercise;
        }

        private async Task<Exercise> MapBasicPropertiesOfExerciseVmToModel_Exercise(ExerciseViewModel exerciseVm)
        {
            var existingExercise = await _context.Exercises.FirstOrDefaultAsync(e => exerciseVm.Id != 0 && e.Id == exerciseVm.Id);

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

        private async Task<Set> MapSetVmToModel_Exercise(SetViewModel setVm, Exercise exercise)
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

        private async Task<ExerciseIndex?> MapExerciseIndexVmToModel_Exercise(ExerciseIndexViewModel exIndexVm, Exercise exercise)
        {
            var existingExIndex = await _context.ExerciseIndices.FirstOrDefaultAsync(e => exIndexVm.Id != 0 && e.Id == exIndexVm.Id);
            if (existingExIndex == null)
            {
                return null;
            }
            existingExIndex.Index = exIndexVm.Index;
            existingExIndex.Exercise = exercise;    // is not necessary because exerciseIndex is in list of exercise -> fk is set automatically
            // existingExIndex.Training stays the same
            return existingExIndex;
        }

        private async Task<Musclegroup> MapMusclegroupVmToModel_Exercise(MusclegroupViewModel musclegroupVm)
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

        #region To ViewModel

        #region ExerciseViewModel 
        private ExerciseViewModel MapExerciseToViewModel_Exercise(Exercise exercise, ExerciseViewModel? exerciseVm)
        {
            // if no entity exists, create a new instance
            if (exerciseVm == null)
            {
                exerciseVm = MapBasicPropertiesOfExerciseToNewViewModel_Exercise(exercise);
            }
            // if entity exists, update properties of existing entity
            else
            {
                exerciseVm = MapBasicPropertiesOfExerciseToExistingViewModel_Exercise(exercise, exerciseVm);
            }

            #region sets of exercise
            ObservableCollection<SetViewModel> sets = new ObservableCollection<SetViewModel>();
            if (exercise.Sets != null && exercise.Sets.Any())
            {
                foreach (var set in exercise.Sets)
                {
                    // check if there is an existing setVm in this exerciseVm
                    var existingSetVm = exerciseVm.Sets?
                        .FirstOrDefault(s => s.Id == set.Id);

                    if (existingSetVm == null)
                    {
                        sets.Add(MapSetToNewViewModel_Exercise(set));
                    }
                    else
                    {
                        sets.Add(MapSetToExistingViewModel_Exercise(set, existingSetVm));
                    }
                }
            }
            if (exerciseVm.Sets != null)
            {
                UpdateCollection(exerciseVm.Sets, sets);
            }
            #endregion

            #region exerciseIndices of exercise
            ObservableCollection<ExerciseIndexViewModel> exerciseIndices = new ObservableCollection<ExerciseIndexViewModel>();
            if (exercise.ExerciseIndices != null && exercise.ExerciseIndices.Any())
            {
                foreach (var exerciseIndex in exercise.ExerciseIndices)
                {
                    // check if there is an existing exerciseIndexVm in this exerciseVm
                    var existingExIndexVm = exerciseVm.ExerciseIndices?
                        .FirstOrDefault(e => e.Id == exerciseIndex.Id);

                    if (existingExIndexVm == null)
                    {
                        if (exerciseIndex.Training != null)
                        {
                            exerciseIndices.Add(MapExerciseIndexToNewViewModel_Exercise(exerciseIndex, exerciseVm)); 
                        }
                    }
                    else
                    {
                        if (exerciseIndex.Training != null)
                        {
                            exerciseIndices.Add(MapExerciseIndexToExistingViewModel_Exercise(exerciseIndex, exerciseVm, existingExIndexVm));
                        }
                    }
                }
            }
            if (exerciseVm.ExerciseIndices != null)
            {
                UpdateCollection(exerciseVm.ExerciseIndices, exerciseIndices);
            }
            #endregion

            #region musclegroup of exercise
            if (exercise.Musclegroup != null)
            {
                if (exerciseVm.Musclegroup == null)
                {
                    exerciseVm.Musclegroup = MapMusclegroupToNewViewModel_Exercise(exercise.Musclegroup);
                }
                else
                {
                    exerciseVm.Musclegroup = MapMusclegroupToExistingViewModel_Exercise(exercise.Musclegroup, exerciseVm.Musclegroup);
                }
            }
            #endregion

            return exerciseVm;
        }

        private ExerciseViewModel MapBasicPropertiesOfExerciseToNewViewModel_Exercise(Exercise exercise)
        {
            return new ExerciseViewModel
            {
                Id = exercise.Id,
                Name = exercise.Name,
                IsActive = exercise.IsActive,
                MachineName = exercise.MachineName,
                Description = exercise.Description,
                AmountOfSets = exercise.AmountOfSets.ToString(),
                RepsGoal = exercise.RepsGoal.ToString()
            };
        }

        private ExerciseViewModel MapBasicPropertiesOfExerciseToExistingViewModel_Exercise(Exercise exercise, ExerciseViewModel exerciseVm)
        {
            exerciseVm.Id = exercise.Id;
            exerciseVm.Name = exercise.Name;
            exerciseVm.IsActive = exercise.IsActive;
            exerciseVm.MachineName = exercise.MachineName;
            exerciseVm.Description = exercise.Description;
            exerciseVm.AmountOfSets = exercise.AmountOfSets.ToString();
            exerciseVm.RepsGoal = exercise.RepsGoal.ToString();
            return exerciseVm;
        }
        #endregion

        #region TrainingViewModel
        private TrainingViewModel MapBasicPropertiesOfTrainingToNewViewModel_Exercise(Training training)
        {
            return new TrainingViewModel()
            {
                Id = training.Id,
                Name = training.Name,
                Description = training.Description
            };
        }
        #endregion

        #region SetViewModel 
        private SetViewModel MapSetToNewViewModel_Exercise(Set set)
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

        private SetViewModel MapSetToExistingViewModel_Exercise(Set set, SetViewModel setVm)
        {
            setVm.Id = set.Id;
            setVm.Index = set.Index;
            setVm.Reps = set.Reps.ToString();
            setVm.Weight = set.Weight.ToString();
            setVm.ExerciseId = set.Exercise.Id;
            return setVm;
        }
        #endregion

        #region ExerciseIndexViewModel
        private ExerciseIndexViewModel MapExerciseIndexToNewViewModel_Exercise(ExerciseIndex exerciseIndex, ExerciseViewModel exerciseVm)
        {
            TrainingViewModel trainingVm = MapBasicPropertiesOfTrainingToNewViewModel_Exercise(exerciseIndex.Training);

            return new ExerciseIndexViewModel
            {
                Id = exerciseIndex.Id,
                Index = exerciseIndex.Index,
                Exercise = exerciseVm,
                Training = trainingVm
            };
        }

        private ExerciseIndexViewModel MapExerciseIndexToExistingViewModel_Exercise(ExerciseIndex exerciseIndex, ExerciseViewModel exerciseVm, ExerciseIndexViewModel exerciseIndexVm)
        {
            TrainingViewModel trainingVm = MapBasicPropertiesOfTrainingToNewViewModel_Exercise(exerciseIndex.Training);

            exerciseIndexVm.Id = exerciseIndex.Id;
            exerciseIndexVm.Index = exerciseIndex.Index;
            exerciseIndexVm.Exercise = exerciseVm;
            exerciseIndexVm.Training = trainingVm;
            return exerciseIndexVm;
        }
        #endregion

        #region MusclegroupViewModel
        private MusclegroupViewModel MapMusclegroupToNewViewModel_Exercise(Musclegroup musclegroup)
        {
            return new MusclegroupViewModel
            {
                Id = musclegroup.Id,
                Name = musclegroup.Name
            };
        }

        private MusclegroupViewModel MapMusclegroupToExistingViewModel_Exercise(Musclegroup musclegroup, MusclegroupViewModel musclegroupVm)
        {
            musclegroupVm.Id = musclegroup.Id;
            musclegroupVm.Name = musclegroup.Name;
            return musclegroupVm;
        }
        #endregion

        #endregion
    }
}
