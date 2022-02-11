using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FG
{
    public class Lamp : MonoBehaviour
    {
        [SerializeField] private UnlockEvent listenEvent;
        [SerializeField] private Material lockMaterial;
        [SerializeField] private Material interMaterial;
        [SerializeField] private Material unlockMaterial;

        private Renderer ren;

        public void Light(int on)
        {
            if (on > 0)
                ren.material = unlockMaterial;
            else if (on < 0)
                ren.material = lockMaterial;
            else
                ren.material = interMaterial;
        }

        private void Awake()
        {
            ren = GetComponent<Renderer>();
        }
    }
}