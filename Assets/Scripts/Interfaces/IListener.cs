using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FG
{
    public interface IListener
    {
        public abstract void OnEventRaised(UnlockEvent thatEvent, int id, bool state);
    }
}