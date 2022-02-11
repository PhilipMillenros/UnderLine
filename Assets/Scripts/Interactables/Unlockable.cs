using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FG
{
    public class Unlockable : MonoBehaviour, ILock
    {
        [SerializeField] private bool glitchy = false;
        [SerializeField] private float cycleSeconds = 5f;

        private Vector3 startPos;

        public void Lock()
        {
            gameObject.SetActive(true);
            Relock();
        }

        public void Unlock()
        {
            if (glitchy)
                StartCoroutine(GlitchLoop(transform.position, transform.position + new Vector3(0f, transform.localScale.y / 2f, 0f)));
            else
                gameObject.SetActive(false);
        }

        private void Relock()
        {
            StopAllCoroutines();
            transform.position = startPos;
        }

        private IEnumerator GlitchLoop(Vector3 startPos, Vector3 target)
        {
            float time = 0;
            while(time < cycleSeconds)
            {
                if (!glitchy)
                    break;

                transform.position = Vector3.Lerp(startPos, target, time / cycleSeconds);
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            transform.position = target;
            StartCoroutine(GlitchLoop(target, startPos));
        }

        private void Awake()
        {
            startPos = transform.position;
        }
    }
}