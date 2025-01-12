using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymExerciseClassLibrary.Models
{
    public class Musclegroup
    {
        int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
