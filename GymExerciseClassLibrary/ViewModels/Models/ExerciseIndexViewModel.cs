using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymExerciseClassLibrary.ViewModels.Models
{
    public partial class ExerciseIndexViewModel : ObservableValidator
    {
        // model properties
        [ObservableProperty]
        private int _id;
        [ObservableProperty]
        private int _index;
        [ObservableProperty]
        private int _exerciseId;
        [ObservableProperty]
        private int _trainingId;
    }
}
