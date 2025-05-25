using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.Mappings;
using GymExerciseClassLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymExerciseClassLibrary.ViewModels
{
    public partial class ExercisesOfTrainingViewModel : ObservableValidator
    {
        private readonly ApplicationDbContext _context;
        [ObservableProperty]
        private TrainingViewModel _training;

        public ExercisesOfTrainingViewModel(ApplicationDbContext context, TrainingViewModel training)
        {
            _context = context;
            _training = training;
        }

        [RelayCommand]
        private static void ToggleAdvancedOptions(ExerciseViewModel exercise)
        {
            exercise.IsAdvancedOptionsClicked = !exercise.IsAdvancedOptionsClicked;
        }

        [RelayCommand]
        private void ToggleExpand(ExerciseViewModel exercise)
        {
            // test: only 1 exercise can be expanded
            var expandedExercise = Training.ExercisesOfTraining.FirstOrDefault(e => e.IsExpanded == true);
            if (expandedExercise != null && expandedExercise != exercise)
            {
                expandedExercise.IsExpanded = false; // !expandedExercise.IsExpanded;
                exercise.IsExpanded = true;
                expandedExercise.IsAdvancedOptionsClicked = false;
            }
            else
            {
                exercise.IsExpanded = !exercise.IsExpanded;
                exercise.IsAdvancedOptionsClicked = false;
            }
            // default: all exercises can be expanded at once
            //exercise.IsExpanded = !exercise.IsExpanded;
        }

        [RelayCommand]
        private async Task IncreaseSets(ExerciseViewModel exercise)
        {
            var exerciseDb = await _context.Exercises.FirstOrDefaultAsync(e => e.Id == exercise.Id);

            if (exerciseDb == null)
            {
                return;
            }
            // increase exerciseDb.Sets by 1 and save to db
            int temp = exerciseDb.Sets;
            temp += 1;
            exerciseDb.Sets = temp;

            exerciseDb.Reps.Add(new Repetition
            {
                Set = temp,
                Reps = 0,
                Weight = 0
            });
            await _context.SaveChangesAsync();

            // updating view
            exercise.Reps.Clear();
            foreach (var rep in exerciseDb.Reps)
            {
                exercise.Reps.Add(Mapper.Map(rep));
            }
        }


        [RelayCommand]
        private async Task RemoveRep(RepetitionViewModel rep)
        {
            var repDb = await _context.Repetitions.FirstOrDefaultAsync(r => r.Id == rep.Id);
            if (repDb == null)
            {
                return;
            }
            _context.Repetitions.Remove(repDb);
            await _context.SaveChangesAsync();

            // updating view
            if (rep.ExerciseId != null)
            {
                return;
            }
            var exercise = Training.ExercisesOfTraining.FirstOrDefault(e => e.Id == rep.ExerciseId);
            if (exercise == null)
            {
                return;
            }
            exercise.Reps.Remove(rep);

            // TODO:
            // - popup: sicher dass du den letzten Satz löschen willst?
            // - rep.Set muss dann angepasst werden, wenn eine repetition gelöscht wird
            
            foreach (var repetition in exercise.Reps)
            {
                if (repetition.Set > rep.Set)
                {

                }
            }
        }
    }
}
