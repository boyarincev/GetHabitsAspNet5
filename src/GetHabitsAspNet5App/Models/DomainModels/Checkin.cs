using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GetHabitsAspNet5App.Models.DomainModels
{
    public class Checkin
    {
        public Int64 Id { get; set; }
        public bool Done { get; set; }
        [Required]
        public DateTime Date { get; set; }
    }
}
