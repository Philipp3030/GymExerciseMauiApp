using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.Models;
using GymExerciseClassLibrary.ViewModels;
using System.Diagnostics;

namespace GymExerciseMauiApp;

public partial class AddTrainingPage : ContentPage
{
    private readonly ApplicationDbContext _context;
    private readonly AddTrainingViewModel _addTrainingViewModel;

    public AddTrainingPage(ApplicationDbContext context)
    {
        InitializeComponent();
        _context = context;
        _addTrainingViewModel = new AddTrainingViewModel(_context);
        BindingContext = _addTrainingViewModel;
    }

    private void OnExerciseCheckedChanged(object sender, EventArgs e)
    {
        var checkbox = (CheckBox)sender;
        var exercise = (ExerciseViewModel)checkbox.BindingContext;
        // Call the ToggleSelection method to update SelectedExercises
        _addTrainingViewModel.ToggleSelection(exercise);
    }
}