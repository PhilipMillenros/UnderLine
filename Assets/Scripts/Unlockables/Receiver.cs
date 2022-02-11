
using System;
using System.Collections.Generic;
using Activators;
using Light;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Unlockables
{
    public abstract class Receiver : MonoBehaviour
    {
        public ActivationManager activationManager;
        public LightColor LightColor;
        public abstract void OnReceiveActivation();
        [SerializeField] bool manuallyAssignActivationManager;
        protected void OnEnable()
        {
            AddReceiver();
        }
        private void OnValidate()
        {
            if (!manuallyAssignActivationManager)
            {
                activationManager = FindObjectOfType<ActivationManager>();
            }
        }
        private void AddReceiver()
        {
            if (!activationManager.Receivers.ContainsKey(LightColor))
            {
                activationManager.Receivers.Add(LightColor, new List<Receiver>());
            }
            activationManager.Receivers[LightColor].Add(this);
        }

        public abstract void Deactivate();
        protected void OnDisable()
        {
            RemoveReceiver();
        }
        
        private void RemoveReceiver()
        {
            if (activationManager.Receivers.ContainsKey(LightColor))
            {
                activationManager.Receivers[LightColor].Remove(this);
            }
        }

    }
}
