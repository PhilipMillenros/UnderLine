using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FG
{
    public interface ILockBack
    {
        public abstract void DoorOpened();
        public abstract void Reset();
    }
}