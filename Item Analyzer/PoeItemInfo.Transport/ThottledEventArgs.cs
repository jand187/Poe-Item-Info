using System;
using System.Linq;

namespace PoeItemInfo.Transport
{
    public class ThottledEventArgs : EventArgs
    {
        public TimeSpan WaitTime { get; private set; }
        public ThottledEventArgs(TimeSpan waitTime)
        {
            this.WaitTime = waitTime;
        }
    }
}
