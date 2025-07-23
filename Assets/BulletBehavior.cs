using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float speed, damage, destroyTime;
    public LayerMask enemyLayer;

    private void Awake()
    {
        Destroy(gameObject, destroyTime);
    }
    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if ((enemyLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            EnemyHealth enemy = collision.GetComponentInParent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            else
            {
                Debug.LogWarning("EnemyHealth tidak ditemukan pada atau di atas " + collision.name);
            }

            Destroy(gameObject);
        }
        else if (collision.CompareTag("Environment"))
        {
            Destroy(gameObject);
        }
    }
}
