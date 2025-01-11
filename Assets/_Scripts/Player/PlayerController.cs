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
        ChangeControlledUnit(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            ChangeControlledUnit(0);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            ChangeControlledUnit(1);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            ChangeControlledUnit(2);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            ChangeControlledUnit(3);

    }


    public void OnMove(CallbackContext context)
    {
        Vector2 inputDirection = context.ReadValue<Vector2>();
        Vector3 moveVec = new Vector3(inputDirection.x, inputDirection.y, 0);
        ControlledSlot.SetVelocity(moveVec);
    }

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
