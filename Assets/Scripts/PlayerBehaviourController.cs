using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[RequireComponent(typeof(PlayerHealth))]
public class PlayerBehaviourController : MonoBehaviour
{

    public InputActionAsset input;
    
    private InputAction _fire;
    
    public Transform WeaponRoot;

    public Weapon CurrentWeapon;
    [FormerlySerializedAs("_health")] [HideInInspector]
    public PlayerHealth health;
    
    // Start is called before the first frame update
    void Start()
    {
        var actionMap = input.FindActionMap("FPS");
        actionMap.Enable();
        _fire = actionMap.FindAction("Fire");
        health = GetComponent<PlayerHealth>();
        health.TakeDamage(2);
    }

    private float triggerPulledAt = 0;
    
    // Update is called once per frame
    void Update()
    {
        GameManager.Instance.NotifyHealthPercent(health.Percentage);
        
        if (_fire.triggered)
        {
            triggerPulledAt = Time.time;
        }
        
        if (CurrentWeapon != null && _fire.IsPressed())
        {
            var durationHeld = Time.time - triggerPulledAt;
            CurrentWeapon.PullTrigger(new TriggerPullInformation()
            {
                pulledAt = triggerPulledAt,
                durationHeld = durationHeld,
                triggered = _fire.triggered
            });
        }
    }
}
