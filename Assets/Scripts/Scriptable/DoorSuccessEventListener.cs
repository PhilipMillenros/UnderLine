using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace FG
{
    public class DoorSuccessEventListener : MonoBehaviour
    {
        [SerializeField] private DoorSuccessEvent thisEvent;
        
        private ILockBack unlocks;

        public void OnEventRaised(int id)
        {
            unlocks.DoorOpened();
        }

        private void Awake()
        {
            unlocks = GetComponent<ILockBack>();

            thisEvent = DoorSuccessEvent.Instance;
            thisEvent.Register(this);
        }

        private void OnApplicationQuit()
        {
            thisEvent.UnRegister(this);
        }
    }
}