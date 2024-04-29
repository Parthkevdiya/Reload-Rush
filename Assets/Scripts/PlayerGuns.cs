using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerGuns : MonoBehaviour
{
    public const string UR_FINISH_LINE_TAG_STRING = "UR_FinishLine";
    public const string MONEY_BUNDLE_TAG_STRING = "MoneyBundle";
    public static PlayerGuns Instance { get; private set; }

    [SerializeField] private GunHolder[] gunHolder;
    [SerializeField] private Vector3[] gunUpgradeRoundPositions;
    [SerializeField] private Vector3[] gunFinalRoundPositions;

    [SerializeField] private PlayerMovement playerMovement;

    [SerializeField] private float boxCastxHalfExtendMultiplier = 1;

    [SerializeField] public GameObject bulletHitPartical;
    [SerializeField] public GameObject moneyBundlePartical;

    [SerializeField] public bool levelEnded = false;
    private void Start()
    {
        Instance = this;
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        Vector3 boxCastCenter = new Vector3(0, 0.5f, 0);
        Vector3 boxCastHalfExtend = new Vector3(0.5f * boxCastxHalfExtendMultiplier, 0.5f, .21f);
        float boxCastMaxDistance = 0.05f;

        RaycastHit[] hitInfo = Physics.BoxCastAll(transform.position + boxCastCenter, boxCastHalfExtend, transform.forward, Quaternion.identity, boxCastMaxDistance);
        if (hitInfo != null)
        {
            foreach (RaycastHit hit in hitInfo)
            {
                /*if (hit.transform.tag.Equals(Bullet.FIRE_RATE_CHARGER_TAG_STRING))
                {
                    int fireRateValue = hit.transform.GetComponent<FireRateCharger>().GetFireRateValue();
                    UpdateGunsFireRate(fireRateValue);
                    Destroy(hit.transform.gameObject);
                }*/

                /*if (hit.transform.tag.Equals(Bullet.DAMAGE_CHARGER_TAG_STRING))
                {
                    int damageValue = hit.transform.GetComponent<DamageCharger>().GetDamageValue();
                    UpdateGunsDamage(damageValue);
                    Destroy(hit.transform.gameObject);
                }*/

                /*if (hit.transform.tag.Equals(Bullet.OBSTACLE_TAG_STRING))
                {
                    Debug.Log("Game Over");
                    playerMovement.enabled = false;
                    StopGunFire();
                    this.enabled = false;
                }*/

                if (hit.transform.tag.Equals(UR_FINISH_LINE_TAG_STRING))
                {
                    hit.transform.tag = "Untagged";
                    Debug.Log("SetFinalRoundStateHere");
                    ReloadRushGameManager.Instance.SetGameState(ReloadRushGameManager.GameState.FinalRound);
                    SetGunPositionsAsFinalRound();
                }

                if (hit.transform.tag.Equals(MONEY_BUNDLE_TAG_STRING))
                {
                    Destroy(hit.transform.gameObject);
                    int randomCash = UnityEngine.Random.Range(10, 50);
                    ReloadRushGameManager.Instance.AddCollectedCash(randomCash);
                    ReloadRushGameManager.Instance.AddMoney(randomCash);
                    Instantiate(moneyBundlePartical , transform.position ,Quaternion.identity);
                }
                /*if (hit.transform.tag.Equals(Bullet.STOPPER_TAG_STRING))
                {
                    playerMovement.TakePushBack();
                }*/
            }
        }
    }

    public void CheckGunHit(Collider collider)
    {
        if (collider.transform.tag.Equals(Bullet.FIRE_RATE_CHARGER_TAG_STRING))
        {
            int fireRateValue = collider.transform.GetComponent<FireRateCharger>().GetFireRateValue();
            UpdateGunsFireRate(fireRateValue);
            Destroy(collider.transform.gameObject);
        }

        if (collider.transform.tag.Equals(Bullet.DAMAGE_CHARGER_TAG_STRING))
        {
            int damageValue = collider.transform.GetComponent<DamageCharger>().GetDamageValue();
            UpdateGunsDamage(damageValue);
            Destroy(collider.transform.gameObject);
        }

        if (collider.transform.tag.Equals(Bullet.OBSTACLE_TAG_STRING))
        {
            if (levelEnded)
            {
                return;
            }

            levelEnded = true;

            AdmobAdManager.Instance.Show_Interstitial_Admob();

            Debug.Log("Game Over");
            playerMovement.RemoveMovementFromEvent();
            playerMovement.enabled = false;
            StopGunFire();
            this.enabled = false;
            ReloadRushGameManager.Instance.SpawnTheEndingPanelUI();
            ReloadRushGameManager.Instance.SetNextLevel();
            LevelSetup.Instance.CleanUp();
            LevelSetup.Instance.SetUpLevel();
            this.transform.position = Vector3.down * 10;
            // Magazine.Instance.transform.position = Vector3.zero;
            Destroy(Magazine.Instance.transform.gameObject);
            LevelSetup.Instance.SpawnHandMagazine();
            
        }

        if (collider.transform.tag.Equals(UR_FINISH_LINE_TAG_STRING))
        {
            Debug.Log("SetFinalRoundStateHere");
            ReloadRushGameManager.Instance.SetGameState(ReloadRushGameManager.GameState.FinalRound);
            SetGunPositionsAsFinalRound();
        }

        if (collider.transform.tag.Equals(Bullet.STOPPER_TAG_STRING))
        {
            playerMovement.TakePushBack();
        }
    }

    public void StartGunsFire()
    {
        for (int i=0; i<gunHolder.Length; i++)
        {
            if (gunHolder[i].gunScript != null)
            {
                gunHolder[i].gunScript.enabled = true;
            }
        }
    }

    public void StopGunFire()
    {
        for (int i = 0; i < gunHolder.Length; i++)
        {
            if (gunHolder[i].gunScript != null)
            {
                gunHolder[i].gunScript.enabled = false;
            }
        }
    }

    public void UpdateGunsDamage(int damage)
    {
        for (int i =0; i<gunHolder.Length; i++)
        {
            if (gunHolder[i].gunScript != null)
            {
                gunHolder[i].gunScript.AddDamage(damage);
            }
        }
    }

    public void UpdateGunsFireRate(int fireRate)
    {
        for (int i = 0; i < gunHolder.Length; i++)
        {
            if (gunHolder[i].gunScript != null)
            {
                gunHolder[i].gunScript.AddFireRate(fireRate);
            }
        }
    }

    public void SetGunPositionsAsGunUpgradeRound()
    {
        float gunOffsetTimes = 0;
        for (int c =0; c<gunHolder.Length; c++)
        {
            if (gunHolder[c].gunScript != null)
            {
                gunOffsetTimes += 0.5f;
            }
        }

        int i = 0;
        while (gunHolder[i].gunScript != null || i <= 4)
        {
            Vector3 gunOffsetValue = new Vector3(1.5f * (2 - gunOffsetTimes+ 0.5f) , 0 , 0);
            gunHolder[i].gunScript.transform.DOLocalMove(gunUpgradeRoundPositions[i] + gunOffsetValue, 1f);
            i++;
        }
    }

    public void SetGunPositionsAsFinalRound()
    {
        float gunOffsetTimes = 0;
        for (int c = 0; c < gunHolder.Length; c++)
        {
            if (gunHolder[c].gunScript != null)
            {
                gunOffsetTimes += 0.5f;
            }
        }

        int i = 0;
        while (i <= 4 && gunHolder[i].gunScript != null)
        {
            Vector3 gunOffsetValue = new Vector3(1.5f * (2 - gunOffsetTimes + 0.5f), 0, 0);
            gunHolder[i].gunScript.transform.DOLocalMove(gunFinalRoundPositions[i] + gunOffsetValue, 1f);
            i++;
        }
    }

    public void AddGunInGunHolder(Gun gun)
    {
        for (int i=0; i< gunHolder.Length; i++)
        {
            if (gunHolder[i].gunScript == null)
            {
                gunHolder[i].gunScript = gun;
                break;
            }
        }
    }

    public int GetActivatedGunsCount()
    {
        int k = 0;
        for (int i = 0; i < gunHolder.Length; i++)
        {
            if (gunHolder[i].gunScript != null)
            {
                k++;
            }
        }

        return k;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 boxCastCenter = new Vector3(0, 0.5f, 0.1f);
        Vector3 boxCastHalfExtend = new Vector3(1 * boxCastxHalfExtendMultiplier, 0.5f, .21f);
        Gizmos.DrawRay(transform.position + boxCastCenter, transform.forward * 1f);
        Gizmos.DrawWireCube(transform.position + boxCastCenter, boxCastHalfExtend * 2);
    }
}

[Serializable]
public struct GunHolder
{
    public string name;
    public Gun gunScript;
}
