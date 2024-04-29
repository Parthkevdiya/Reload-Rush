using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public const string STOPPER_TAG_STRING = "Stopper";
    public const string DAMAGE_CHARGER_TAG_STRING = "DamgeCharger";
    public const string FIRE_RATE_CHARGER_TAG_STRING = "FireRateCharger";
    public const string OBSTACLE_TAG_STRING = "Obstacles";
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletDestroyTime;

    [SerializeField] private int bulletDamage;
    private void Start()
    {
        Destroy(this.gameObject , bulletDestroyTime);
    }
    private void Update()
    {
        transform.position += transform.forward * Time.deltaTime * bulletSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag.Equals(STOPPER_TAG_STRING))
        {
            Debug.Log("Misery");
            collision.collider.transform.GetComponent<Enemy>().TakeDamage(bulletDamage);
        }

        if (collision.collider.tag.Equals(DAMAGE_CHARGER_TAG_STRING))
        {
            collision.collider.transform.GetComponent<DamageCharger>().TakeDamage(Random.Range(0, 3));
        }

        if (collision.collider.tag.Equals(FIRE_RATE_CHARGER_TAG_STRING))
        {
            collision.collider.transform.GetComponent<FireRateCharger>().TakeDamage(Random.Range(0, 3));
        }

        if (collision.collider.tag.Equals(OBSTACLE_TAG_STRING))
        {
            collision.collider.transform.GetComponent<Obstacle>().TakeDamage(bulletDamage);
        }

        Instantiate(PlayerGuns.Instance.bulletHitPartical , transform.position , Quaternion.identity);
        SoundManager.Instance.PlayShotSound();
        Destroy(this.gameObject);
    }


    public void SetBulletDamage(int damage)
    {
        bulletDamage = damage;
    }
}
