using GymExerciseClassLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using GymExerciseClassLibrary.Data;

namespace GymExerciseMauiApp
{
    public partial class MainPage : ContentPage
    {
        private readonly ApplicationDbContext _context;

        public MainPage(ApplicationDbContext context)
        {
            InitializeComponent();
            _context = context;
        }

        private async void NavigateToSavedTrainings(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SavedTrainings(_context));
        }

        private async void NavigateToSavedExercises(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SavedExercises(_context));
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
