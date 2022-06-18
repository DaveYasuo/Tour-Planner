﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tour_Planner.DataModels.Enums;

namespace Tour_Planner.DataModels.Collections
{
    public class DifficultyTypes :ObservableCollection<string>
    {
        public DifficultyTypes()
        {
            foreach (string difficultyType in Enum.GetNames(typeof(DifficultyType)))
            {
                Add(difficultyType);
            }
        }
    }
}
