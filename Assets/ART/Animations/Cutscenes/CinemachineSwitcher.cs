using UnityEngine;
using UnityEngine.InputSystem;

public class CinemachineSwitcher : MonoBehaviour
{

    [SerializeField] private InputAction action;
    
    private Animator introanimator;

    private bool startCamera = true;

    private void Awake()
    {
        introanimator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        action.Enable();
    }

    private void OnDisable()
    {
        action.Disable();
    }

    void Start()
    {
        action.performed += _ => SwitchState();
    }

    private void SwitchState()
    {
        if (startCamera)
            {
            introanimator.Play("ZoomOutCamera");
            }
    }

}
