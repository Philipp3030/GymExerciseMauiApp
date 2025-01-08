using GymExerciseClassLibrary.Models;
using GymExerciseClassLibrary;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System.Diagnostics.Metrics;

namespace GymExerciseMauiApp;

public partial class AddExercisePage : ContentPage
{
    private readonly ApplicationDbContext _context;

    public AddExercisePage(ApplicationDbContext context)
	{
		InitializeComponent();
		_context = context;
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

    private async void OnSubmitClicked(object sender, EventArgs e)
    {
        var newExercise = new Exercise
        {
            Musclegroup = MusclegroupEntry.Text,
            Name = NameEntry.Text,
            IsActive = IsActiveSwitch.IsToggled,
            MachineName = MachineNameEntry.Text,
            Description = DescriptionEditor.Text,
            Sets = Convert.ToInt32(SetsEntry.Text),
            RepsPrevious = Convert.ToInt32(RepsPreviousEntry.Text),
            Reps = Convert.ToInt32(RepsEntry.Text),
            RepsGoal = Convert.ToInt32(RepsGoalEntry.Text)
        };

        try
        {
            await _context.Exercises.AddAsync(newExercise);
            await _context.SaveChangesAsync();
            ResultEditor.Text = $"Exercise Added successfully";
        }
        catch (Exception ex)
        {
            ResultEditor.Text = ex.Message;
        }
    }

    private void IdEntry_Completed(object sender, EventArgs e)
    {
        // Logic when Id entry editing is completed
    }

    private void MusclegroupEntry_Completed(object sender, EventArgs e)
    {
        // Logic when Musclegroup entry editing is completed
    }

    private void NameEntry_Completed(object sender, EventArgs e)
    {
        // Logic when Name entry editing is completed
    }

    private void IsActiveSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        // Logic when IsActive switch is toggled
    }

    private void MachineNameEntry_Completed(object sender, EventArgs e)
    {
        // Logic when MachineName entry editing is completed
    }

    private void DescriptionEditor_Completed(object sender, EventArgs e)
    {
        // Logic when Description editor editing is completed
    }

    private void SetsEntry_Completed(object sender, EventArgs e)
    {
        // Logic when Sets entry editing is completed
    }

    private void RepsPreviousEntry_Completed(object sender, EventArgs e)
    {
        // Logic when RepsPrevious entry editing is completed
    }

    private void RepsEntry_Completed(object sender, EventArgs e)
    {
        // Logic when Reps entry editing is completed
    }

    private void RepsGoalEntry_Completed(object sender, EventArgs e)
    {
        // Logic when RepsGoal entry editing is completed
    }
    private void AddToNewTraining(object sender, EventArgs e)
    {
        // 
    }
    private void AddToExistingTraining(object sender, EventArgs e)
    {
        // 
    }
}