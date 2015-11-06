using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GetHabitsAspNet5App.Models.DomainModels
{
    public class Habit
    {
        private string _userId;

        public Int64 Id { get; set; }
        public string UserId { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<Checkin> Checkins { get; set; }

        public Habit(string name, string userId)
        {
            Name = name;
            UserId = userId;
        }

        public Habit()
        {

        }
    }
}
