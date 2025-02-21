using GymExerciseClassLibrary.ViewModels;

namespace GymExerciseMauiApp;

public partial class ExercisesOfTrainingPage : ContentPage
{
	private readonly ExercisesOfTrainingViewModel _exercisesOfTrainingViewModel;
    public ExercisesOfTrainingPage(ExercisesOfTrainingViewModel exercisesOfTrainingViewModel)
	{
		InitializeComponent();
        _exercisesOfTrainingViewModel = exercisesOfTrainingViewModel;
		BindingContext = _exercisesOfTrainingViewModel;
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

    private void TriggerCheckForErrorsExerciseCommand(object sender, TextChangedEventArgs e)
    {
        if (sender is Entry entry && entry.BindingContext is ExerciseViewModel exercise)
        {
            // Call the command manually
            if (exercise.CheckForErrorsCommand.CanExecute(null))
            {
                exercise.CheckForErrorsCommand.Execute(null);
            }
        }
    }
}