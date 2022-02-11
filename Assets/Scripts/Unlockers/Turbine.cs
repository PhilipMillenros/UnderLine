using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Oxygen_Line;

namespace FG
{
	[SelectionBase]
	public class Turbine : Activators.Activator, ILockBack
	{
		[SerializeField] private bool spinRight = true;
		[SerializeField] private float senseRange = 1f;
		[SerializeField] private bool speedThreshold = false;

		private LayerMask playerMask;
		private bool playerRight = false;
		private bool spinning = false;

		private void Update()
		{
			Collider[] near = new Collider[0];
			near = Physics.OverlapSphere(transform.position, senseRange, playerMask);

			if (near.Length > 0)
			{
				Vector3 playerPos = near.First(any => any != null).transform.position;
				Vector3 playerDir = -near.First(any => any != null).transform.forward;

				playerDir = playerDir.normalized;

				Vector3 pos = transform.position;

				if (playerDir == Vector3.zero || spinning || !near.First(any => any != null).GetComponent<Input>().Pressing)
					return;

				if (near.First(any => any != null).GetComponent<Input>().Pressing != speedThreshold)
					return;

				if (spinRight)
				{
					if (playerPos.x > pos.x && playerPos.z > pos.z)
						if (playerDir.z < 0 && playerDir.x >= 0)
							playerRight = true;
						else
							playerRight = false;
					if (playerPos.x > pos.x && playerPos.z < pos.z)
						if (playerDir.x < 0 && playerDir.z <= 0)
							playerRight = true;
						else
							playerRight = false;
					if (playerPos.x < pos.x && playerPos.z < pos.z)
						if (playerDir.z > 0 && playerDir.x <= 0)
							playerRight = true;
						else
							playerRight = false;
					if (playerPos.x < pos.x && playerPos.z > pos.z)
						if (playerDir.x > 0 && playerDir.z >= 0)
							playerRight = true;
						else
							playerRight = false;
				}
				else
				{
					if (playerPos.x > pos.x && playerPos.z > pos.z)
						if (playerDir.x < 0 && playerDir.z >= 0)
							playerRight = true;
						else
							playerRight = false;
					if (playerPos.x > pos.x && playerPos.z < pos.z)
						if (playerDir.z > 0 && playerDir.x >= 0)
							playerRight = true;
						else
							playerRight = false;
					if (playerPos.x < pos.x && playerPos.z < pos.z)
						if (playerDir.x > 0 && playerDir.z <= 0)
							playerRight = true;
						else
							playerRight = false;
					if (playerPos.x < pos.x && playerPos.z > pos.z)
						if (playerDir.z < 0 && playerDir.x <= 0)
							playerRight = true;
						else
							playerRight = false;
				}

				if (playerRight)
				{
					spinning = true;
					StartCoroutine(Spin());

					Activate();
				}
			}
		}

		public void DoorOpened()
		{
			if (!permanentSwitch)
			{
				spinning = false;
			}
		}

		public void Reset()
		{
			if (!permanentSwitch)
				spinning = false;
		}

		private IEnumerator Spin()
		{
			AudioManager.Instance.Play("Turbine");
			while (spinning)
			{
				transform.RotateAround(transform.position, Vector3.up, spinRight ? 1f : -1f);
				yield return new WaitForEndOfFrame();
			}
			AudioManager.Instance.Stop("Turbine");
		}

		private new void Awake()
		{
			base.Awake();
			playerMask = LayerMask.GetMask("Player");
		}

		/*private new void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            if (senseVisualization)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(transform.position, senseRange);
            }
        }*/
	}
}