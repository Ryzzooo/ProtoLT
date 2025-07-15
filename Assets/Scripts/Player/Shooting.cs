using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bullet;
    public Transform shotPoint;

    void Update()
    {
        Shoot();
    }

    void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Instantiate(bullet, shotPoint.position, transform.rotation);
        }
    }
}
