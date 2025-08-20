using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.Mappings;
using GymExerciseClassLibrary.Models;
using GymExerciseClassLibrary.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymExerciseClassLibrary.Services
{
    public class SetService
    {
        private readonly ApplicationDbContext _context;
        private readonly Mapper _mapper;

        public SetService(ApplicationDbContext context)
        {
            _context = context;
            _mapper = new Mapper(context);
        }

        #region Create
        // create set and add to exercise
        public async Task AddSet(ExerciseViewModel exercise)
        {
            var exerciseDb = await _context.Exercises
                .Include(e => e.Sets)
                .FirstOrDefaultAsync(e => e.Id == exercise.Id);

            if (exerciseDb == null)
            {
                return;
            }

            // increase exerciseDb.Sets by 1 and save to db
            int? temp = exerciseDb.AmountOfSets == null ? 0 : exerciseDb.AmountOfSets;
            temp += 1;
            exerciseDb.AmountOfSets = temp;

            Set newSet = new Set
            {
                Index = temp,
                Reps = 0,
                Weight = 0
            };
            exerciseDb.Sets.Add(newSet);
            //_context.Exercises.Update(exerciseDb); // not needed because the exerciseDb object is being tracked.
            await _context.SaveChangesAsync();

            // updating view
            exercise.Sets.Add(_mapper.MapToViewModel(newSet, null));
            exercise.AmountOfSets = exerciseDb.AmountOfSets.ToString();
        }
        #endregion

        #region Delete
        // delete set and remove from exercise
        public async Task DeleteSet(SetViewModel setToRemove, ExerciseViewModel exerciseOfSet)
        {
            // remove setToRemove from db
            var setDb = await _context.Sets
                .Include(s => s.Exercise)
                .FirstOrDefaultAsync(s => s.Id == setToRemove.Id);
            if (setDb == null)
            {
                return;
            }
            _context.Sets.Remove(setDb);

            // decrease AmountOfSets by 1 and readjust Index
            Exercise exerciseOfSetDb = setDb.Exercise;
            if (exerciseOfSetDb == null)
            {
                return;
            }
            foreach (var set in exerciseOfSetDb.Sets)
            {
                if (set.Index > setToRemove.Index)
                {
                    set.Index--;
                }
            }
            exerciseOfSetDb.AmountOfSets--;
            _context.Exercises.Update(exerciseOfSetDb);
            await _context.SaveChangesAsync();

            // updating view
            if (setToRemove.ExerciseId == null)
            {
                return;
            }

            if (exerciseOfSet == null)
            {
                return;
            }
            exerciseOfSet.Sets.Remove(setToRemove);
            exerciseOfSet.AmountOfSets = exerciseOfSetDb.AmountOfSets.ToString();

            foreach (var set in exerciseOfSetDb.Sets)
            {
                var currentSet = exerciseOfSet.Sets.FirstOrDefault(s => s.Id == set.Id);
                if (currentSet != null)
                {
                    currentSet.Index = set.Index;
                }
            }
            // TODO:
            // - popup: sicher dass du den Satz löschen willst?
        }
        #endregion
    }
}
