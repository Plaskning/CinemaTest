using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{

    [Tooltip("This is a tooltip")]
    [Header("Input Action Asset")] 
    [SerializeField] private InputActionAsset PlayerControls;

    [Header("Action Map Name References")]
    [SerializeField] private string actionMapName = "Player";

    [Header("Action Name References")]
    [SerializeField] private string move = "Move";
    [SerializeField] private string jump = "Jump";

    private InputAction moveAction;
    private InputAction jumpAction;

    public Vector2 MoveInput { get; private set; }
    public bool JumpTriggered { get; private set; }

    public static PlayerInputHandler Instance { get; private set; }


    private void Awake()
    {
        //Make sure only one instance exists
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        moveAction = PlayerControls.FindActionMap(actionMapName).FindAction(move);
        jumpAction = PlayerControls.FindActionMap(actionMapName).FindAction(jump);


    }

    void RegisterInputActions()
    {
        moveAction.performed += context => MoveInput = context.ReadValue<Vector2>();
        moveAction.canceled += context => MoveInput = Vector2.zero;

        jumpAction.performed += context => JumpTriggered = true;
        jumpAction.canceled += context => JumpTriggered = false;
    }

    private void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
