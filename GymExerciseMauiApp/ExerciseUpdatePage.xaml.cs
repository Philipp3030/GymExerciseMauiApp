using GymExerciseClassLibrary.ViewModels;

namespace GymExerciseMauiApp;

public partial class ExerciseUpdatePage : ContentPage
{
    private readonly ExerciseUpdateViewModel _exerciseUpdateViewModel;
	public ExerciseUpdatePage(ExerciseUpdateViewModel exerciseUpdateViewModel)
	{
		InitializeComponent();
        _exerciseUpdateViewModel = exerciseUpdateViewModel;
        BindingContext = _exerciseUpdateViewModel;
	}

    private void TriggerCheckForErrorsCommand(object sender, EventArgs e)
    {
        _exerciseUpdateViewModel.Exercise.CheckForErrorsCommand?.Execute(null);
    }
}