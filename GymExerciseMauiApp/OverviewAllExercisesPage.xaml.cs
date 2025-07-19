using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.ViewModels;
using Microsoft.EntityFrameworkCore;

#if ANDROID
using GymExerciseMauiApp.Custom;
#endif

namespace GymExerciseMauiApp;

public partial class OverviewAllExercisesPage : ContentPage
{
    private readonly ApplicationDbContext _context;
    private readonly NavigationDataService _navigationDataService;
    private readonly OverviewAllExercisesViewModel _overviewAllExercisesViewModel;

    public OverviewAllExercisesPage(ApplicationDbContext context, NavigationDataService navigationDataService)
	{
        InitializeComponent();
		_context = context;
        _navigationDataService = navigationDataService;
        _overviewAllExercisesViewModel = new OverviewAllExercisesViewModel(_context);
        BindingContext = _overviewAllExercisesViewModel;
    }

    private async void NavigateToUpdateExercise(object sender, TappedEventArgs e)
    {
        if (sender is Image image && image.BindingContext is ExerciseViewModel exercise)
        {
            _navigationDataService.Exercise = exercise;
            if (_navigationDataService.Exercise != null)
            {
                _navigationDataService.PreviousPageRoute = nameof(OverviewAllExercisesPage);
                await Shell.Current.GoToAsync(nameof(UpdateExercisePage));
            }
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _overviewAllExercisesViewModel.InitializeAsync();

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

    private async void OnEntryUnfocused(object sender, FocusEventArgs e)
    {
        if (sender is Entry setEntry && setEntry.BindingContext is SetViewModel set)
        {
            var exerciseOfSet = _overviewAllExercisesViewModel.Exercises.FirstOrDefault(e => e.Id == set.ExerciseId);
            if (exerciseOfSet == null)
            {
                return;
            }
            await _overviewAllExercisesViewModel.UpdateExercise(exerciseOfSet);
        }

        if (sender is Entry exerciseEntry && exerciseEntry.BindingContext is ExerciseViewModel exercise)
        {
            await _overviewAllExercisesViewModel.UpdateExercise(exercise);
        }
    }
}