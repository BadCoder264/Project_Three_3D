using UnityEngine;

public class Recoil : MonoBehaviour
{
    // ==============================
    // Private Variables
    // ==============================
    private Vector3 currentRotation;
    private Vector3 targetRotation;

    // ==============================
    // Unity Methods
    // ==============================
    private void Update()
    {
        ApplyRecoil();
    }

    // ==============================
    // Private Methods
    // ==============================
    private void ApplyRecoil()
    {
        // Smoothly interpolate the target rotation back to zero
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, 6f * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, 2f * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    // ==============================
    // Public Methods
    // ==============================
    public void RecoilFire(float recoilX, float recoilY, float recoilZ)
    {
        targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }
}