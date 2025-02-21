using CommunityToolkit.Maui.Core.Extensions;
using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.ViewModels;
using Microsoft.Maui.Controls.Internals;

namespace GymExerciseMauiApp
{
    public partial class MainPage : ContentPage
    {
        private readonly ApplicationDbContext _context;

        public MainPage(ApplicationDbContext context)
        {
            InitializeComponent();
            _context = context;
            BindingContext = new MainViewModel(_context);
        }

        private async void OnLabelTapped(object sender, EventArgs e)
        {
            var frame = sender as Frame;
            var selectedTraining = frame?.BindingContext as TrainingViewModel;

            if (selectedTraining != null)
            {
                await Navigation.PushAsync(new ExercisesOfTrainingPage(new ExercisesOfTrainingViewModel(selectedTraining)));
            }
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

        private async void NavigateToSavedExercises(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SavedExercisesPage(_context));
        }

        private async void NavigateToAddTraining(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddTrainingPage(_context));
        }

        private async void NavigateToAddExercise(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddExercisePage(_context));
        }
    }

}
