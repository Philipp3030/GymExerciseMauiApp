using CommunityToolkit.Maui.Views;
using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.ViewModels;
using SQLitePCL;
using System.Diagnostics;

namespace GymExerciseMauiApp;

public partial class AddMusclegroupPopup : Popup
{
    private readonly ApplicationDbContext _context;
    private readonly MusclegroupViewModel _musclegroupVM;
    //private readonly ExerciseViewModel _exerciseVM;

    public AddMusclegroupPopup(ApplicationDbContext context, ExerciseViewModel exerciseVM)
	{
		InitializeComponent();
        _context = context;
        //_musclegroupVM = new MusclegroupViewModel(_context);
        //BindingContext = _musclegroupVM;
        //_exerciseVM = exerciseVM;
        BindingContext = exerciseVM;
    }

    private void OnCancelClicked(object sender, EventArgs e)
    {
        Close(); // Close the popup when "Cancel" is clicked
    }
}