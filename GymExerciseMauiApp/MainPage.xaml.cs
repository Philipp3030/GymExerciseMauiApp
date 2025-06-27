using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Views;
using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.ViewModels;
using Microsoft.Maui.Controls.Internals;

namespace GymExerciseMauiApp
{
    public partial class MainPage : ContentPage
    {
        private readonly ApplicationDbContext _context;
        private readonly NavigationDataService _navigationDataService;

        public MainPage(ApplicationDbContext context, NavigationDataService navigationDataService)
        {
            InitializeComponent();
            _context = context;
            _navigationDataService = navigationDataService;
            BindingContext = new MainViewModel(_context);
            Application.Current.UserAppTheme = AppTheme.Light;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            RefreshPage();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        private void RefreshPage()
        {
            (BindingContext as MainViewModel)?.ReloadData();
        }

        private async void NavigateToOverviewExercisesOfTrainingPage(object sender, TappedEventArgs e)
        {
            var grid = sender as Grid;
            var selectedTraining = grid?.BindingContext as TrainingViewModel;

            if (selectedTraining != null)
            {
                _navigationDataService.Training = selectedTraining;
                if (_navigationDataService.Training != null)
                {
                    await Shell.Current.GoToAsync(nameof(OverviewExercisesOfTrainingPage));
                }
                //await Navigation.PushAsync(new OverviewExercisesOfTrainingPage(_context, new OverviewExercisesOfTrainingViewModel(_context, selectedTraining)));
            }
        }

        private async void NavigateToTrainingUpdatePage(object sender, TappedEventArgs e)
        {
            var image = sender as Image;
            var selectedTraining = image?.BindingContext as TrainingViewModel;

            if (selectedTraining != null)
            {
                _navigationDataService.Training = selectedTraining;
                if (_navigationDataService.Training != null)
                {
                    await Shell.Current.GoToAsync(nameof(UpdateTrainingPage)); 
                }
            }
        }

        private async void NavigateToDeleteTrainingPage(object sender, TappedEventArgs e)
        {
            var image = sender as Image;
            var selectedTraining = image?.BindingContext as TrainingViewModel;

            if (selectedTraining != null)
            {
                _navigationDataService.Training = selectedTraining;
                if (_navigationDataService.Training != null)
                {
                    await Shell.Current.GoToAsync(nameof(DeleteTrainingPage));
                }
            }
        }

        private async void NavigateToSavedExercises(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(OverviewAllExercisesPage));
        }

        private async void NavigateToAddTraining(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(AddTrainingPage));
        }

        private async void NavigateToAddExercise(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(AddExercisePage));
        }
        
        private async void NavigateToAddMusclegroup(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(AddMusclegroupPage));
        }

        private async void NavigateToTestPage(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(TestPage));
        }
    }

}
