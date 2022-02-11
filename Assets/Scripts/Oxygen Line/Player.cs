using System.Collections.Generic;
using Activators;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Oxygen_Line
{
	[SelectionBase]
	public class Player : MonoBehaviour
	{
		public static Player player;
        [SerializeField] private LayerMask layerMask;
        public Transform origin;
        private void Awake()
        {
            if (player == null)
            {
                player = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
  
        private void OnFire(InputValue input)
        {
            List<IInteractable> interactables = ActivationManager.Interactables;
            for (int i = 0; i < interactables.Count; i++)
            {
                if (InRangeOfInteractable(interactables[i]) && !PlayerIsActivatingThroughWall(interactables[i]))
                {
                    interactables[i].Press();
                }
            }
        }

        private bool InRangeOfInteractable(IInteractable interactable)
        {
            float squaredDistance = (interactable.Position - transform.position).sqrMagnitude;
            float squaredRange = interactable.Range * interactable.Range;
            return squaredDistance < squaredRange;
        }

        private bool PlayerIsActivatingThroughWall(IInteractable interactable)
        {
            Vector3 direction = transform.position - interactable.Position;
            RaycastHit hit;
            if (Physics.Raycast(interactable.Position, direction, out hit, 1000, layerMask))
            {
                if (hit.transform.TryGetComponent(out Player player))
                {
                    return false;
                }
            }
            return true;
        }
    }
}