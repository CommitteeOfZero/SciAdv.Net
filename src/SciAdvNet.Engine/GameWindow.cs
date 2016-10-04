using System;

namespace SciAdvNet.Engine
{
    public sealed class GameWindow
    {
        public GameWindow(object windowObject, Action processEventsFunc)
        {
            WindowObject = windowObject;
            ProcessEventsFunc = processEventsFunc;
        }

        public object WindowObject { get; }
        public Action ProcessEventsFunc { get; }
    }
}
