
using System;
using System.Collections;
using System.Collections.Generic;
using FG;
using Light;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.GlobalIllumination;


namespace Activators
{
    public abstract class Activator : MonoBehaviour
    {
        [SerializeField] protected ActivationManager activationManager;
        public LightColor LightColor;
        public bool permanentSwitch;
        [SerializeField] private float timeActivated;
        [HideInInspector] public bool IsActivated;
        [SerializeField] private UnityEvent OnActivateEvent;
        [HideInInspector] public bool IsPermanentlyActivated;
        [SerializeField] private Indicator indicator;

        private Vector3 offset = new Vector3(0, 5, 0);
        
        private void OnValidate()
        {
            activationManager = FindObjectOfType<ActivationManager>();
        }
        protected void Awake()
        {
            indicator.SetLight(false, LightColor);
            indicator.lightColor = LightColor;
        }

        protected virtual void Activate()
        {
            if (IsPermanentlyActivated || IsActivated)
                return;

            FG.AudioManager.Instance.PlayAccumulated("Timer");
            OnActivateEvent.Invoke();
            IsActivated = true;
            StartCoroutine(StartTimer());
            activationManager.OnActivatorActivated(this);
            StartCoroutine(Flash());
        }

        protected virtual void Deactivate()
        {
            if (IsPermanentlyActivated || permanentSwitch)
                return;

            FG.AudioManager.Instance.StopAccumulated("Timer");
            indicator.SetLight(false, LightColor);
            IsActivated = false;
            activationManager.OnActivatorDeactivated(this);
            if (TryGetComponent(out ILockBack lockBack))
            {
                lockBack.Reset();
            }
        }

        private IEnumerator StartTimer()
        {
            yield return new WaitForSeconds(timeActivated);
            Deactivate();
        }

        private IEnumerator Flash()
        {
            indicator.SetLight(true, LightColor);
            
            if (IsPermanentlyActivated)
            {
                yield break;
            }

            yield return new WaitForSeconds(0.5f / indicator.flashesPerSecond);
            if (IsPermanentlyActivated)
            {
                yield break;
            }
            indicator.SetLight(false, LightColor);
            if (IsActivated)
            {
                yield return new WaitForSeconds(0.5f / indicator.flashesPerSecond);
                StartCoroutine(Flash());
            }
        }

        public virtual void ActivatePermanently()
        {
            StopCoroutine(StartTimer());
            IsActivated = true;
            IsPermanentlyActivated = true;

        }

        private void TurnOnIndicator()
        {
            indicator.SetLight(true, LightColor);
        }

        private void TurnOffIndicator()
        {
            indicator.SetLight(false, LightColor);
        }
        protected void OnEnable()
        {
            AddActivator();
        }

        protected void AddActivator()
        {
            if (!activationManager.Activators.ContainsKey(LightColor))
            {
                activationManager.Activators.Add(LightColor, new List<Activator>());
            }

            activationManager.Activators[LightColor].Add(this);
        }

        protected void OnDisable()
        {
            RemoveActivator();
        }

        protected void RemoveActivator()
        {
            if (activationManager.Activators.ContainsKey(LightColor))
            {
                activationManager.Activators[LightColor].Remove(this);
            }
        }
#if UNITY_EDITOR
        protected void OnDrawGizmos()
        {
            Gizmos.DrawIcon(transform.position + offset, "switch.png", true);
            
            if (activationManager != null && !activationManager.HideConnections && LightColor != null)
            {
                Gizmos.color = LightColor.onColor;
                DrawConnectionToPosition(activationManager.Activator);
            }
        }
        private void DrawConnectionToPosition(Vector3 targetPosition)
        {
            
                float halfHeight = (targetPosition.y - indicator.transform.position.y) * 0.5f;
                Vector3 offset = Vector3.up * halfHeight;
                Handles.DrawBezier(
                    targetPosition,
                    indicator.transform.position,
                    targetPosition - offset,
                    indicator.transform.position + offset,
                    LightColor.onColor, EditorGUIUtility.whiteTexture,
                    1f);
            
        }
        
#endif
    }
}
