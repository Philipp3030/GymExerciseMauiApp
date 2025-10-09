using GymExerciseClassLibrary.Models;
using GymExerciseClassLibrary.ViewModels;
using GymExerciseClassLibrary.ViewModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Collections.ObjectModel;

namespace GymExerciseClassLibrary.Mappings
{
    public partial class Mapper   // Change_0210
    {
        #region To Model
        private async Task<Training> MapTrainingVmToModel_Training(TrainingViewModel trainingVm)
        {
            // map basic properties
            Training training = await MapBasicPropertiesOfTrainingVmToModel_Training(trainingVm);

            #region exercises
            List<Exercise> exercises = new List<Exercise>();
            if (trainingVm.ExercisesOfTraining != null && trainingVm.ExercisesOfTraining.Any())
            {
                foreach (var exerciseVm in trainingVm.ExercisesOfTraining)
                {
                    exercises.Add(await MapExerciseVmToModel_Training(exerciseVm, training));
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

        private async Task<Training> MapBasicPropertiesOfTrainingVmToModel_Training(TrainingViewModel trainingVm)
        {
            var existingTraining = await _context.Trainings
                .Include(t => t.Exercises)  //then include....
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

        private async Task<Exercise> MapExerciseVmToModel_Training(ExerciseViewModel exerciseVm, Training training)
        {
            Exercise exercise = await MapBasicPropertiesOfExerciseVmToModel_Training(exerciseVm);

            #region sets of exercise
            List<Set> sets = new List<Set>();
            if (exerciseVm.Sets != null && exerciseVm.Sets.Any())
            {
                foreach (var setVm in exerciseVm.Sets)
                {
                    sets.Add(await MapSetVmToModel_Training(setVm, exercise));
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
                exercise.Musclegroup = await MapMusclegroupVmToModel_Training(exerciseVm.Musclegroup);
            }
            #endregion

            return exercise;
        }

        private async Task<Exercise> MapBasicPropertiesOfExerciseVmToModel_Training(ExerciseViewModel exerciseVm)
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

        private async Task<Set> MapSetVmToModel_Training(SetViewModel setVm, Exercise exercise)
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

        private async Task<ExerciseIndex> MapExerciseIndexVmToModel_Training(ExerciseIndexViewModel exIndexVm, Exercise exercise, Training training)
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

        private async Task<Musclegroup> MapMusclegroupVmToModel_Training(MusclegroupViewModel musclegroupVm)
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

        #region TrainingViewModel
        private TrainingViewModel MapTrainingToViewModel_Training(Training training, TrainingViewModel? trainingVm)
        {
            // if no entity exists, create a new instance
            if (trainingVm == null)
            {
                trainingVm = MapBasicPropertiesOfTrainingToNewViewModel_Training(training);
            }
            // if entity exists, update properties of existing entity
            else
            {
                trainingVm = MapBasicPropertiesOfTrainingToExistingViewModel_Training(training, trainingVm);
            }

            ObservableCollection<ExerciseViewModel> exercises = new ObservableCollection<ExerciseViewModel>();
            if (training.Exercises != null && training.Exercises.Any())
            {
                foreach (var exercise in training.Exercises)
                {
                    // check if there is an existing exerciseVm in this trainingVm
                    var existingExerciseVm = trainingVm.ExercisesOfTraining?
                        .FirstOrDefault(e => e.Id == exercise.Id);

                    exercises.Add(MapExerciseToViewModel_Training(exercise, trainingVm, existingExerciseVm));
                }
            }
            if (trainingVm.ExercisesOfTraining != null)
            {
                UpdateCollection(trainingVm.ExercisesOfTraining, exercises);
            }

            return trainingVm;
        }

        private TrainingViewModel MapBasicPropertiesOfTrainingToNewViewModel_Training(Training training)
        {
            return new TrainingViewModel()
            {
                Id = training.Id,
                Name = training.Name,
                Description = training.Description
            };
        }

        private TrainingViewModel MapBasicPropertiesOfTrainingToExistingViewModel_Training(Training training, TrainingViewModel trainingVm)
        {
            trainingVm.Id = training.Id;
            trainingVm.Name = training.Name;
            trainingVm.Description = training.Description;
            return trainingVm;
        }
        #endregion

        #region ExerciseViewModel 
        private ExerciseViewModel MapExerciseToViewModel_Training(Exercise exercise, TrainingViewModel trainingVm, ExerciseViewModel? exerciseVm)
        {
            // if no entity exists, create a new instance
            if (exerciseVm == null)
            {
                exerciseVm = MapBasicPropertiesOfExerciseToNewViewModel_Training(exercise);
            }
            // if entity exists, update properties of existing entity
            else
            {
                exerciseVm = MapBasicPropertiesOfExerciseToExistingViewModel_Training(exercise, exerciseVm);
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
                        sets.Add(MapSetToNewViewModel_Training(set));
                    }
                    else
                    {
                        sets.Add(MapSetToExistingViewModel_Training(set, existingSetVm));
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
                        exerciseIndices.Add(MapExerciseIndexToNewViewModel_Training(exerciseIndex, exerciseVm));
                    }
                    else
                    {
                        exerciseIndices.Add(MapExerciseIndexToExistingViewModel_Training(exerciseIndex, exerciseVm, existingExIndexVm));
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
                    exerciseVm.Musclegroup = MapMusclegroupToNewViewModel_Training(exercise.Musclegroup);
                }
                else
                {
                    exerciseVm.Musclegroup = MapMusclegroupToExistingViewModel_Training(exercise.Musclegroup, exerciseVm.Musclegroup);
                }
            }
            #endregion

            return exerciseVm;
        }

        private ExerciseViewModel MapBasicPropertiesOfExerciseToNewViewModel_Training(Exercise exercise)
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

        private ExerciseViewModel MapBasicPropertiesOfExerciseToExistingViewModel_Training(Exercise exercise, ExerciseViewModel exerciseVm)
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

        #region SetViewModel 
        private SetViewModel MapSetToNewViewModel_Training(Set set)
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

        private SetViewModel MapSetToExistingViewModel_Training(Set set, SetViewModel setVm)
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
        private ExerciseIndexViewModel MapExerciseIndexToNewViewModel_Training(ExerciseIndex exerciseIndex, ExerciseViewModel exerciseVm)
        {
            return new ExerciseIndexViewModel
            {
                Id = exerciseIndex.Id,
                Index = exerciseIndex.Index,
                Exercise = exerciseVm,
                Training = MapBasicPropertiesOfTrainingToNewViewModel_Training(exerciseIndex.Training)
            };
        }

        private ExerciseIndexViewModel MapExerciseIndexToExistingViewModel_Training(ExerciseIndex exerciseIndex, ExerciseViewModel exerciseVm, ExerciseIndexViewModel exerciseIndexVm)
        {
            exerciseIndexVm.Id = exerciseIndex.Id;
            exerciseIndexVm.Index = exerciseIndex.Index;
            exerciseIndexVm.Exercise = exerciseVm;
            exerciseIndexVm.Training = MapBasicPropertiesOfTrainingToNewViewModel_Training(exerciseIndex.Training);
            return exerciseIndexVm;
        }
        #endregion

        #region MusclegroupViewModel
        private MusclegroupViewModel MapMusclegroupToNewViewModel_Training(Musclegroup musclegroup)
        {
            return new MusclegroupViewModel
            {
                Id = musclegroup.Id,
                Name = musclegroup.Name
            };
        }

        private MusclegroupViewModel MapMusclegroupToExistingViewModel_Training(Musclegroup musclegroup, MusclegroupViewModel musclegroupVm)
        {
            musclegroupVm.Id = musclegroup.Id;
            musclegroupVm.Name = musclegroup.Name;
            return musclegroupVm;
        }
        #endregion

        #endregion
    }
}
