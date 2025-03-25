using UnityEngine;

public class SwingingWeapon : MonoBehaviour
{
    [SerializeField] private float swayAmount;
    [SerializeField] private float smoothFactor;

    private void Update()
    {
        ApplySwing();
    }

    private void ApplySwing()
    {
        float mouseX = Input.GetAxis("Mouse X") * swayAmount;
        float mouseY = Input.GetAxis("Mouse Y") * swayAmount;

        Vector3 targetPosition = new Vector3(mouseX, mouseY, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * smoothFactor);
    }
}