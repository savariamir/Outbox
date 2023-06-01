using System;

namespace Anshan.Core
{
    public class Clock : IClock
    {
        public DateTime Now()
        {
            return DateTime.UtcNow;
        }
    }
}