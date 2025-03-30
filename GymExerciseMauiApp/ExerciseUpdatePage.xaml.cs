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

    private void TriggerCheckForErrorsRepCommand(object sender, TextChangedEventArgs e)
    {
        if (sender is Entry entry && entry.BindingContext is RepetitionViewModel rep)
        {
            // Call the command manually
            if (rep.CheckForErrorsCommand.CanExecute(null))
            {
                rep.CheckForErrorsCommand.Execute(null);
            }
        }
    }
}