using GymExerciseClassLibrary.ViewModels;
using Microsoft.Maui.Controls.Shapes;
using System.Collections.ObjectModel;
#if ANDROID
using GymExerciseMauiApp.Custom;
#endif

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

    protected override void OnAppearing()
    {
        base.OnAppearing();

#if ANDROID
        ExerciseCollectionView.HandlerChanged += (s, e) =>
        {
            if (ExerciseCollectionView.Handler?.PlatformView is AndroidX.RecyclerView.Widget.RecyclerView recyclerView)
            {
                recyclerView.AddOnScrollListener(new CustomScrollListener(FocusStealer));
            }
        };
#endif
    }

    private async void HideKeyboardAndUnfocus(object sender, TappedEventArgs e)
    {
#if ANDROID
        var current = Platform.CurrentActivity?.CurrentFocus;
        current?.ClearFocus();
#endif
        await FocusStealer.HideSoftInputAsync(CancellationToken.None);
    }

    private void OnChangeClipClicked(object sender, FocusEventArgs e)
    {
        if (sender is Entry entry)
        {
            // Traverse up the visual tree to find the parent (template root)
            var parent = entry.Parent;

            while (parent != null && !(parent is Grid))
                parent = parent.Parent;

            if (parent is Grid grid)
            {
                var nameClip = grid.FindByName<RectangleGeometry>("NameEntryClip");
                var nameEntry = grid.FindByName<Entry>("NameEntry");
                var machineEntry = grid.FindByName<Entry>("MachineEntry");
                var machineClip = grid.FindByName<RectangleGeometry>("MachineEntryClip");

                //if (nameEntry != null)
                //{
                //    nameEntry.CursorPosition = 0;
                //}
                if (machineEntry != null)
                {
                    machineEntry.BackgroundColor = Colors.Aqua;
                    machineEntry.CursorPosition = machineEntry.Text?.Length ?? 0;
                }

                //if (nameClip != null)
                //{
                //    nameClip.Rect = new Rect(4, 10, 150, 50); // New clip
                //}
                //if (machineClip != null)
                //{
                //    machineClip.Rect = new Rect(4, 10, 80, 50); // New clip
                //}
            }
        }
    }

    private async void Entry_Focused(object sender, FocusEventArgs e)
    {
        if (sender is Entry entry)
        {
            await Task.Delay(100);
            // Unsubscribe to prevent triggering TextChanged
            //entry.TextChanged -= TriggerOnTextChanged;

            // Set new text on focus
            string temp = entry.Text;
            entry.Text = entry.Text + 999;
            entry.Text = temp;

            // Re-subscribe after change
            //entry.TextChanged += TriggerOnTextChanged;
        }
    }

    private async void TriggerOnUnfocused(object sender, FocusEventArgs e)
    {
        if (sender is Entry setEntry && setEntry.BindingContext is SetViewModel set)
        {
            var exerciseOfSet = _exercisesOfTrainingViewModel.Training.ExercisesOfTraining.FirstOrDefault(e => e.Id == set.ExerciseId);
            if (exerciseOfSet == null)
            {
                return;
            }
            if (!exerciseOfSet.CheckForErrorsCommand.CanExecute(null))
            {
                return;
            }
            exerciseOfSet.CheckForErrorsCommand.Execute(null);
            if (exerciseOfSet.HasErrors == true)
            {
                return;
            }

            // check all sets for errors
            foreach (var s in exerciseOfSet.Sets)
            {
                if (!s.CheckForErrorsCommand.CanExecute(null))
                {
                    return;
                }
                s.CheckForErrorsCommand.Execute(null);
                if (s.HasErrors == true)
                {
                    return;
                }
            }
            await _exercisesOfTrainingViewModel.UpdateExercise(exerciseOfSet);
        }

        if (sender is Entry exerciseEntry && exerciseEntry.BindingContext is ExerciseViewModel exercise)
        {
            if (!exercise.CheckForErrorsCommand.CanExecute(null))
            {
                return;
            }
            exercise.CheckForErrorsCommand.Execute(null);
            if (exercise.HasErrors == true)
            {
                return;
            }

            // check all sets for errors
            foreach (var s in exercise.Sets)
            {
                if (!s.CheckForErrorsCommand.CanExecute(null))
                {
                    return;
                }
                s.CheckForErrorsCommand.Execute(null);
                if (s.HasErrors == true)
                {
                    return;
                }
            }
            await _exercisesOfTrainingViewModel.UpdateExercise(exercise);
        }
    }
}