using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace GetHabitsAspNet5App.Models.DomainModels
{
    public class Habit
    {
        public Int64 Id { get; set; }
        [JsonIgnoreAttribute]
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
