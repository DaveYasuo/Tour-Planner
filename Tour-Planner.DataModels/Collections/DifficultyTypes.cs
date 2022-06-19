using System;
using System.Collections.ObjectModel;
using Tour_Planner.DataModels.Enums;

namespace Tour_Planner.DataModels.Collections
{
    public class DifficultyTypes : ObservableCollection<Difficulty>
    {
        public DifficultyTypes()
        {
            foreach (Difficulty difficultyType in Enum.GetValues(typeof(Difficulty)))
            {
                Add(difficultyType);
            }
        }
    }
}
