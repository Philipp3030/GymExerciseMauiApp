using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.Models;
using GymExerciseClassLibrary.ViewModels;
using System.Diagnostics;

namespace GymExerciseMauiApp;

public partial class AddTrainingPage : ContentPage
{
    //private readonly ApplicationDbContext _context;
    //private readonly ExerciseViewModel _exerciseViewModel;

    private readonly TrainingViewModel _trainingViewModel;

    public AddTrainingPage(ApplicationDbContext context)
	{
		InitializeComponent();
        //_context = context;
        _trainingViewModel = new TrainingViewModel(context);
        BindingContext = _trainingViewModel;
        //_exerciseViewModel = new ExerciseViewModel(context);
        //BindingContext = _exerciseViewModel;
    }

    //private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
    //{
    //    if (sender is Entry entry)
    //    {
    //        // Ensure the text contains only numeric characters
    //        if (!int.TryParse(entry.Text, out _))
    //        {
    //            entry.Text = new string(e.OldTextValue?.Where(char.IsDigit).ToArray() ?? Array.Empty<char>());
    //        }
    //    }
    //}

    private void OnExerciseCheckedChanged(object sender, EventArgs e)
    {
        var checkbox = (CheckBox)sender;
        var exercise = (ExerciseViewModel)checkbox.BindingContext;

        // Call the ToggleSelection method to update SelectedExercises
        _trainingViewModel.ToggleSelection(exercise);
    }
}