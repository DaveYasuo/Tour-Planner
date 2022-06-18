using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tour_Planner.DataModels.Enums;

namespace Tour_Planner.Extensions
{
    public interface IMediator
    {
        public void Subscribe(Action<object?> callback, ViewModelMessage message);
        public void Publish(ViewModelMessage message, object? args);
    }
}
