using System;
using System.Collections;
using FG;
using UnityEngine;

namespace Oxygen_Line
{
    public class CollisionDetection : MonoBehaviour
    {
        private Retraction retraction;
        private RaycastHit objectHit;
        [SerializeField] private float stunDuration;
        [SerializeField] private ParticleSystem[] StunEffects;
        private FG.Input input;

        private void Awake()
        {
            retraction = GetComponent<Retraction>();
            input = GetComponent<FG.Input>();
            SetStunEffect(false);
        }
        
        private void OnCollisionEnter(Collision other)
        {
            if (StunConditions(other))
            {
                StunPlayer();
            }
        }

        private bool StunConditions(Collision other)
        {
            if (other.transform.CompareTag("Glitchy Door"))
            {
                GlitchyDoor glitchyDoor = other.transform.GetComponentInParent<GlitchyDoor>();
                return  retraction.IsRetracting && !retraction.IsStunned &&
                        !glitchyDoor.Open && glitchyDoor.IsActive;
            }
            return false;
        }
        private void StunPlayer()
        {
            FG.AudioManager.Instance.Play("GlitchStop");
            retraction.IsStunned = true;
            StartCoroutine(StunDuration());
            SetStunEffect(true);

            input.enabled = false;
        }

        private IEnumerator StunDuration()
        {
            yield return new WaitForSeconds(stunDuration);
            retraction.IsStunned = false;
            SetStunEffect(false);
            input.enabled = true;
        }

        private void SetStunEffect(bool stun)
        {
            if (StunEffects != null)
            {
                for (int i = 0; i < StunEffects.Length; i++)
                {
                    if(stun) StunEffects[i].Play();
                    else StunEffects[i].Stop();
                }
            }
        }
    }
}
