using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using StriveSteady.Enums;

namespace StriveSteady.Models
{
    public class Goal
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description {get; set;}
        public ImportanceType ImportanceType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public GoalType GoalType { get; set; }
        public List<Subtask> Subtasks { get; set; }
        public bool IsChecked;
    }
}