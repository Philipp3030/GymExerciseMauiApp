using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.ViewModels;
using Microsoft.Maui.Controls.Shapes;
using System.Collections.ObjectModel;
using System.Drawing;

#if ANDROID
using GymExerciseMauiApp.Custom;
#endif

namespace GymExerciseMauiApp;

public partial class OverviewExercisesOfTrainingPage : ContentPage
{
    private readonly ApplicationDbContext _context;
    private readonly OverviewExercisesOfTrainingViewModel _overviewExercisesOfTrainingViewModel;
    private readonly NavigationDataService _navigationDataService;

    // Updated for DI
    public OverviewExercisesOfTrainingPage(ApplicationDbContext context, NavigationDataService navigationDataService)
	{
		InitializeComponent();
        _context = context;
        _navigationDataService = navigationDataService;
        _overviewExercisesOfTrainingViewModel = new OverviewExercisesOfTrainingViewModel(_context, _navigationDataService.Training);
        BindingContext = _overviewExercisesOfTrainingViewModel;
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

    private async void OnEntryFocused(object sender, FocusEventArgs e)
    {
        if (sender is Entry entry)
        {
            if (entry != null)
            {
                await Task.Delay(100);

                // Change text to show keyboard, set cursor or something
                string temp = entry.Text;
                entry.Text = entry.Text + 999;
                entry.Text = temp; 
            }
        }
    }

    private async void NavigateToUpdateExercise(object sender, TappedEventArgs e)
    {
        if (sender is Image image && image.BindingContext is ExerciseViewModel exercise &&
            image.Parent is Grid grid && grid.Parent is Grid grid2 && grid2.Parent is Border border &&
            border.Parent is VerticalStackLayout layout && layout.Parent is CollectionView view &&
            view.BindingContext is OverviewExercisesOfTrainingViewModel exercisesOfTraining)
        {
            _navigationDataService.Exercise = exercise;
            _navigationDataService.Training = exercisesOfTraining.Training;
            if (_navigationDataService.Exercise != null && _navigationDataService.Training != null)
            {
                _navigationDataService.PreviousPageRoute = nameof(OverviewExercisesOfTrainingPage);
                await Shell.Current.GoToAsync(nameof(UpdateExercisePage));
            }
        }
    }

    private async void TriggerOnUnfocused(object sender, FocusEventArgs e)
    {
        if (sender is Entry setEntry && setEntry.BindingContext is SetViewModel set)
        {
            var exerciseOfSet = _overviewExercisesOfTrainingViewModel.Training.ExercisesOfTraining.FirstOrDefault(e => e.Id == set.ExerciseId);
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
            await _overviewExercisesOfTrainingViewModel.UpdateExercise(exerciseOfSet);
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
            await _overviewExercisesOfTrainingViewModel.UpdateExercise(exercise);
        }
    }
}