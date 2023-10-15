using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementController : MonoBehaviour
{
    public InputActionAsset input;

    private InputAction _look;
    private InputAction _move;
    private InputAction _fire;

    private Rigidbody _rb;
    
    [Header("Camera")]
    public Camera cam;
    public float smoothing = .1f;
    public float camHeight = .8f;
    public float lookSensitivity = 100f;
    public bool invertVert = true;
    public bool invertHorz = false;
    public float vertMin = -50;
    public float vertMax = 50;
    public CursorLockMode LockMode = CursorLockMode.Locked;
    
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float maxVelocityChange = 10.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        var actionMap = input.FindActionMap("FPS");
        actionMap.Enable();
        _look = actionMap.FindAction("Look");
        _move = actionMap.FindAction("Move");
        _fire = actionMap.FindAction("Fire");

        if (cam == null) cam = Camera.main;
        _rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Look();
    }
    
    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        var rawMoveInput = _move.ReadValue<Vector2>();
        float moveSideways = rawMoveInput.x * moveSpeed;
        float moveForward = rawMoveInput.y * moveSpeed;
        Vector3 targetVelocity = (transform.right * moveSideways) + (transform.forward * moveForward);

        Vector3 velocity = _rb.velocity;
        Vector3 velocityChange = (targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;

        _rb.AddForce(velocityChange, ForceMode.VelocityChange);
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
        var targetCamPos = new Vector3(transform.position.x, transform.position.y + camHeight, transform.position.z);
        camTransform.position = Vector3.LerpUnclamped(camTransform.position, targetCamPos, smoothing);

        lookRotation = Quaternion.Euler(transform.eulerAngles.x, camTransform.eulerAngles.y, camTransform.eulerAngles.z);
        // _rb.MoveRotation(lookRotation);
        transform.rotation = lookRotation;
    }
}
