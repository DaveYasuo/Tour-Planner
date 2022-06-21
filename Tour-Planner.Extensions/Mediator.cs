using System;
using Tour_Planner.DataModels.Enums;

namespace Tour_Planner.Extensions
{
    public sealed class Mediator : IMediator
    {
        private readonly MultiDictionary<ViewModelMessage, Action<object?>> _internalList = new();

        public void Subscribe(Action<object?> callback, ViewModelMessage message)
        {
            _internalList.AddValue(message, callback);
        }


        public void Publish(ViewModelMessage message, object? args)
        {
            if (!_internalList.ContainsKey(message)) return;
            //forward the message to all listeners
            foreach (Action<object?> callback in
                _internalList[message])
                callback(args);
        }
    }
}
