using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.Mappings;
using GymExerciseClassLibrary.Models;
using GymExerciseClassLibrary.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace GymExerciseClassLibrary.ViewModels
{
    public partial class UpdateExerciseViewModel : ObservableValidator
    {
        private readonly ApplicationDbContext _context;
        private readonly ExerciseService _exerciseService;
        [ObservableProperty]
        private ExerciseViewModel _exercise;
        [ObservableProperty]
        private ObservableCollection<MusclegroupViewModel> _musclegroups = new();

        public UpdateExerciseViewModel(ApplicationDbContext context, ExerciseViewModel exercise)
        {
            _context = context;
            _exerciseService = new ExerciseService(context);
            Exercise = exercise;
        }

        public async Task InitializeAsync()
        {
            int musclegroupId = Exercise.Musclegroup.Id;
            await LoadMusclegroupsFromDbAsync();
            Exercise.Musclegroup = Musclegroups.FirstOrDefault(m => m.Id == musclegroupId); // set value for picker to find entity
        }

        private async Task LoadMusclegroupsFromDbAsync()
        {
            Musclegroups.Clear();

            var musclegroups = await _context.Musclegroups.ToListAsync();
            foreach (var musclegroup in musclegroups)
            {
                Musclegroups.Add(Mapper.MapToViewModel(musclegroup, null));
            }
        }

        [RelayCommand]
        private async Task UpdateExercise()
        {
            await _exerciseService.UpdateExercise(Exercise);
        }
    }
}
