using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace FG
{
    public class Keyreceiver : Activators.Activator, ILockBack
    {
        [SerializeField] private Transform keyhole;

        private GameObject key;

        public bool Insert(GameObject key)
        {
            if (this.key == null)
            {
                this.key = key;
                this.key.transform.parent = transform;
                this.key.transform.position = keyhole.position;
                this.key.layer = (int) Mathf.Log(LayerMask.GetMask("Default"), 2);
                
                this.key.GetComponent<StartPos>().KeyActivated();

                Activate();

                return true;
            }
            else
                return false;
        }

        public void DoorOpened()
        {
                
        }

        public void Reset()
        {
            if (!permanentSwitch)
            {
                this.key.layer = (int)Mathf.Log(LayerMask.GetMask("Pickup"), 2);
                key.transform.parent = null;
                key.GetComponent<StartPos>().Retracing = true;
                key = null;
            }
        }
    }
}