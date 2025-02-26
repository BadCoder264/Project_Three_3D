using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // ==============================
    // Serialized Fields
    // ==============================
    [Header("Movement Settings")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float minSpeed;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Rigidbody playerRigidbody;

    // ==============================
    // Private Variables
    // ==============================
    private float currentSpeed;

    // ==============================
    // Unity Methods
    // ==============================
    private void Start()
    {
        currentSpeed = minSpeed;
    }

    // ==============================
    // Public Methods
    // ==============================
    public void Move()
    {
        if (playerCamera != null && playerRigidbody != null)
        {
            Vector3 forward = playerCamera.transform.forward;
            Vector3 right = playerCamera.transform.right;

            forward.y = 0;
            right.y = 0;

            Vector3 movement = forward.normalized * Input.GetAxis("Vertical") + right.normalized * Input.GetAxis("Horizontal");
            playerRigidbody.velocity = new Vector3(movement.x * currentSpeed, playerRigidbody.velocity.y, movement.z * currentSpeed);
        }
    }

    public void Sprint(bool isKeyPressed)
    {
        currentSpeed = isKeyPressed ? maxSpeed : minSpeed;
    }
}