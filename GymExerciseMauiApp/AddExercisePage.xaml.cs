using GymExerciseClassLibrary.Models;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System.Diagnostics.Metrics;
using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.ViewModels;
using CommunityToolkit.Maui.Views;

namespace GymExerciseMauiApp;

public partial class AddExercisePage : ContentPage
{
    private readonly ApplicationDbContext _context;
    private readonly ExerciseViewModel _exerciseViewModel;

    public AddExercisePage(ApplicationDbContext context)
	{
		InitializeComponent();
		_context = context;
        _exerciseViewModel = new ExerciseViewModel(_context);
        BindingContext = _exerciseViewModel;
	}

    private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is Entry entry)
        {
            // Ensure the text contains only numeric characters
            if (!int.TryParse(entry.Text, out _))
            {
                entry.Text = new string(e.OldTextValue?.Where(char.IsDigit).ToArray() ?? Array.Empty<char>());
            }
        }
    }

    private void ShowAddMusclegroupPopup(object sender, EventArgs e)
    {
        var popup = new AddMusclegroupPopup(_context, _exerciseViewModel);
        this.ShowPopup(popup); // Display the popup
    }

    //private async void OnSubmitClicked(object sender, EventArgs e)
    //{
    //    var newExercise = new Exercise
    //    {
    //        Musclegroup = (Musclegroup)MusclegroupOptions.SelectedItem, // to viewmodel
    //        Name = NameEntry.Text,
    //        IsActive = IsActiveSwitch.IsToggled,
    //        MachineName = MachineNameEntry.Text,
    //        Description = DescriptionEditor.Text,
    //        Sets = Convert.ToInt32(SetsEntry.Text),
    //        RepsPrevious = Convert.ToInt32(RepsPreviousEntry.Text),
    //        Reps = Convert.ToInt32(RepsEntry.Text),
    //        RepsGoal = Convert.ToInt32(RepsGoalEntry.Text)
    //    };

    //    try
    //    {
    //        await _context.Exercises.AddAsync(newExercise);
    //        await _context.SaveChangesAsync();
    //        ResultEditor.Text = $"Exercise Added successfully";
    //    }
    //    catch (Exception ex)
    //    {
    //        ResultEditor.Text = ex.Message;
    //    }
    //}
}