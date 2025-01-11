using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Cinemachine;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.InputSystem;

public class PlayerController : Singleton<PlayerController>
{
    [field: SerializeField]
    public List<UnitSlot> UnitSlots { get; private set; }
    private int _controlledSlotIndex = 0;
    public UnitSlot ControlledSlot { get { return UnitSlots[_controlledSlotIndex]; } }

    [SerializeField] private CinemachineCamera _cinemachineCamera;

    private void Start()
    {
        // TODO - add creation of units.
        ChangeControlledUnit(0);
    }

    #region Player Input Methods
    public void OnMove(CallbackContext context)
    {
        if (GameManager.Instance.IsPaused) return; // Cant move while paused;

        Vector2 inputDirection = context.ReadValue<Vector2>();
        Vector3 moveVec = new Vector3(inputDirection.x, inputDirection.y, 0);
        ControlledSlot.SetVelocity(moveVec);
    }

    public void OnInteract(CallbackContext context)
    {
        if (GameManager.Instance.IsPaused) return; // Can't interact while paused.


        // Interact with stuff.
    }

    public void OnPause(CallbackContext context)
    {
        if (context.started)
        {
            if(!GameManager.Instance.IsPaused)
                GameManager.Instance.PauseGame();
            else // TODO - Remove this later, only for debugging atm, exiting pause will be from the pause menu or after selecing an upgrade.
                GameManager.Instance.ResumeGame();
        }
    }

    public void OnDeviceLost()
    {
        GameManager.Instance.PauseGame();
    }

    #endregion

    private void ChangeControlledUnit(int index)
    {
        if (index < 0 || index >= UnitSlots.Count)
            throw new ArgumentOutOfRangeException();

        _controlledSlotIndex = index;

        for (int i = 0; i < UnitSlots.Count; i++)
        {
            if (i == index)
            {
                UnitSlots[i].SetControl(true);
                _cinemachineCamera.Follow = UnitSlots[i].Unit.transform;
            }
            else
            {
                UnitSlots[i].SetControl(false, ControlledSlot.Unit.transform);
            }
        }
    }

}
