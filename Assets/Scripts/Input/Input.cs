using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Linq;
using Oxygen_Line;
using Unity.Mathematics;
using UnityEngine.SceneManagement;

namespace FG
{

	public class Input : MonoBehaviour
	{
		[SerializeField] private float speed = 1f;
		[SerializeField] private bool enabledTurnSpeed = true;
		[SerializeField] [Tooltip("Higher number gives tighter turn radius")] private float turnSpeed = 1f;
		//private float rotSpeed = 1f;
		[SerializeField] private float gravityScale = 1f;
		[SerializeField] private float pickupRange = 1f;
		[SerializeField] private bool senseVisualization = false;
		[SerializeField] private Transform hand;
		[SerializeField] private bool introControls = false;
		[SerializeField] private bool outro = false;

		public Transform Hand
		{
			get { return hand; }
		}

		public float PickupRange
		{
			get { return pickupRange; }
		}

		//private PlayerMovement playerMovement;

		private bool isPressing = false;
		public bool Pressing
		{
			get
			{
				return isPressing;
			}
		}

		//private float rot = 0f;
		private Vector2 dir = new Vector2();
		private Rigidbody body;
		private Retraction retraction;
		private LayerMask interactionMask;
		private LayerMask pickupMask;
		private LayerMask receiverMask;
		private LayerMask switchMask;
		private GameObject currentPickup;
		private bool steps = false;
		private bool maxSound = false;

		public Animator animator;

		private PlayerTriggerButtonPrompt playerBtnPrmpt;

		public GameObject CurrentPickup
		{
			get { return currentPickup; }
		}

		//private Transform rotatablesTransf;

		private void Awake()
		{
			body = gameObject.GetComponent<Rigidbody>();

			retraction = GetComponent<Retraction>();
			interactionMask = LayerMask.GetMask("Pickup", "Receiver", "Switch");
			pickupMask = LayerMask.GetMask("Pickup");
			receiverMask = LayerMask.GetMask("Receiver");
			switchMask = LayerMask.GetMask("Switch");

			playerBtnPrmpt = GetComponent<PlayerTriggerButtonPrompt>();

			//playerMovement = GetComponent<PlayerMovement>();

			//rotatablesTransf = transform.GetChild(0).transform;
		}

		private void Move()
		{
			if (!introControls)
			{
				//Vector3 velocity = new Vector3();
				Vector3 velocity = Vector3.zero;

				if (dir.x != 0)
					//velocity += transform.right.normalized * dir.x * speed;
					velocity += Vector3.right.normalized * dir.x;
				if (dir.y != 0)
					//velocity += transform.forward.normalized * dir.y * speed;
					velocity += Vector3.forward.normalized * dir.y;

				//body.velocity = velocity;

				if (velocity == Vector3.zero)
				{
					animator.SetBool("isMoving", false);
					if (AudioManager.Instance != null)
					{
						AudioManager.Instance.Stop("PlayerWalking");
						steps = false;
					}
					return;
				}
				animator.SetBool("isMoving", true);
				if (AudioManager.Instance != null && !steps)
				{
					AudioManager.Instance.Play("PlayerWalking");
					steps = true;
				}

				float ts;
				switch (enabledTurnSpeed)
				{
					case true:
						ts = turnSpeed;
						break;
					case false:
						ts = 1000f;
						break;
				}



				Vector3 newDirection = Vector3.RotateTowards(transform.forward, velocity, ts / 100, 0f);
				Debug.DrawRay(transform.position, velocity * 2, Color.green);
				Debug.DrawRay(transform.position, transform.forward * 3, Color.cyan);
				Debug.DrawRay(transform.position, body.velocity.normalized * 4, Color.red);
				transform.rotation = Quaternion.LookRotation(newDirection);



				body.AddForce(transform.forward * speed * Time.fixedDeltaTime);

				maxSound = false;
			}
			else
			{
				GetComponent<Rigidbody>().useGravity = false;
				GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
				gravityScale = 0;
				speed = 5f;

				Vector3 velocity = Vector3.zero;

				if (dir.x != 0)
					velocity += Vector3.forward.normalized * dir.x;
				if (dir.y != 0)
					velocity += Vector3.up.normalized * dir.y;

				body.velocity = velocity * speed;
			}

			//if(velocity != Vector3.zero) rotatablesTransf.rotation = quaternion.LookRotation(velocity, Vector3.up);
		}

