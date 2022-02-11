using System;
using System.Collections.Generic;
using Light;
using UnityEditor;
using UnityEngine;
using Unlockables;
using System.Linq;

namespace Activators
{
    public class ActivationManager : MonoBehaviour
    {
        public readonly Dictionary<LightColor, List<Receiver>> Receivers =
            new Dictionary<LightColor, List<Receiver>>();
        
        public readonly Dictionary<LightColor, List<Activator>> Activators = 
            new Dictionary<LightColor, List<Activator>>();

        public static readonly List<IInteractable> Interactables = new List<IInteractable>(); 

        public bool HideConnections;
        
        public Vector3 Activator
        {
            get => transform.position + new Vector3(-2, 0, 0);
        }
        public Vector3 Receiver
        {
            get => transform.position + new Vector3(2, 0, 0);
        }

        public void OnActivatorActivated(Activator activator)
        {
            SetReceiverIndicatorToFlash(activator, true);
            if (AllLightsOfColorIsActivated(activator.LightColor))
            {
                ActivateAllReceiversWithColor(activator.LightColor);
                PermanentlyActivatePermanentLights(activator.LightColor);
                UpdateAllUnlockables();
            }
        }

        private void SetReceiverIndicatorToFlash(Activator activator, bool flash)
        {
            for (int i = 0; i < Receivers[activator.LightColor].Count; i++)
            {
                LampListener lampListener = (LampListener) Receivers[activator.LightColor][i];
                if (lampListener != null)
                {
                    if (!lampListener.IsFlashing && flash)
                    {
                        lampListener.IsFlashing = flash;
                        lampListener.StartCoroutine(lampListener.Flash());
                        Debug.Log(lampListener.transform.name);
                    }
                    else
                    {
                        lampListener.IsFlashing = false;
                        lampListener.StopCoroutine(lampListener.Flash());
                    }
                }
            }
        }

        private void UpdateAllUnlockables()
        {
            List<IUnlockable> unlockables = UnlockableManager.Unlockables;
            for (int i = 0; i < unlockables.Count; i++)
            {
                unlockables[i].UpdateUnlockable();
            }
        }
        public void OnActivatorDeactivated(Activator activator)
        {
            SetReceiverIndicatorToFlash(activator, false);
            if (!Receivers.ContainsKey(activator.LightColor))
            {
                Debug.LogError($"There are no receivers/doors connected to a {activator.LightColor}");
            }
            else
            {
                List<Receiver> receivers = this.Receivers[activator.LightColor];
                for (int i = 0; i < receivers.Count; i++)
                {
                    receivers[i].Deactivate();
                }
                if (receivers.Where(each => each.GetComponent<LampListener>().IsActivatedPermanently).Count() != receivers.Count)
                    FG.AudioManager.Instance.Play("fail");
            }
        }
        private bool AllLightsOfColorIsActivated(LightColor lightColor)
        {
            List<Activator> activators = Activators[lightColor]; 
            
            for (int i = 0; i < activators.Count; i++)
            {
                if (!activators[i].IsActivated)
                {
                    return false;
                }
            }
            return true;
        }
        private void PermanentlyActivatePermanentLights(LightColor lightColor)
        {
            List<Activator> activators = Activators[lightColor]; 
            for (int i = 0; i < activators.Count; i++)
            {
                if (activators[i].permanentSwitch)
                {
                    activators[i].ActivatePermanently();
                }
            }
        }
        
        private void ActivateAllReceiversWithColor(LightColor lightColor)
        {
            List<Receiver> receivers = Receivers[lightColor];
            
            for (int i = 0; i < receivers.Count; i++)
            {
                receivers[i].OnReceiveActivation();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawIcon(transform.position, "circuit.png", true);
            Gizmos.DrawIcon(Activator, "switch.png", true);
            Gizmos.DrawIcon(Receiver, "door.png", true);
        }
    }
}
