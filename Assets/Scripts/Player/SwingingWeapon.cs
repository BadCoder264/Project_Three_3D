using UnityEngine;

public class SwingingWeapon : MonoBehaviour
{
    // ==============================
    // Serialized Fields
    // ==============================
    [Header("Weapon Sway Settings")]
    [SerializeField] private float swayAmount;
    [SerializeField] private float smoothFactor;

    // ==============================
    // Unity Methods
    // ==============================
    private void Update()
    {
        ApplySwing();
    }

    // ==============================
    // Private Methods
    // ==============================
    private void ApplySwing()
    {
        float mouseX = Input.GetAxis("Mouse X") * swayAmount;
        float mouseY = Input.GetAxis("Mouse Y") * swayAmount;

        Vector3 targetPosition = new Vector3(mouseX, mouseY, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * smoothFactor);
    }
}