		private void Gravity()
		{
			body.velocity += new Vector3(0, -1, 0) * gravityScale;
		}

		private void FixedUpdate()
		{
			if (!isPressing && !retraction.OxygenLine.ReachedMaximumLength || !Application.isFocused)
				Move();
			else
			{
				body.velocity = Vector3.zero;
				animator.SetBool("isMoving", false);
				AudioManager.Instance.Stop("PlayerWalking");
				steps = false;

				if (retraction.OxygenLine.ReachedMaximumLength && !maxSound)
				{
					if (!outro)
					{
						AudioManager.Instance.Play("O2 Stop");
						maxSound = true;
					}
				}
			}

			Gravity();
		}

		private void OnDrawGizmos()
		{
			if (senseVisualization)
			{
				Gizmos.color = Color.cyan;
				Gizmos.DrawWireSphere(transform.position, pickupRange);
			}
		}

		private void OnMove(InputValue input)
		{
			dir = input.Get<Vector2>();
		}

		private void OnFire(InputValue input)
		{
			Collider[] surroundings = new Collider[0];
			surroundings = Physics.OverlapSphere(transform.position, pickupRange, interactionMask);
			if (currentPickup == null)
			{
				if (surroundings.Any(any => any.gameObject.layer == (int)Mathf.Log(pickupMask.value, 2)))
				{
					currentPickup = surroundings.First(any => any.gameObject.layer == (int)Mathf.Log(pickupMask.value, 2)).gameObject;
					currentPickup.transform.parent = hand.transform;
					currentPickup.transform.position = hand.position;
					currentPickup.GetComponent<StartPos>().PickUp(true);
					playerBtnPrmpt.HasEel = true;
					AudioManager.Instance.Play("eel");
				}
				/*if (surroundings.Any(any => any.gameObject.layer == (int) Mathf.Log(switchMask.value, 2)))
                {
                    IEnumerable<Collider> temp = surroundings.Where(any => any.gameObject.layer == (int) Mathf.Log(switchMask.value, 2));
                    foreach (Collider each in temp)
                        each.gameObject.GetComponent<Switch>().Activate();
                }*/
			}
			else if (surroundings.Any(any => any.gameObject.layer == (int)Mathf.Log(receiverMask.value, 2)))
			{
				if (surroundings.First(any => any.gameObject.layer == (int)Mathf.Log(receiverMask.value, 2)).gameObject.GetComponent<Keyreceiver>().Insert(currentPickup))
				{
					playerBtnPrmpt.DisablePrompt(currentPickup.transform);
					playerBtnPrmpt.DisableSocketPrompt();
					currentPickup = null;
					AudioManager.Instance.Stop("eel");
					AudioManager.Instance.Play("Eel Connected");
				}
			}
			else
			{
				currentPickup.GetComponent<StartPos>().PickUp(false);
				currentPickup.transform.parent = null;
				currentPickup = null;
				AudioManager.Instance.Stop("eel");
			}
			playerBtnPrmpt.HasEel = false;
		}

		private void OnRetract(InputValue input)
		{
			TryRetracting(input.isPressed);
		}

		public void TryRetracting(bool isPressed)
		{
			isPressing = isPressed;

			if (retraction.RetractingConditions())
			{
				if(!outro)
					AudioManager.Instance.Play("O2 Pullback");
				animator.SetBool("isRetracting", true);
			}
			else
			{
				if (!isPressing)
				{
					if (!outro)
					{
						AudioManager.Instance.Stop("O2 Pullback");
						AudioManager.Instance.Play("O2 Stop");
					}
				}
				animator.SetBool("isRetracting", false);
			}
			
		}
		private void OnEsc(InputValue input)
		{
			/*if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Menu"))
                SceneManager.LoadScene("Menu");*/
		}
	}
}