
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Activators
{
	[SelectionBase]
	public class LightSwitch : Activator, IInteractable
    {
		[SerializeField] private float range;
		public float Range { get => range; set => range = value; }
		public Vector3 Position { get => transform.position; }

		public UnityEvent OnDeactivateSwitch;

		private bool leverIsUp = true;
		
		public void Press()
		{
			if (!IsPermanentlyActivated && !IsActivated)
			{
				FG.AudioManager.Instance.Play("Lever down");
				leverIsUp = false;
			}
			Activate();
		}
		private new void OnEnable()
		{
			base.OnEnable();
			ActivationManager.Interactables.Add(this);
		}
		private new void OnDisable()
		{
			base.OnDisable();
			ActivationManager.Interactables.Remove(this);
		}
		protected override void Deactivate()
		{
			base.Deactivate();
			if (!IsPermanentlyActivated && !leverIsUp)
			{
				FG.AudioManager.Instance.Play("Lever up");
				OnDeactivateSwitch.Invoke();
				Debug.Log("Played");
				leverIsUp = true;
			}
		}

		public override void ActivatePermanently()
		{
			base.ActivatePermanently();
			
			if (AllLeversArePermanentlyActivated())
			{
				FG.AudioManager.Instance.Play("Lever up");
				leverIsUp = true;
				OnDeactivateSwitch.Invoke();
			}
		}

		public bool AllLeversArePermanentlyActivated()
		{
			LightSwitch[] lightSwitches = GetComponents<LightSwitch>();

			for (int i = 0; i < lightSwitches.Length; i++)
			{
				if (!lightSwitches[i].IsPermanentlyActivated)
				{
					return false;
				}
			}
			return true;
		}

#if UNITY_EDITOR
		private void OnDrawGizmosSelected()
		{
			DrawClickRange();
		}

		private void DrawClickRange()
		{
			if (LightColor != null)
			{
				Handles.color = LightColor.onColor;
				Handles.DrawWireDisc(transform.position, transform.up, range);
			}
		}
#endif
    }
}
