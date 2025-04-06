using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float maxSpeed;
    [SerializeField] private float minSpeed;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Rigidbody playerRigidbody;

    private float currentSpeed;

    private void Start()
    {
        if (playerRigidbody == null)
            return;

        currentSpeed = minSpeed;
    }

    public void Move()
    {
        if (playerCamera == null || playerRigidbody == null)
            return;

        Vector3 movement = CalculateMovement();
        playerRigidbody.velocity = new Vector3(movement.x * currentSpeed, playerRigidbody.velocity.y, movement.z * currentSpeed);
    }

    private Vector3 CalculateMovement()
    {
        Vector3 forward = playerCamera.transform.forward;
        Vector3 right = playerCamera.transform.right;

        forward.y = 0;
        right.y = 0;

        return forward.normalized * Input.GetAxis("Vertical") + right.normalized * Input.GetAxis("Horizontal");
    }

    public void Sprint(bool isKeyPressed)
    {
        currentSpeed = isKeyPressed ? maxSpeed : minSpeed;
    }
}