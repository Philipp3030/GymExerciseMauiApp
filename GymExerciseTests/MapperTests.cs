using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.Mappings;
using GymExerciseClassLibrary.Models;
using GymExerciseClassLibrary.ViewModels;
using GymExerciseClassLibrary.ViewModels.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace GymExerciseTests
{
    public class MapperTests
    {
        private SqliteConnection _connection;
        private ApplicationDbContext _context;

        [SetUp]
        public void Setup()
        {
            // Create in-memory SQLite connection
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open(); // Must remain open for the DB to persist during the test session

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new ApplicationDbContext(options);

            // Apply schema
            _context.Database.EnsureCreated();
        }

        [TearDown]
        public void Teardown()
        {
            _context.Dispose();
            _connection.Dispose();
        }

        [Test]
        public async Task DoesMapperMapTrainingVmToModel()
        {
            // Arrange
            #region Models
            Musclegroup musclegroup = new Musclegroup()
            {
                Id = 1,
                Name = "Arme"
            };

            Set set1 = new Set()
            {
                Id = 1,
                Index = 1,
                Reps = 7,
                Weight = 20
            };

            Set set2 = new Set()
            {
                Id = 2,
                Index = 2,
                Reps = 5,
                Weight = 20
            };

            Set set3 = new Set()
            {
                Id = 3,
                Index = 1,
                Reps = 8,
                Weight = 25
            };

            Set set4 = new Set()
            {
                Id = 4,
                Index = 2,
                Reps = 5,
                Weight = 20
            };

            Exercise exercise1 = new Exercise()
            {
                Id = 1,
                Name = "Bizeps Curls",
                Musclegroup = musclegroup,
                Sets = new List<Set> { set1, set2 }
            };

            Exercise exercise2 = new Exercise()
            {
                Id = 2,
                Name = "Trizepsdrücken",
                Musclegroup = musclegroup,
                Sets = new List<Set> { set3, set4 }
            };

            ExerciseIndex exerciseIndex1 = new ExerciseIndex()
            {
                Id = 1,
                Index = 0,
                Exercise = exercise1
            };

            ExerciseIndex exerciseIndex2 = new ExerciseIndex()
            {
                Id = 2,
                Index = 1,
                Exercise = exercise2
            };

            Training training1 = new Training()
            {
                Id= 1,
                Name = "Arme",
                Exercises = new List<Exercise> { exercise1, exercise2 },
                ExerciseIndices = new List<ExerciseIndex> { exerciseIndex1, exerciseIndex2 }
            };
            #endregion

            #region ViewModels
            MusclegroupViewModel musclegroupVM = new MusclegroupViewModel()
            {
                Id = 1,
                Name = "Arme"
            };

            SetViewModel setVm1 = new SetViewModel()
            {
                Id = 1,
                Index = 1,
                Reps = "7",
                Weight = "20"
            };

            SetViewModel setVm2 = new SetViewModel()
            {
                Id = 2,
                Index = 2,
                Reps = "5",
                Weight = "20"
            };

            SetViewModel setVm3 = new SetViewModel()
            {
                Id = 3,
                Index = 1,
                Reps = "8",
                Weight = "25"
            };

            SetViewModel setVm4 = new SetViewModel()
            {
                Id = 4,
                Index = 2,
                Reps = "5",
                Weight = "20"
            };

            ExerciseViewModel exerciseVm1 = new ExerciseViewModel()
            {
                Id = 1,
                Name = "Bizeps Curls",
                Musclegroup = musclegroupVM,
                Sets = new ObservableCollection<SetViewModel> { setVm1, setVm2 }
            };

            ExerciseViewModel exerciseVm2 = new ExerciseViewModel()
            {
                Id = 2,
                Name = "Trizepsdrücken",
                Musclegroup = musclegroupVM,
                Sets = new ObservableCollection<SetViewModel> { setVm3, setVm4 }
            };

            ExerciseIndexViewModel exerciseIndexVm1 = new ExerciseIndexViewModel()
            {
                Id = 1,
                Index = 0,
                Exercise = exerciseVm1
            };

            ExerciseIndexViewModel exerciseIndexVm2 = new ExerciseIndexViewModel()
            {
                Id = 2,
                Index = 1,
                Exercise = exerciseVm2
            };

            TrainingViewModel trainingVm1 = new TrainingViewModel()
            {
                Id = 1,
                Name = "Arme",
                ExercisesOfTraining = new ObservableCollection<ExerciseViewModel> { exerciseVm1, exerciseVm2 },
                ExerciseIndices = new ObservableCollection<ExerciseIndexViewModel> { exerciseIndexVm1, exerciseIndexVm2 }
            };
            #endregion

            _context.Add(training1);
            _context.SaveChanges();
            

            // Act
            Mapper mapper = new Mapper(_context);
            var mappedTraining = await mapper.MapTrainingVmToModelTest2(trainingVm1);

            // Assert

            TestContext.WriteLine($"Indeces: {training1.Exercises.Count}");
            var training = await _context.Trainings.FirstAsync();
            Assert.That(training.Name == "Arme");
            Assert.That(training1.ExerciseIndices.Count == 2);
            Assert.That(training.Exercises.Count == 2);
        }
    }
}
