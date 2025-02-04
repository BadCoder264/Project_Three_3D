using UnityEngine;

public class PickUpWeapon : MonoBehaviour
{
    [Header("Pick Up Settings")]
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask targetLayerMask;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform weaponHandler;

    private InputListner inputListener;

    private void Start()
    {
        inputListener = FindObjectOfType<InputListner>();
    }

    public void PickUp(bool isKeyPressed)
    {
        if (isKeyPressed)
        {
            if (playerCamera != null)
            {
                Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, rayDistance, targetLayerMask))
                {
                    GameObject hitObject = hit.collider.gameObject;
                    PlayerShoot playerShoot = hitObject.GetComponent<PlayerShoot>();

                    if (playerShoot != null)
                    {
                        if (inputListener != null)
                        {
                            inputListener.PlayerWeaponsList.Add(hitObject);
                            inputListener.PlayerShootingList.Add(playerShoot);
                            inputListener.weaponSwitcherController.CurrentWeaponIndex = inputListener.PlayerShootingList.Count - 1;
                        }

                        playerShoot.IsWeaponEquipped = true;

                        if (weaponHandler != null)
                        {
                            foreach (Transform child in weaponHandler)
                            {
                                child.gameObject.SetActive(false);
                            }

                            hitObject.transform.SetParent(weaponHandler);
                        }

                        hitObject.GetComponent<BoxCollider>().enabled = false;
                        hitObject.transform.localPosition = Vector3.zero;
                        hitObject.transform.localRotation = Quaternion.identity;
                        hitObject.SetActive(true);
                    }
                }
            }
        }
    }
}