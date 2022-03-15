using System;

namespace Events.Args
{
    public class PauseArgs : EventArgs
    {
        public bool IsPauseActive { get; set; }
    }
}