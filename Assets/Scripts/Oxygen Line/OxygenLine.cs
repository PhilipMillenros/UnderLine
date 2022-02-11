using System;
using System.Collections.Generic;
using Oxygen_Line;
using UnityEngine;


namespace Oxygen_Path
{
	[SelectionBase]
	public class OxygenLine : MonoBehaviour
	{
		[SerializeField] private float distancePerPoint;

		public float DistancePerPoint
		{
			get => distancePerPoint;
		}

		[HideInInspector] public bool IsConnectedToPlayer;
		[HideInInspector] public bool ReachedMaximumLength;

		public int MaxLength;
		public static readonly List<OxygenLine> OxygenLines = new List<OxygenLine>();
		public readonly List<Point> Points = new List<Point>();
		public ConnectionPoint ConnectionPoint;
		public Point LastPoint;

		private Transform player;
		private Retraction retraction;
		public bool IntroOxygenLine;

		public int CurrentLength { get => Points.Count - 1; }
		private void Start()
		{
			for (int i = 0; i < Points.Count; i++)
			{
				Points[i].Position.y = Player.player.transform.position.y;
			}

			if (Points.Count > 1)
			{
				ConnectionPoint.transform.position = Points[Points.Count - 1].Position;
				ConnectionPoint.transform.rotation = Points[Points.Count - 1].Rotation;
			}

			UpdateLastPoint();
		}
		
		private void Update()
		{
			TryAddingPoint();
		}
		private void TryAddingPoint()
		{
			if (AddPointConditions())
			{
				AddPoint(player);
			}
		}
		private bool AddPointConditions()
		{
			return !ReachedMaxLength() && IsConnectedToPlayer && ReachedDistanceThreshold() && !retraction.IsRetracting;
		}
		private void AddPoint(Transform pointTransform)
		{
			Point point = new Point(new Vector3(pointTransform.position.x,
				Player.player.origin.transform.position.y, pointTransform.transform.position.z), pointTransform.rotation);
			Points.Add(point);
			UpdateLastPoint();
		}
		private bool ReachedMaxLength()
		{
			ReachedMaximumLength = MaxLength < Points.Count;
			return ReachedMaximumLength;
		}

		private bool ReachedDistanceThreshold()
		{
			if (LastPoint == null)
			{
				return false;
			}
			Vector3 offset = LastPoint.Position - player.position;
			float squaredLength = offset.sqrMagnitude;
			return squaredLength > distancePerPoint * distancePerPoint;
		}
		public void RemovePointAt(int index)
		{
			Points.RemoveAt(index);
			UpdateLastPoint();
			ConnectionPoint.transform.position = LastPoint.Position;
			ConnectionPoint.transform.rotation = LastPoint.Rotation;
		}
		private void UpdateLastPoint()
		{
			if (Points.Count > 0)
			{
				int lastIndex = Points.Count - 1;
				LastPoint = Points[lastIndex];
				ConnectionPoint.transform.position = LastPoint.Position;
				ConnectionPoint.transform.rotation = LastPoint.Rotation;
			}
		}
		public void Connect(Transform player)
		{
			this.player = player;
			IsConnectedToPlayer = true;
			retraction = player.GetComponent<Retraction>();
		}
		public void Disconnect()
		{
			player = null;
			IsConnectedToPlayer = false;
			retraction = null;
			ConnectionPoint.Disconnect();
		}
		private void OnEnable()
		{
			OxygenLines.Add(this);
		}

		private void OnDisable()
		{
			OxygenLines.Remove(this);
		}
	}
}
