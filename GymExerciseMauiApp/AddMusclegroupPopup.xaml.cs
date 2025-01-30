using CommunityToolkit.Maui.Views;
using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.ViewModels;
using SQLitePCL;
using System.Diagnostics;

namespace GymExerciseMauiApp;

public partial class AddMusclegroupPopup : Popup
{
    private readonly ApplicationDbContext _context;
    private readonly AddExerciseViewModel _addExerciseViewModel;

    public AddMusclegroupPopup() {  }

    public AddMusclegroupPopup(ApplicationDbContext context, AddExerciseViewModel addExerciseViewModel)
	{
		InitializeComponent();
        _context = context;
        _addExerciseViewModel = addExerciseViewModel;
        BindingContext = new AddMusclegroupViewModel(_context);
    }

    private void OnCancelClicked(object sender, EventArgs e)
    {
        _addExerciseViewModel.CallLoadMusclegroupsFromDb();
        Close(); // Close the popup
    }
}