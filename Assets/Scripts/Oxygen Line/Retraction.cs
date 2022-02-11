using System;
using Oxygen_Path;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Oxygen_Line
{
    public class Retraction : MonoBehaviour
    {
        [SerializeField] private float speedMultiplier;
        [SerializeField] private float RotationSpeed;
        [HideInInspector] public bool IsRetracting;
        [HideInInspector] public bool IsStunned;
        [HideInInspector] public OxygenLine OxygenLine;
        [SerializeField] private ParticleSystem[] retractionParticles;
        private bool isConnectedToOxygen = true;
        private float timeRetracted;
        public AnimationCurve retractingSpeedGraph;
        private bool reachedStart;
        private FG.Input input;
        private bool isStopped = true;

        private void Start()
        {
            SetParticleEffects(false);
        }

        private void Update()
        {
            TryRetracting();
        }
        private void TryRetracting()
        {
            if (RetractingConditions())
            {
                TryRemovingLastPoint();
                Retract();
                timeRetracted += Time.deltaTime;
                IsRetracting = true;
                isStopped = false;
                SetParticleEffects(true);
            }
            else if(!isStopped)
            {
                timeRetracted = 0;
                IsRetracting = false;
                input.TryRetracting(false);
                isStopped = true;
                SetParticleEffects(false);
            }
        }
        public bool RetractingConditions()
        {
            return input.Pressing && isConnectedToOxygen && !IsStunned && !ReachedStart() && !OxygenLine.IntroOxygenLine;
        }
        private void Retract()
        {
            Point point = GetTargetPoint();
            RetractToPosition(new Vector3( point.Position.x, transform.position.y, point.Position.z));
            RetractToRotation(point.Rotation);
        }

        private bool ReachedStart()
        {
            if (GetTargetPoint() == OxygenLine.ConnectionPoint.StartPoint && ReachedPoint(OxygenLine.ConnectionPoint.StartPoint.Position))
            {
                reachedStart = true;
            }
            else
            {
                reachedStart = false;
            }
            return reachedStart;
        }

        private void SetParticleEffects(bool play)
        {
            for (int i = 0; i < retractionParticles.Length; i++)
            {
                if (play)
                {
                    if (!retractionParticles[i].isPlaying)
                    {
                        retractionParticles[i].time = 0;
                        retractionParticles[i].Play();
                    }
                }
                else
                {
                    retractionParticles[i].Stop();
                }
            }
        }
        private Point GetTargetPoint()
        {
            if (OxygenLine.Points.Count > 0)
            {
                int lastPoint = OxygenLine.Points.Count - 1;
                return OxygenLine.Points[lastPoint];
            }
            return OxygenLine.ConnectionPoint.StartPoint;
        }
        private void TryRemovingLastPoint()
        {
            if (OxygenLine.Points.Count < 1)
            {
                return;
            }
            int lastIndex = OxygenLine.Points.Count - 1;
            if (ReachedPoint(OxygenLine.Points[lastIndex].Position))
            {
                OxygenLine.RemovePointAt(lastIndex);
            }
        }
        private bool ReachedPoint(Vector3 point)
        {
            Vector3 offset = new Vector3(point.x, transform.position.y, point.z) - transform.position;
            float squaredLength = offset.sqrMagnitude;
            float precision = 0.001f;
            return squaredLength < precision;
        }
        private void RetractToPosition(Vector3 targetPosition)
        {
            float easingFunction = retractingSpeedGraph.Evaluate(timeRetracted);
            float speed = speedMultiplier * easingFunction * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed);
        }
        private void RetractToRotation(Quaternion targetRotation)
        {
            float easingFunction = retractingSpeedGraph.Evaluate(timeRetracted);
            float speed = RotationSpeed * easingFunction * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed);
        }

        private void Awake()
        {
            input = GetComponent<FG.Input>();
        }
    }
}
