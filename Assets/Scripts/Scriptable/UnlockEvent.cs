using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace FG
{
    [CreateAssetMenu, Serializable]
    public class UnlockEvent : ScriptableObject
    {
        private static UnlockEvent instance;
        public static UnlockEvent Instance
        {
            get
            {
                if (instance == null)
                    instance = (UnlockEvent) CreateInstance(typeof(UnlockEvent).Name);

                return instance;
            }
        }

        private List<IListener> listeners = new List<IListener>();

        private UnlockEvent()
        {
            
        }

        public void Raise(int id, bool state)
        {
            foreach (IListener each in listeners)
                each.OnEventRaised(this, id, state);
        }

        public void Register(IListener listener)
        {
            if(!listeners.Contains(listener))
                listeners.Add(listener);
        }

        public void UnRegister(IListener listener)
        {
            if (listeners.Contains(listener))
                listeners.Remove(listener);
        }

        private void OnApplicationQuit()
        {
            listeners.Clear();
        }
    }
}