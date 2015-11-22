using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GetHabitsAspNet5App.Models.DomainModels
{
    public class Checkin
    {
        [JsonIgnoreAttribute]
        public Int64 Id { get; set; }
        public Int64? HabitId { get; set; }
        public CheckinState State { get; set; }
        //TODO need Index by this property
        [Required]
        public DateTime Date { get; set; }
    }

    public enum CheckinState
    {
        NotSet, Done, NotDone
    }
}
