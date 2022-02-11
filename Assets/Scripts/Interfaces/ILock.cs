using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FG
{
    public interface ILock
    {
        public abstract void Unlock();
        public abstract void Lock();
    }
}