using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;
using static UnityEngine.InputSystem.InputAction;
using System.Linq;
using UnityEngine.InputSystem;
using SeraphRandom;
using SeraphUtil;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private HeroMover _heroMover;
    [SerializeField] private PlayerInput _input;


    public void OnMove(CallbackContext context)
    {
    //    if (GameManager.Instance.IsPaused) return; // Cant move while paused;

        Vector2 moveVec = context.ReadValue<Vector2>();
        _heroMover.Move(moveVec);
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
                GameManager.Instance.PauseGame(true);
        }
    }

    public void OnDeviceLost()
    {
        GameManager.Instance.PauseGame(true);
    }
}
