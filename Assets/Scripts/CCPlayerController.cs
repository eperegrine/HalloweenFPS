using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class CCPlayerController : MonoBehaviour
{
    public InputActionAsset input;

    private InputAction _look;
    private InputAction _move;
    private InputAction _fire;
    
    [Header("Camera")]
    public Camera cam;
    public float camHeight = .8f;
    public float lookSensitivity = 100f;
    public bool invertVert = true;
    public bool invertHorz = false;
    public float vertMin = -50;
    public float vertMax = 50;
    public CursorLockMode LockMode = CursorLockMode.Locked;
    
    [Header("Movement")]
    public float moveSpeed = 5f;

    private CharacterController _cc;
    
    // Start is called before the first frame update
    void Start()
    {
        var actionMap = input.FindActionMap("FPS");
        actionMap.Enable();
        _look = actionMap.FindAction("Look");
        _move = actionMap.FindAction("Move");
        _fire = actionMap.FindAction("Fire");

        if (cam == null) cam = Camera.main;
        _cc = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Look();
    }
    
    private float vertAngle = 0;
    private Quaternion lookRotation;
    
    void Look()
    {
        Vector2 rawLookInput = _look.ReadValue<Vector2>() * lookSensitivity * Time.deltaTime;
        float viewVert = rawLookInput.y;
        float viewHorz = rawLookInput.x;
        if (invertVert) viewVert *= -1;
        if (invertHorz) viewHorz *= -1;

        Debug.Log(rawLookInput);

        float newVert = vertAngle + viewVert;
        vertAngle = Mathf.Clamp(newVert, vertMin, vertMax);

        var camTransform = cam.transform;
        camTransform.rotation = Quaternion.Euler(
            vertAngle,
            camTransform.eulerAngles.y + viewHorz,
            camTransform.eulerAngles.z);
        camTransform.position = new Vector3(transform.position.x, transform.position.y + camHeight, transform.position.z);
        // Quaternion.Lerp()

        lookRotation = Quaternion.Euler(transform.eulerAngles.x, camTransform.eulerAngles.y, camTransform.eulerAngles.z);
        // _rb.MoveRotation(lookRotation);
        transform.rotation = lookRotation;
    }
}
