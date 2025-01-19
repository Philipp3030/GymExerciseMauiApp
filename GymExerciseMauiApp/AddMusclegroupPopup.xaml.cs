using CommunityToolkit.Maui.Views;
using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.ViewModels;
using SQLitePCL;
using System.Diagnostics;

namespace GymExerciseMauiApp;

public partial class AddMusclegroupPopup : Popup
{

    public AddMusclegroupPopup() {  }

    public AddMusclegroupPopup(ApplicationDbContext context, ExerciseViewModel exerciseVM)
	{
		InitializeComponent();
        BindingContext = exerciseVM.MusclegroupVM;
    }

    private void OnCancelClicked(object sender, EventArgs e)
    {
        Close(); // Close the popup
    }
}