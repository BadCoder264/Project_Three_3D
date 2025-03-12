using UnityEngine;

public class Recoil : MonoBehaviour
{
    private Vector3 currentRotation;
    private Vector3 targetRotation;

    private void Update()
    {
        ApplyRecoil();
    }

    public void RecoilFire(float recoilX, float recoilY, float recoilZ)
    {
        targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }

    private void ApplyRecoil()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, 6f * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, 2f * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }
}