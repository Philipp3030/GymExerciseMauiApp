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

    private void TriggerCheckForErrorsSetCommand(object sender, TextChangedEventArgs e)
    {
        if (sender is Entry entry && entry.BindingContext is SetViewModel set)
        {
            // Call the command manually
            if (set.CheckForErrorsCommand.CanExecute(null))
            {
                set.CheckForErrorsCommand.Execute(null);
            }
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _exerciseUpdateViewModel.InitializeAsync();
    }

}