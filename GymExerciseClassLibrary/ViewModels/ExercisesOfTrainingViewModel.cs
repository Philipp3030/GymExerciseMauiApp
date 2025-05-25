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
            int temp = exerciseDb.AmountOfSets;
            temp += 1;
            exerciseDb.AmountOfSets = temp;

            exerciseDb.Sets.Add(new Set
            {
                Index = temp,
                Reps = 0,
                Weight = 0
            });
            await _context.SaveChangesAsync();

            // updating view
            exercise.Sets.Clear();
            foreach (var set in exerciseDb.Sets)
            {
                exercise.Sets.Add(Mapper.Map(set));
            }
        }


        [RelayCommand]
        private async Task RemoveSet(SetViewModel setToRemove)
        {
            var setDb = await _context.Sets.FirstOrDefaultAsync(r => r.Id == setToRemove.Id);
            if (setDb == null)
            {
                return;
            }
            _context.Sets.Remove(setDb);
            await _context.SaveChangesAsync();

            // updating view
            if (setToRemove.ExerciseId == null)
            {
                return;
            }
            var exercise = Training.ExercisesOfTraining.FirstOrDefault(e => e.Id == setToRemove.ExerciseId);
            if (exercise == null)
            {
                return;
            }
            exercise.Sets.Remove(setToRemove);

            // TODO:
            // - popup: sicher dass du den letzten Satz löschen willst?
            // - set.Index muss dann angepasst werden, wenn ein set gelöscht wird
            
            foreach (var set in exercise.Sets)
            {
                if (set.Index > setToRemove.Index)
                {
                    set.Index--;
                    // noch für db anpassen
                }
            }
        }
    }
}
