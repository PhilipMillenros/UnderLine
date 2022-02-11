using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace FG
{
    public class StartPos : MonoBehaviour
    {
        [SerializeField] private float cooldown = 0f;
        [SerializeField] private float oldRetraceSeconds = 5f;

        private Animator animator;
        private NavMeshAgent agent;
        private Coroutine retraceRoutine;
        private SphereCollider sphereCollider;
        private Vector3 startPos;
        public Vector3 StartPosition
        {
            get
            {
                return startPos;
            }
        }

        private bool isRetracing = false;
        public bool Retracing
        {
            set
            {
                isRetracing = value;
                if(isRetracing)
                    Retrace();
            }
        }

        public void KeyActivated()
        {
            sphereCollider.enabled = false;
            animator.SetBool("isElectrified", true);
        }

        public void PickUp(bool state)
        {
            if (state)
            {
                agent.ResetPath();
                agent.enabled = false;
            }
            else
            {
                agent.enabled = true;
                agent.destination = startPos;
            }
        }

        private void Retrace()
        {
            if (agent != null)
            {
                sphereCollider.enabled = true;
                animator.SetBool("isElectrified", false);
                PickUp(false);
            }
            else
                retraceRoutine = StartCoroutine(OldRetrace());
        }

        private IEnumerator OldRetrace()
        {
            Vector3 pos = transform.position;
            float time = 0;
            while (time < oldRetraceSeconds)
            {
                transform.position = Vector3.Lerp(pos, startPos, time / oldRetraceSeconds);
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            transform.position = startPos;
            isRetracing = false;
        }

        private IEnumerator RetraceCooldown()
        {
            yield return new WaitForSeconds(cooldown);

            Retrace();
        }

        private void Update()
        {
            if (transform.parent == null && transform.position != startPos && cooldown != 0f)
            {
                retraceRoutine = StartCoroutine(RetraceCooldown());
                isRetracing = true;
            }

            if(transform.parent != null && isRetracing)
            {
                StopAllCoroutines();
                isRetracing = false;
            }
            animator.SetBool("isSwimming", isRetracing);
        }

        private void Awake()
        {
            startPos = transform.position;
            animator = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
            sphereCollider = GetComponent<SphereCollider>();
        }
    }
}