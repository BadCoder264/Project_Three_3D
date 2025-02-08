using UnityEngine;

public class Recoil : MonoBehaviour
{
    [Header("Recoil Settings")]
    [SerializeField] private float recoilX;
    [SerializeField] private float recoilY;
    [SerializeField] private float recoilZ;
    [SerializeField] private float snappiness;
    [SerializeField] private float returnSpeed;

    private Vector3 currentRotation;
    private Vector3 targetRotation;

    private void Update()
    {
        _Recoil();
    }

    private void _Recoil()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    public void RecoilFire()
    {
        targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }
}