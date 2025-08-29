using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.Mappings;
using GymExerciseClassLibrary.Models;
using GymExerciseClassLibrary.ViewModels;
using GymExerciseClassLibrary.ViewModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymExerciseClassLibrary.Services
{
    public class ExerciseIndexService
    {
        private readonly ApplicationDbContext _context;
        public ExerciseIndexService(ApplicationDbContext context)
        {
            _context = context;
        }

    }
}
