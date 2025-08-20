using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.Mappings;
using GymExerciseClassLibrary.Models;
using GymExerciseClassLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymExerciseClassLibrary.Services
{
    public class MusclegroupService
    {
        private readonly ApplicationDbContext _context;
        private readonly Mapper _mapper;

        public MusclegroupService(ApplicationDbContext context)
        {
            _context = context;
            _mapper = new Mapper(context);
        }

        public async Task SaveNewMusclegroup(MusclegroupViewModel musclegroup)
        {
            bool hasErrors = musclegroup.Validate();

            if (!hasErrors)
            {
                try
                {
                    // Save new musclegroup to database
                    _context.Musclegroups.Add(await _mapper.MapToModel(musclegroup));
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Message: {e.Message}\nStackTrace: {e.StackTrace}Inner Exception: {e.InnerException?.Message}");
                    throw;
                }
            }
            else
            {
                musclegroup.CheckForErrorsCommand.Execute(null);
            }
        }
    }
}
