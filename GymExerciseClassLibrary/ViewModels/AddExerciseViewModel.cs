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
using System.Xml.Linq;

namespace GymExerciseClassLibrary.ViewModels
{
    public partial class AddExerciseViewModel : ObservableValidator
    {
        private readonly ApplicationDbContext _context;
        [ObservableProperty]
        private ExerciseViewModel _exercise = new();
        [ObservableProperty]
        private ObservableCollection<MusclegroupViewModel> _musclegroups = new();

        public AddExerciseViewModel(ApplicationDbContext context)
        {
            _context = context;
            LoadMusclegroupsFromDb();
        }

        private async void LoadMusclegroupsFromDb()
        {
            Musclegroups.Clear();

            var musclegroups = await _context.Musclegroups.ToListAsync();
            foreach (var musclegroup in musclegroups)
            {
                Musclegroups.Add(Mapper.Map(musclegroup));
            }
        }

        public void CallLoadMusclegroupsFromDb()
        {
            LoadMusclegroupsFromDb();
        }

        [RelayCommand]
        private async Task SaveNewExercise()
        {
            //ValidateAllProperties();

            bool hasErrors = Exercise.Validate();

            // Check for errors
            if (!hasErrors)
            {
                try
                {
                    for (int i = 0; i < Convert.ToInt32(Exercise.AmountOfSets); i++)
                    {
                        Exercise.Sets.Add(new SetViewModel()
                        {
                            Index = i + 1
                        });
                    }
                    _context.Exercises.Add(await Mapper.Map(_context, Exercise)); 
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Message: {e.Message}\nInner Exception: {e.InnerException?.Message}");
                    throw;
                }
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                Exercise.CheckForErrorsOnSaveCommand.Execute(null);
            }
        }
    }
}
