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
            ExerciseViewModel newExerciseVM = new ExerciseViewModel
            {
                Id = exercise.Id,
                Name = exercise.Name,
                IsActive = exercise.IsActive,
                SelectedMusclegroupVM = MapMusclegroupToViewModel(exercise.Musclegroup),
                MachineName = exercise.MachineName,
                Description = exercise.Description,
                Sets = exercise.Sets.ToString(),
                RepsPrevious = exercise.RepsPrevious.ToString(),
                Reps = exercise.Reps.ToString(),
                RepsGoal = exercise.RepsGoal.ToString()
            };
            return newExerciseVM;
        }

        public static Exercise MapExerciseViewModelToModel(ApplicationDbContext context, ExerciseViewModel exerciseVM)
        {
            var existingEntity = context.Exercises.FirstOrDefault(e => e.Id == exerciseVM.Id && exerciseVM.Id != 0);
            if (existingEntity != null)
            {
                return existingEntity;
            }
            else
            {
                return new Exercise
                {
                    Name = exerciseVM.Name,
                    IsActive = exerciseVM.IsActive,
                    Musclegroup = MapMusclegroupViewModelToModel(context, exerciseVM.SelectedMusclegroupVM),
                    MachineName = exerciseVM.MachineName,
                    Description = exerciseVM.Description,
                    Sets = Convert.ToInt32(exerciseVM.Sets),
                    RepsPrevious = Convert.ToInt32(exerciseVM.RepsPrevious),
                    Reps = Convert.ToInt32(exerciseVM.Reps),
                    RepsGoal = Convert.ToInt32(exerciseVM.RepsGoal)
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
                ExerciseVMsOfTraining = exercises
            };
            return newTrainingVM;
        }

        public static Training MapTrainingViewModelToModel(ApplicationDbContext context, TrainingViewModel trainingVM)
        {
            var existingEntity = context.Trainings.FirstOrDefault(e => e.Id == trainingVM.Id && trainingVM.Id != 0);
            if (existingEntity != null)
            {
                return existingEntity;
            }
            else
            {
                return new Training
                {
                    Name = trainingVM.Name,
                    Description = trainingVM.Description
                };
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

        public static Musclegroup MapMusclegroupViewModelToModel(ApplicationDbContext context, MusclegroupViewModel musclegroupVM)
        {
            var existingEntity = context.Musclegroups.FirstOrDefault(e => e.Id == musclegroupVM.Id && musclegroupVM.Id != 0);
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
    }
}
