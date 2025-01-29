using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;
using static UnityEngine.InputSystem.InputAction;
using System.Linq;
using UnityEngine.InputSystem;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] private CinemachineCamera _cinemachineCamera;
    [SerializeField] private Transform _heroParent;
    [SerializeField] private HeroMover _heroMover;
    [SerializeField] private PlayerInput _input;
    public List<HeroUnit> Heroes { get; private set; }

    public Transform Center { get { return _heroMover.transform; } }

    private void Start()
    {
        _cinemachineCamera.Follow = _heroMover.transform;
        InitHeroes(); // Move this to game manager when init creates heroes.
    }

    public void InitHeroes()
    {
        // TODO - create the heroes using the data of which heroes selected and the stats.

        // Temporarily gets the heroes in the hero group

        Heroes = _heroParent.GetComponentsInChildren<HeroUnit>().ToList();
        
        foreach(HeroUnit hero in Heroes)
        {
            hero.PathfindingModule.SetTarget(_heroMover.transform);
        }

    }


    #region Player Input Methods
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

    public bool DamageHero(Transform heroHit, int damage)
    {
        // Negetive Damage
        if (damage < 0) return false;

        HeroUnit hero = Heroes.Find((h) => h.gameObject == heroHit.gameObject);
        if(hero == null) return false; // Didn't hit a hero.

        hero.TakeDamage(damage);
        return true;
    }

}
