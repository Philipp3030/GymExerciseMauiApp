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
    private readonly AddExerciseViewModel _addExerciseViewModel;

    public AddExercisePage(ApplicationDbContext context)
	{
		InitializeComponent();
		_context = context;
        _addExerciseViewModel = new AddExerciseViewModel(_context);
        BindingContext = _addExerciseViewModel;
	}

    private void TriggerCheckForErrorsCommand(object sender, EventArgs e)
    {
        _addExerciseViewModel.Exercise.CheckForErrorsCommand?.Execute(null);
    }

    private void ShowAddMusclegroupPopup(object sender, EventArgs e)
    {
        var popup = new AddMusclegroupPopup(_context, _addExerciseViewModel);
        this.ShowPopup(popup); // Display the popup
    }
}