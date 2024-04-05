using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    
    #region Singleton

    public static InputManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            // GetComponent
            _playerInput = GetComponent<PlayerInput>();


            // auto binding
            move = _playerInput.actions["Move"];
            sprint = _playerInput.actions["Sprint"];
            attack = _playerInput.actions["Attack"];
            dodge = _playerInput.actions["Dodge"];
            grafitti = _playerInput.actions["GraffitiSystem"];

            confirm = _playerInput.actions["Confirm"];
            cancel = _playerInput.actions["Cancel"];
            equipment = _playerInput.actions["OpenEquipment"];
            inventory = _playerInput.actions["OpenInventory"];
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    public Vector2 MoveInput {get; private set;}
    public bool SprintInput {get; private set;}
    public bool AttackInput {get; private set;}
    public bool DodgeInput {get; private set;}
    public bool DodgeProgressInput {get; private set;}
    public bool GraffitiStartInput {get; private set;}
    public bool GraffitiEndInput {get; private set;}

    public bool ConfirmInput {get; private set;}
    public bool CancelInput {get; private set;}
    public bool EquipmentOpenCloseInput {get; private set;}
    public bool InventoryOpenCloseInput {get; private set;}

    private PlayerInput _playerInput;
    private InputAction move, sprint, attack, dodge, grafitti, confirm, cancel, equipment, inventory;

    public bool stopPlayer;
    public bool canSprint;


    public void StopPlayerInput(bool _stop)
    {
        stopPlayer = _stop;
        MoveInput = Vector2.zero;
        SprintInput = false;
        AttackInput = false;
        DodgeInput = false;
        DodgeProgressInput = false;
        GraffitiStartInput = false;
        GraffitiEndInput = false;
    }


    private void Update()
    {
        // player input detect
        if (!stopPlayer)
        {
            MoveInput = move.ReadValue<Vector2>();
            if (canSprint)
            {
                SprintInput = sprint.inProgress;
            }
            AttackInput = attack.WasPressedThisFrame();
            DodgeInput = dodge.WasPressedThisFrame();
            DodgeProgressInput = dodge.inProgress;
            GraffitiStartInput = grafitti.WasPressedThisFrame();
            GraffitiEndInput = grafitti.WasReleasedThisFrame();
        }
        

        // ui input detect 
        ConfirmInput = confirm.WasPressedThisFrame();
        CancelInput = cancel.WasPressedThisFrame();
        EquipmentOpenCloseInput = equipment.WasPressedThisFrame();
        InventoryOpenCloseInput = inventory.WasPressedThisFrame();
    }
}
