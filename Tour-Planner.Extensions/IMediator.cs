using System;
using Tour_Planner.DataModels.Enums;

namespace Tour_Planner.Extensions
{
    public interface IMediator
    {
        public void Subscribe(Action<object?> callback, ViewModelMessage message);
        public void Publish(ViewModelMessage message, object? args);
    }
}
