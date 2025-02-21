using CommunityToolkit.Maui.Views;
using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.ViewModels;

namespace GymExerciseMauiApp;

public partial class AddMusclegroupPopup : Popup
{
    private readonly ApplicationDbContext _context;
    private readonly AddExerciseViewModel _addExerciseViewModel;

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

    private void OnAddClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is AddMusclegroupViewModel addMusclegroupVm)
        {
            bool hasErrors = addMusclegroupVm.Musclegroup.Validate();

            if (!hasErrors)
            {
                _addExerciseViewModel.CallLoadMusclegroupsFromDb();
                Close(); // Close the popup 
            }
            else
            {
                // Call the command manually
                if (addMusclegroupVm.Musclegroup.CheckForErrorsCommand.CanExecute(null))
                {
                    addMusclegroupVm.Musclegroup.CheckForErrorsCommand.Execute(null);
                }
            }
        }
    }
}