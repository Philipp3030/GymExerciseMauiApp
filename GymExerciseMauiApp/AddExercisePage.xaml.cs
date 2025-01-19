using GymExerciseClassLibrary.Models;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System.Diagnostics.Metrics;
using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.ViewModels;
using CommunityToolkit.Maui.Views;
using GymExerciseClassLibrary.Mappings;

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

    private void TriggerCheckForErrorsCommand(object sender, EventArgs e)
    {
        _exerciseViewModel.CheckForErrorsCommand?.Execute(null);
    }

    private void ShowAddMusclegroupPopup(object sender, EventArgs e)
    {
        _exerciseViewModel.MusclegroupVM = new MusclegroupViewModel(_context);
        var popup = new AddMusclegroupPopup(_context, _exerciseViewModel);
        this.ShowPopup(popup); // Display the popup
    }
}