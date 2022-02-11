

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Unlockables;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Light
{
    public class LampListener : Receiver
    {
        [HideInInspector] public bool IsActivated;
        [SerializeField] private UnityEvent OnActivatedEvent;
        [SerializeField] private Indicator indicator;
        public bool IsActivatedPermanently;

        [HideInInspector] public bool IsFlashing;
        
        private void Start()
        {
            indicator.SetLight(false, LightColor);
            indicator.lightColor = LightColor;
        }

        public override void OnReceiveActivation()
        {
            OnActivatedEvent.Invoke();
            IsActivated = true;
            SetLight(true);
        }

        public IEnumerator Flash()
        {
            indicator.SetLight(true, LightColor);
            
            if (IsActivatedPermanently)
            {
                yield break;
            }

            yield return new WaitForSeconds(0.5f / indicator.flashesPerSecond);
            if (IsActivatedPermanently)
            {
                yield break;
            }
            indicator.SetLight(false, LightColor);
            if (IsFlashing)
            {
                yield return new WaitForSeconds(0.5f / indicator.flashesPerSecond);
                StartCoroutine(Flash());
            }
        }
        public override void Deactivate()
        {
            if(IsActivatedPermanently)
                return;
            IsActivated = false;
            SetLight(false);
        }

        public void DeactivateIndicator()
        {
            SetLight(false);
        }
        private void SetLight(bool lightOn)
        {
            indicator.SetLight(lightOn, LightColor);
        }
        public void ActivateLightPermanently()
        {
            IsActivated = true;
            IsActivatedPermanently = true;
            SetLight(true);

        }
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (activationManager != null && !activationManager.HideConnections && LightColor != null)
            {
                DrawConnectionToPosition(activationManager.Receiver);
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