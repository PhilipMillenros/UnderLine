using UnityEngine;

namespace Oxygen_Line
{
	public class OxygenLineCreator : MonoBehaviour
	{
	
		[SerializeField, HideInInspector] public OxygenLinePath OxygenLinePath;

		public void CreateNewPath()
		{
			OxygenLinePath = new OxygenLinePath(transform.position);
			OxygenLinePath.Points.Add(transform.position);
		}
	}
}