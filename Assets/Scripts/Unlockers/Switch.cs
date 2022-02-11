using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace FG
{
    public class Switch : MonoBehaviour, ILockBack
    {
        [SerializeField] private UnlockEvent affectedEvent;
        [SerializeField] private float resetSeconds = 0;
        [SerializeField] private int[] id;
        [SerializeField] private bool keepKey = true;

        private Coroutine resetRoutine;
        private bool active = false;

        public void Activate()
        {
            if (!active)
            {
                active = true;

                if (resetSeconds != 0f)
                    resetRoutine = StartCoroutine(Reset());

                foreach (int each in id)
                    affectedEvent.Raise(each, true);
            }
        }

        public void DoorOpened()
        {
                if (resetRoutine != null && keepKey)
                    StopCoroutine(resetRoutine);
        }

        private IEnumerator Reset()
        {
            yield return new WaitForSeconds(resetSeconds);

            active = false;

            foreach (int each in id)
                affectedEvent.Raise(each, false);
        }

        private void Awake()
        {
            affectedEvent = UnlockEvent.Instance;
        }

        void ILockBack.Reset()
        {
            throw new System.NotImplementedException();
        }
    }
}