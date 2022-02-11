using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace FG
{
    [CreateAssetMenu, Serializable]
    public class DoorSuccessEvent : ScriptableObject
    {
        private static DoorSuccessEvent instance;
        public static DoorSuccessEvent Instance
        {
            get
            {
                if (instance == null)
                    instance = (DoorSuccessEvent) CreateInstance(typeof(DoorSuccessEvent).Name);

                return instance;
            }
        }

        private List<DoorSuccessEventListener> listeners = new List<DoorSuccessEventListener>();

        private DoorSuccessEvent()
        {

        }

        public void Raise(int id)
        {
            foreach (DoorSuccessEventListener each in listeners)
                each.OnEventRaised(id);
        }

        public void Register(DoorSuccessEventListener listener)
        {
            if (!listeners.Contains(listener))
                listeners.Add(listener);
        }

        public void UnRegister(DoorSuccessEventListener listener)
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