using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 1f;
    public float scaleSpeed = 1f;
    private Rigidbody rb;
    private Vector2 moveInput;
    private float scaleInput;
    private PlayerControls controls;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controls = new PlayerControls();

        // Subscribe to input
        // controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        // controls.Player.Move.canceled += _ => moveInput = Vector2.zero;
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += _ => moveInput = Vector2.zero;
        controls.Player.Scale.performed += ctx => scaleInput = ctx.ReadValue<float>();
        controls.Player.Scale.canceled += _ => scaleInput = 0;
    }

    void OnEnable() {
        controls.Enable();                 // enable ALL maps
        controls.Player.Enable();          // belt & suspenders: enable the Player map too
        Debug.Log($"Enabled? Move:{controls.Player.Move.enabled}  Map:{controls.Player.enabled}");
    }
    void OnDisable() {
        controls.Player.Disable();
        controls.Disable();
    }


    void FixedUpdate()
    {
        // rb.linearVelocity = moveInput * moveSpeed;
        // Vector3 v = new Vector3(moveInput.x, 0f, moveInput.y) * moveSpeed;
        // rb.linearVelocity = v;

        // Forward/backward
        float move = moveInput.y * moveSpeed;
        // Left/right rotation
        float turn = moveInput.x * rotationSpeed;
        // Move forward in facing direction
        rb.MovePosition(rb.position + (Vector3)transform.forward * move * Time.fixedDeltaTime);
        // Apply rotation around Y
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0, turn, 0));

        if (Mathf.Abs(scaleInput) > 0.01f)
        {
            Vector3 newScale = transform.localScale + Vector3.one * scaleInput * scaleSpeed * Time.deltaTime;
            newScale = Vector3.Max(newScale, Vector3.one * 0.1f); // prevent negative or zero scale
            transform.localScale = newScale;
        }
    }
}
