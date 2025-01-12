using GymExerciseClassLibrary.Data;

namespace GymExerciseMauiApp
{
    public partial class App : Application
    {
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Apply migrations and seed data
                dbContext.SeedData();
            }

            MainPage = new AppShell();
        }
    }
}
