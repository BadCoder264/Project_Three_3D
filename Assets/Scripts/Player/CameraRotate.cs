using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    [SerializeField] private float lookSpeed;
    [SerializeField] private float upDownRange;

    private float rotationX;

    private void Start()
    {
        LockCursor();
    }

    public void RotateCamera()
    {
        if (IsLookSpeedValid())
        {
            HandleMouseInput();
        }
    }

    private bool IsLookSpeedValid()
    {
        return lookSpeed > 0;
    }

    private void HandleMouseInput()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        rotationX = Mathf.Clamp(rotationX - mouseY, -upDownRange, upDownRange);

        ApplyRotation(mouseX);
    }

    private void ApplyRotation(float mouseX)
    {
        transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        transform.parent.Rotate(Vector3.up * mouseX);
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}