using System;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// This class creates a Interface between the different components of the Movementclass.
/// </summary>
public class MovementManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI InteractionUI;
    [SerializeField] private TextMeshProUGUI PickUpUI;
    //scriptable object references
    [Header("ScriptableObject References")]
    [SerializeField] private BehaviorMap selectedBehaviorSO;
    [SerializeField] private SO_JumpBehavior jumpBehaviorSO;
    [SerializeField] private SO_MovementBehavior movementBehaviorSO;
    [SerializeField] private SO_ViewVariables viewBehaviorSO;
    [SerializeField] private SO_RigidbodyBehavior rigidbodyBehaviorSO;
    [SerializeField] private SO_InteractionBehavior interactionBehaviorSO;

    [Header("Components")]
    [SerializeField] private Collider attachedCollider;
    [SerializeField] private CTrigger attachedTrigger;
    [SerializeField] private Rigidbody attachedRigidbody;
    [SerializeField] private Transform attachedCameraTransform;
    [SerializeField] private Transform attachedWeaoponTransform;
    
    //initialzised components
    private CC CharacterController;
    private CRigidbody customRigidbodyBehavior;
    private CharacterViewBehavior viewBehavior;
    private JumpBehavior jumpBehavior;
    private MovementBehavior movementBehavior;
    private InteractBehavior interactBehavior;
    private PickUpBehavior pickUpBehavior;

    //events 
    private event Action updateEvent;
    private event Action fixedUpdateEvent;
    private event Action<RaycastHit?> raycastEvent; //this event returns the hit object 

    private RaycastHit hitObject;
    private float interactDistance;
    private bool interactionEnabled;
    
    private void Awake()
    {
        InitializeCharacterController();
        if (selectedBehaviorSO.Jumping) InitialiseJumpBehavior();
        if (selectedBehaviorSO.Movement) InitializeMovementBehavior();
        if (selectedBehaviorSO.ViewControll) InitializeCharacterViewBehavior();
        if (selectedBehaviorSO.CustomeRigidbody) InitializeCustomeRigidbody();
        if (selectedBehaviorSO.Interaction) InitializeInteractionBehavior();
        if (selectedBehaviorSO.Sprinting) InitializeSprinting();
        if (selectedBehaviorSO.PickUp) InitializePickUpBehavior();
    }
    private void OnDisable()
    {
        if (selectedBehaviorSO.Jumping) DetachJumpBehavior();
        if (selectedBehaviorSO.Movement) DetachMovementBehavior();
        if (selectedBehaviorSO.ViewControll) DetachCharacterViewBehavior();
        if (selectedBehaviorSO.CustomeRigidbody) DetachCustomeRigidbody();
        if (selectedBehaviorSO.Interaction) DetachInteractionBehavior();
        if (selectedBehaviorSO.Sprinting) DetachSprinting();
        if (selectedBehaviorSO.PickUp) DetachPickUpBehavior();
    }
    private void Update()
    {
        updateEvent.Invoke();
    }
    private void FixedUpdate()
    {
        fixedUpdateEvent.Invoke();
        //raycasting from the center of the view
        if(interactionEnabled)
        {
            if (Physics.Raycast(attachedCameraTransform.position, attachedCameraTransform.forward, out hitObject, interactDistance))
                raycastEvent.Invoke(hitObject); //invoking the raycast event with the raycast object
            else raycastEvent.Invoke(null);
        }
    }
    #region Initialise Functions
    //CharacterController
    private void InitializeCharacterController()
    {
        CharacterController = new CC();
        CharacterController.Enable();
    }
    //CRigidbody
    private void InitializeCustomeRigidbody()
    {
        customRigidbodyBehavior = this.AddComponent<CRigidbody>();
        customRigidbodyBehavior.Initialize(rigidbodyBehaviorSO,attachedRigidbody,ref fixedUpdateEvent);
    }
    private void DetachCustomeRigidbody()
    {
        customRigidbodyBehavior.Detach(ref fixedUpdateEvent);
    }
    //View
    private void InitializeCharacterViewBehavior()
    {
        viewBehavior = this.AddComponent<CharacterViewBehavior>();
        CharacterController.CharacterController.Mouse.started += viewBehavior.MouseMoment;
        CharacterController.CharacterController.Mouse.performed += viewBehavior.MouseMoment;
        CharacterController.CharacterController.Mouse.canceled += viewBehavior.MouseMoment;
        viewBehavior.Initialize(attachedCameraTransform, transform, viewBehaviorSO, ref updateEvent);
    }
    private void DetachCharacterViewBehavior()
    {
        CharacterController.CharacterController.Mouse.started -= viewBehavior.MouseMoment;
        CharacterController.CharacterController.Mouse.performed -= viewBehavior.MouseMoment;
        CharacterController.CharacterController.Mouse.canceled -= viewBehavior.MouseMoment;
        viewBehavior.Detach(ref updateEvent);
    }
    //Jump
    private void InitialiseJumpBehavior()
    {
        jumpBehavior = this.AddComponent<JumpBehavior>();
        jumpBehavior.Initialise(attachedRigidbody, attachedTrigger, jumpBehaviorSO);
        CharacterController.CharacterController.Jump.started += jumpBehavior.BehaviorIsCalled;
        CharacterController.CharacterController.Jump.performed += jumpBehavior.BehaviorIsCalled;
    }
    private void DetachJumpBehavior()
    {
        CharacterController.CharacterController.Jump.started -= jumpBehavior.BehaviorIsCalled;
        CharacterController.CharacterController.Jump.performed -= jumpBehavior.BehaviorIsCalled;
    }
    //Movement
    private void InitializeMovementBehavior()
    {
        movementBehavior = this.AddComponent<MovementBehavior>();
        CharacterController.CharacterController.Movement.started += movementBehavior.BehaviorCheck;
        CharacterController.CharacterController.Movement.performed += movementBehavior.BehaviorCheck;
        CharacterController.CharacterController.Movement.canceled += movementBehavior.BehaviorCheck;
        movementBehavior.Initialize(attachedRigidbody,attachedTrigger,movementBehaviorSO, ref fixedUpdateEvent);
    }
    private void DetachMovementBehavior()
    {
        CharacterController.CharacterController.Movement.started -= movementBehavior.BehaviorCheck;
        CharacterController.CharacterController.Movement.canceled -= movementBehavior.BehaviorCheck;
        CharacterController.CharacterController.Movement.performed -= movementBehavior.BehaviorCheck;
        movementBehavior.DetachMovement(ref fixedUpdateEvent);
    }
    //Interaction
    private void InitializeInteractionBehavior()
    {
        interactBehavior = this.AddComponent<InteractBehavior>();
        CharacterController.CharacterController.Interact.started += interactBehavior.BehaviorIsCalled;
        interactBehavior.Initialize(ref raycastEvent, InteractionUI);

        interactDistance = interactionBehaviorSO.InteractionRange; //setting the range of the interaction 
        interactionEnabled = selectedBehaviorSO.Interaction; //enabling the raycast
    }
    private void DetachInteractionBehavior()
    {
        CharacterController.CharacterController.Interact.started -= interactBehavior.BehaviorIsCalled;
        interactBehavior.Detach(ref raycastEvent);

        interactionEnabled = false; //disable the raycast 
    }
    //PickUp
    private void InitializePickUpBehavior()
    {
        if(interactionEnabled) //if interaction is enabled 
        {
            pickUpBehavior = this.AddComponent<PickUpBehavior>();
            CharacterController.CharacterController.PickUp.started += pickUpBehavior.BehaviorIsCalled;
            pickUpBehavior.Initialize(ref raycastEvent, PickUpUI, attachedWeaoponTransform, CharacterController.CharacterController.ItemAction);
        }
    }
    private void DetachPickUpBehavior()
    {
        if(interactionEnabled)
        {
            CharacterController.CharacterController.PickUp.started -= pickUpBehavior.BehaviorIsCalled;
            pickUpBehavior.Detach(ref raycastEvent);
        }
    }
    //Sprinting
    private void InitializeSprinting()
    {
        CharacterController.CharacterController.Sprint.started += movementBehavior.SprintingBehavior;
        CharacterController.CharacterController.Sprint.performed += movementBehavior.SprintingBehavior;
        CharacterController.CharacterController.Sprint.canceled += movementBehavior.SprintingBehavior;
        movementBehavior.InitializeSprinting(movementBehaviorSO);
    }
    private void DetachSprinting()
    {
        CharacterController.CharacterController.Sprint.started -= movementBehavior.SprintingBehavior;
        CharacterController.CharacterController.Sprint.performed -= movementBehavior.SprintingBehavior;
        CharacterController.CharacterController.Sprint.canceled -= movementBehavior.SprintingBehavior;
    }
    #endregion
}
