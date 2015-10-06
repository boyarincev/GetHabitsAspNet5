using GetHabitsAspNet5App.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GetHabitsAspNet5App.Services
{
    public class HabitService
    {
        private GetHabitsContext _dbContext;

        public HabitService(GetHabitsContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Get all Habit entities
        /// </summary>
        /// <returns>querying Habit collection</returns>
        public IEnumerable<Habit> Get()
        {
            return _dbContext.Habits;
        }

        /// <summary>
        /// Create new Habit
        /// </summary>
        /// <param name="habit">Habit for saving, Id have to be equal 0</param>
        /// <returns>Habit if creating is happened and null if not</returns>
        public async Task<Habit> Create(Habit habit)
        {
            //we create new entity if only Id equal 0
            if (habit.Id != 0)
                return null;

            //Checkins we save separately from Habit
            habit.Checkins = null;
            _dbContext.Habits.Add(habit);
            await _dbContext.SaveChangesAsync();
            return habit;
        }

        /// <summary>
        /// Edit Habit entity
        /// </summary>
        /// <param name="habit">Habit with changed fields</param>
        /// <returns>Changed Habit if editing is happened and null if not</returns>
        public async Task<Habit> Edit(Habit habit)
        {
            var original = _dbContext.Habits.FirstOrDefault(h => h.Id == habit.Id);

            if (original == null)
                return null;

            original.Name = habit.Name;
            await _dbContext.SaveChangesAsync();

            return original;
        }

        /// <summary>
        /// Delete Habit entity
        /// </summary>
        /// <param name="habit">Habit entity for deleting</param>
        /// <returns></returns>
        public async Task<Boolean> Delete(Int64 Id)
        {
            var original = _dbContext.Habits.FirstOrDefault(h => h.Id == Id);

            if (original == null)
                return false;

            _dbContext.Remove(original);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
