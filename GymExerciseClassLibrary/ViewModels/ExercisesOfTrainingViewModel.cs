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
            // increase exercise.Sets by 1
            int temp = Convert.ToInt32(exercise.Sets);
            temp += 1;
            exercise.Sets = temp.ToString();

            // create new Repetition
            exercise.Reps.Add(new RepetitionViewModel()
            {
                Set = temp,
                Reps = "0",
                Weight = "0"
            });

            await UpdateExercise(exercise, null);
        }

        [RelayCommand]
        private async Task DecreaseSets(ExerciseViewModel exercise)
        {
            // TODO
            // popup: sicher dass du den letzten Satz löschen willst?
            if (exercise.Sets != null && exercise.Sets == "0")
            {
                return;
            }

            // remove last Repetition
            // vielleicht erst in db löschen und dann view updaten? weniger raum für fehler?
            var repToRemove = exercise.Reps.FirstOrDefault(r => r.Set.ToString() == exercise.Sets);
            if (repToRemove != null)
            {
                exercise.Reps.Remove(repToRemove);
            }

            // decrease exercise.Sets by 1
            int temp = Convert.ToInt32(exercise.Sets);
            temp -= 1;
            exercise.Sets = temp.ToString();

            await UpdateExercise(exercise, repToRemove);
        }

        private async Task ReloadRepetitions(ExerciseViewModel exercise)
        {
            exercise.Reps.Clear();

            var repsFromModifiedExercise = await _context.Repetitions.Where(r => r.Exercise.Id == exercise.Id).ToListAsync();

            foreach (var rep in repsFromModifiedExercise)
            {
                exercise.Reps.Add(Mapper.Map(rep));
            }
        }

        private async Task UpdateExercise(ExerciseViewModel exercise, RepetitionViewModel? repToRemove)
        {
            if (exercise == null)
            {
                Debug.WriteLine("No exercise found");
                return;
            }

            bool hasErrors = exercise.Validate();
            foreach (var rep in exercise.Reps)
            {
                if (hasErrors)
                {
                    // return if there are errors
                    return;
                }
                hasErrors = rep.Validate();
            }

            if (!hasErrors)
            {
                try
                {
                    if (repToRemove != null)
                    {
                        // remove Repetition
                        var repModelToRemove = _context.Repetitions.FirstOrDefault(r => r.Id == repToRemove.Id);

                        if (repModelToRemove != null)
                        {
                            _context.Repetitions.Remove(repModelToRemove); 
                        }
                    }

                    _context.Exercises.Update(await Mapper.Map(_context, exercise));
                    await _context.SaveChangesAsync();
                    await ReloadRepetitions(exercise);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"{ex.Message} {ex.StackTrace}");
                    throw;
                } 
            }
        }
    }
}
