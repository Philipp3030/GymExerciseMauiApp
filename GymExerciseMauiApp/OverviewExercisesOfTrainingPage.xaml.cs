using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.ViewModels;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Dispatching;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using GymExerciseClassLibrary.FrontendServices;



#if ANDROID
using GymExerciseMauiApp.Custom;
#endif

namespace GymExerciseMauiApp;

public partial class OverviewExercisesOfTrainingPage : ContentPage
{
    private readonly ApplicationDbContext _context;
    private readonly OverviewExercisesOfTrainingViewModel _overviewExercisesOfTrainingViewModel;
    private readonly NavigationDataService _navigationDataService;

    // timer
    private IDispatcherTimer _timer;
    private Stopwatch _stopwatch;
    // Updated for DI
    public OverviewExercisesOfTrainingPage(ApplicationDbContext context, NavigationDataService navigationDataService, TimerService timerService)
    {
        InitializeComponent();
        _context = context;
        _navigationDataService = navigationDataService;
        _overviewExercisesOfTrainingViewModel = new OverviewExercisesOfTrainingViewModel(_context, _navigationDataService.Training);
        BindingContext = _overviewExercisesOfTrainingViewModel;

        _timer = timerService.Timer;
        _stopwatch = timerService.Stopwatch;
        _timer.Tick += (s, e) => OnTimerTick();
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
            image.Parent is Grid grid && grid.Parent is Grid grid2 &&
            grid2.Parent is Grid grid3 && grid3.Parent is Border border &&
            border.Parent is VerticalStackLayout layout && layout.Parent is SwipeView swipeView &&
            swipeView.Parent is CollectionView view && view.BindingContext is 
            OverviewExercisesOfTrainingViewModel exercisesOfTraining)
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

    private async void OnEntryUnfocused(object sender, FocusEventArgs e)
    {
        if (sender is Entry setEntry && setEntry.BindingContext is SetViewModel set)
        {
            var exerciseOfSet = _overviewExercisesOfTrainingViewModel.Training.ExercisesOfTraining.FirstOrDefault(e => e.Id == set.ExerciseId);
            if (exerciseOfSet == null)
            {
                return;
            }
            await _overviewExercisesOfTrainingViewModel.UpdateExercise(exerciseOfSet);
        }

        if (sender is Entry exerciseEntry && exerciseEntry.BindingContext is ExerciseViewModel exercise)
        {
            await _overviewExercisesOfTrainingViewModel.UpdateExercise(exercise);
        }
    }

    private void OnStartClicked(object sender, EventArgs e)
    {
        _stopwatch.Start();
        _timer.Start();
    }

    //private void OnStopClicked(object sender, EventArgs e)
    //{
    //    _timer.Stop();
    //    _stopwatch.Stop();
    //}

    private void OnResetClicked(object sender, EventArgs e)
    {
        _timer.Stop();
        _stopwatch.Reset();
        TimeLabelButton.Text = "00:00.00";
    }

    private void OnTimerTick()
    {
        TimeLabelButton.Text = _stopwatch.Elapsed.ToString(@"mm\:ss\.ff");
    }

    private async void SetStatusOfExercise(object sender, TappedEventArgs e)
    {
        if (sender is Image image && image.BindingContext is ExerciseViewModel exercise)
        {
            exercise.IsActive = !exercise.IsActive;
            if (exercise.IsActive)
            {
                exercise.Color = "#808080";
            }
            else
            {
                exercise.Color = "#800418";
            }
            await _overviewExercisesOfTrainingViewModel.UpdateExercise(exercise);
        }
    }

    private void OnTopSwiped(object sender, EventArgs e)
    {
        if (sender is SwipeItem swipeItem && swipeItem.BindingContext is ExerciseViewModel exercise)
        {
            _overviewExercisesOfTrainingViewModel.ToggleExpandCommand.Execute(exercise);
        }
    }
}