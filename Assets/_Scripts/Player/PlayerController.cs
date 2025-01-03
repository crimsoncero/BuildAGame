using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    [field: SerializeField]
    public List<UnitSlot> UnitSlots { get; private set; }
    private int _controlledSlotIndex = 0;
    public UnitSlot ControlledSlot { get { return UnitSlots[_controlledSlotIndex]; } }

    private void Start()
    {
        ChangeControlledUnit(0);
    }

    private void Update()
    {
        Vector3 moveVec = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        ControlledSlot.SetVelocity(moveVec);

        if (Input.GetKeyDown(KeyCode.Alpha1))
            ChangeControlledUnit(0);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            ChangeControlledUnit(1);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            ChangeControlledUnit(2);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            ChangeControlledUnit(3);

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
            }
            else
            {
                UnitSlots[i].SetControl(false, ControlledSlot.Unit.transform);
            }
        }
    }

}
