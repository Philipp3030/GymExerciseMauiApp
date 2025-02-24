using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.ViewModels;

namespace GymExerciseMauiApp;

public partial class AddMusclegroupPage : ContentPage
{
    private readonly ApplicationDbContext _context;

    public AddMusclegroupPage(ApplicationDbContext context)
	{
		InitializeComponent();
		_context = context;
		BindingContext = new AddMusclegroupViewModel(_context);
	}
}