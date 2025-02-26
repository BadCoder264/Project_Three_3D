using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    // ==============================
    // Serialized Fields
    // ==============================
    [Header("Camera Rotation Settings")]
    [SerializeField] private float lookSpeed;
    [SerializeField] private float upDownRange;

    // ==============================
    // Private Variables
    // ==============================
    private float rotationX;

    // ==============================
    // Unity Methods
    // ==============================
    private void Start()
    {
        LockCursor();
    }

    private void Update()
    {
        RotateCamera();
    }

    // ==============================
    // Public Methods
    // ==============================
    public void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        rotationX = Mathf.Clamp(rotationX - mouseY, -upDownRange, upDownRange);

        transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        transform.parent.Rotate(Vector3.up * mouseX);
    }

    // ==============================
    // Private Methods
    // ==============================
    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}