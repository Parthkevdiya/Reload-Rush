using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public const string BULLET_TAG_STRING = "Bullet";

    [SerializeField] private int damage;
    [SerializeField] private int fireRate;
    [SerializeField] private Transform magazinePoint;
    [SerializeField] private float moveUpYvalue;
    [SerializeField] private bool gunActived;

    private float fireTime = 1;
    int maxFireTime = 10;

    [SerializeField] private Bullet bulletPrefeb;

    [SerializeField] private Transform firePoint;
    private void Update()
    {
        if (fireTime <= 0)
        {
            fireTime = maxFireTime;
            // Here Shoot Bullet
            Bullet bullet = Instantiate(bulletPrefeb , firePoint.position , Quaternion.identity , null);
            bullet.SetBulletDamage(damage);
        }
        else
        {
            fireTime -= fireRate * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(BULLET_TAG_STRING))
        {
            return;
        }

        PlayerGuns.Instance.CheckGunHit(other);
    }

    #region Variable Helper Functions

    public int GetDamage()
    {
        return damage;
    }

    public void AddDamage(int damageValue)
    {
        damage += damageValue;
        int clampDamage = Mathf.Clamp(damage , 2 , 9999);
        damage = clampDamage;
    }


    public void AddFireRate(int fireRateValue)
    {
        fireRate += fireRateValue;
        int clampFireRate = Mathf.Clamp(fireRate, 10, 9999);
        fireRate = clampFireRate;
    }

    public Transform GetGunReloadPoint()
    {
        return magazinePoint;
    }

    public void SetGunActivated()
    {
        gunActived = true;
    }

    public bool GetIsGunActivated()
    {
        return gunActived;
    }
    public void ReloadMagazine(MagazineVisual magazine)
    {
        magazine.transform.parent = this.transform;
        transform.DOMoveY(transform.position.y + moveUpYvalue, 1);
        magazine.transform.DOLocalMove(magazinePoint.localPosition - new Vector3(0 , moveUpYvalue , 0), 1f);
        Vector3 magazineRotaion = new Vector3(magazinePoint.localRotation.x , magazinePoint.localRotation.y , magazinePoint.localRotation.z);
        magazine.transform.DOLocalRotate(magazineRotaion, 1f).OnComplete(() => 
        {
            transform.DOMoveY(transform.position.y - moveUpYvalue, 0.25f);
            magazine.transform.DOLocalMove(magazinePoint.localPosition, 0.25f).OnComplete(() => 
            {
                ReloadZone.Instance.AddReloadNumber();
                SetGunActivated();
                SoundManager.Instance.PlayReloadSound();
                ReloadZone.Instance.ReloadGun();
            });
        });
    }
    #endregion
}
