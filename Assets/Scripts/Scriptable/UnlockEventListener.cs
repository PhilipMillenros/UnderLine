using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace FG
{
    public class UnlockEventListener : MonoBehaviour, IListener
    {
        [SerializeField] private UnlockEvent thisEvent;
        [SerializeField] private int[] interactablesPerSet;
        [SerializeField] private int[] setId;
        [SerializeField] private DoorSuccessEvent successEvent;

        private int[] setCounter;
        private ILock unlocks;
        private bool done = false;

        public void OnEventRaised(UnlockEvent thatEvent, int id, bool state)
        {
            if (setId.Any(any => any == id))
            {
                if (state)
                    setCounter[setId.Select((any, index) => (id, index)).First(any => any.id == id).index]++;
                else
                {
                    setCounter[setId.Select((any, index) => (id, index)).First(any => any.id == id).index]--;

                    if (done)
                        unlocks.Lock();
                }

                done = true;
                for (int c = 0; c < interactablesPerSet.Length; c++)
                    if (setCounter[c] != interactablesPerSet[c])
                    {
                        done = false;
                        break;
                    }

                if (done)
                {
                    unlocks.Unlock();
                    if (successEvent != null)
                        successEvent.Raise(id);
                }
            }
        }

        private void Awake()
        {
            setCounter = new int[interactablesPerSet.Length];
            unlocks = GetComponent<ILock>();

            thisEvent = UnlockEvent.Instance;
            thisEvent.Register(this);

            successEvent = DoorSuccessEvent.Instance;
        }

        private void OnApplicationQuit()
        {
            thisEvent.UnRegister(this);
        }
    }
}