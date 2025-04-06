using UnityEngine;

public class Recoil : MonoBehaviour
{
    private Vector3 currentRotation;
    private Vector3 targetRotation;
    private const float returnSpeed = 6f;
    private const float recoilSmoothFactor = 2f;

    private void Update()
    {
        ApplyRecoil();
    }

    private void ApplyRecoil()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, recoilSmoothFactor * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    public void RecoilFire(float recoilX, float recoilY, float recoilZ)
    {
        targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }
}