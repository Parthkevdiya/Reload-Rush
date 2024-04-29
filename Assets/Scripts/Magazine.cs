using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Magazine : MonoBehaviour
{
    public static Magazine Instance { get; private set; }

    public const string RELOAD_ZONE_TAG_STRING = "Reload Zone";
    public const string BULLET_TAG_STRING = "Bullet";
    public const string M_BULLET_TAG_STRING = "M_Bullet";

    PlayerMovement playerMovement;

    [SerializeField] private MagazineData magazineData;

    [Header("   ")]
    [SerializeField] private int currentIndexOfMagazine = 0;
    [SerializeField] private int currentBulletCount = 0;
    [SerializeField] private CompletedMagazine completedMagazine;
    [SerializeField] private MagazineVisual[] magazineVisual;

    [SerializeField] private GameObject bulletToSpawn;

    
    //[SerializeField] private MagazinesOfAllLevels[] magazinesOfAllLevels;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        ReloadRushGameManager.Instance.OnGameStart += ReloadRushGameManager_OnGameStart;
        playerMovement = GetComponent<PlayerMovement>();
        SetUpMagazines();
        magazineVisual[currentIndexOfMagazine].SetMagazineBullet(currentBulletCount, magazineVisual[currentIndexOfMagazine].GetMagazineMaxBullet());
    }

    private void Update()
    {
        
        Vector3 boxCastCenter = new Vector3(0 , 0.5f , 0.1f);
        Vector3 boxCastHalfExtend = new Vector3(0.18f*2 , 0.5f , .21f);
        float boxCastMaxDistance = 0.05f;
        /*if(Physics.BoxCast(transform.position + boxCastCenter, boxCastHalfExtend , transform.forward , out RaycastHit hitInfo , Quaternion.identity , boxCastMaxDistance))
        {
            Debug.Log(hitInfo.transform.name);
            if (hitInfo.transform.tag.Equals(RELOAD_ZONE_TAG_STRING))
            {
                EndMagazineState();
            }

            if (hitInfo.transform.tag.Equals(BULLET_TAG_STRING))
            {
                Destroy(hitInfo.transform.gameObject);
                currentBulletCount++;
                if (currentBulletCount >= magazineData[currentIndexOfMagazine].maxBulletCount)
                {
                    currentIndexOfMagazine++;
                    currentBulletCount = 0;

                }
                magazineVisual[currentIndexOfMagazine].SetMagazineBullet(currentBulletCount, magazineData[currentIndexOfMagazine].maxBulletCount);
            }
        }*/

        RaycastHit[] hitInfo = Physics.BoxCastAll(transform.position + boxCastCenter, boxCastHalfExtend, transform.forward, Quaternion.identity, boxCastMaxDistance);
        if (hitInfo != null)
        {
            foreach (RaycastHit hit in hitInfo) 
            {
                if (hit.transform.tag.Equals(RELOAD_ZONE_TAG_STRING))
                {
                    EndMagazineState();
                }

                if (hit.transform.tag.Equals(BULLET_TAG_STRING))
                {
                    
                    if (AddBulletInMagazine())
                    {
                        hit.transform.tag = "Untagged";
                        hit.transform.parent = this.transform;
                        float randomTime = 1f - (UnityEngine.Random.Range(0, 50) * 0.01f);

                        StartCoroutine(WaitAndPlayGetBulletSound(randomTime / 2));
                        hit.transform.DOLocalJump(Vector3.zero , 2 , 1 , randomTime).OnComplete(() => { Destroy(hit.transform.gameObject); });
                        hit.transform.DOLocalRotate(new Vector3(-28 , -15 ,-31 ) , randomTime);
                    }
                }

                if (hit.transform.tag.Equals(M_BULLET_TAG_STRING))
                {
                    Destroy(hit.transform.gameObject);
                    int incBulletNumber = hit.transform.GetComponent<M_Bullet>().GetBulletCount();

                    for (int i=1; i<=5; i++)
                    {
                        GameObject bullet = Instantiate(bulletToSpawn, this.transform);
                        bullet.transform.localPosition = new Vector3(0 , 3 , 0);
                        bullet.transform.DOLocalRotate(new Vector3(-28, -15, -31), 0);

                        float randomTime = 1.5f - (i * 0.25f);
                        //bullet.transform.DOLocalMove(Vector3.zero, randomTime).OnComplete(() => { Destroy(bullet); });
                        StartCoroutine(WaitAndPlayGetBulletSound(randomTime /2));
                        bullet.transform.DOLocalJump(Vector3.zero , 2 , 1 , randomTime).OnComplete(() => { Destroy(bullet); });
                    }
                    for (int i = 0; i < incBulletNumber; i++) 
                    {   // this could be more performing

                        if (AddBulletInMagazine())
                        {
                            Destroy(hit.transform.gameObject);
                        }
                    }
                }
            }
            
        }
    }

    public void AddBulletViaButtonEffects(int bulletCount)
    {
        for (int i = 1; i <= bulletCount; i++)
        {
            GameObject bullet = Instantiate(bulletToSpawn, this.transform);
            bullet.transform.localPosition = new Vector3(0, 3, 0);
            bullet.transform.DOLocalRotate(new Vector3(-28, -15, -31), 0);

            float randomTime = 1.5f - (i * 0.25f);

            StartCoroutine(WaitAndPlayGetBulletSound(randomTime /2));
            
            bullet.transform.DOLocalJump(Vector3.zero, 2, 1, randomTime).OnComplete(() => {  Destroy(bullet); });
        }

        OnScreenPanelUI.Instance.AddBulletShowEffect(bulletCount);
    }

    private IEnumerator WaitAndPlayGetBulletSound(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SoundManager.Instance.PlayGetBulletSound();
    }

    public bool AddBulletInMagazine()
    {
        if (currentIndexOfMagazine >= 5)
        {
            Debug.Log("I am All Out");
            return false;
        }
        

        currentBulletCount++;
        if (currentBulletCount >= magazineVisual[currentIndexOfMagazine].GetMagazineMaxBullet() && currentIndexOfMagazine != 4)
        {
            completedMagazine.SetUpCompletedMagazine(magazineVisual[currentIndexOfMagazine]);
            currentIndexOfMagazine++;
            currentBulletCount = 0;
            magazineVisual[currentIndexOfMagazine].gameObject.SetActive(true);
        }

        if (currentIndexOfMagazine == 4 && magazineVisual[currentIndexOfMagazine].GetMagazineMaxBullet() == currentBulletCount)
        {
            completedMagazine.SetUpLastMagazine(magazineVisual[currentIndexOfMagazine]);
        }

        if (currentBulletCount <= magazineVisual[currentIndexOfMagazine].GetMagazineMaxBullet())
        {
            magazineVisual[currentIndexOfMagazine].SetMagazineBullet(currentBulletCount, magazineVisual[currentIndexOfMagazine].GetMagazineMaxBullet());
            ReloadRushGameManager.Instance.AddCollectedBullets();
            return true;
        }

        return false;
    }

    private void ReloadRushGameManager_OnGameStart(object sender, System.EventArgs e)
    {
        playerMovement.enabled = true;
    }

    public void EndMagazineState()
    {
        playerMovement.RemoveMovementFromEvent();
        Destroy(playerMovement);
        ReloadRushGameManager.Instance.SetGameState(ReloadRushGameManager.GameState.MagazineReloadTime);
        ReloadZone.Instance.ReloadGun();
        this.enabled = false;
    }


    public void SetUpMagazines()
    {
        DisableAllMagazines();

        magazineVisual[0] = magazineData.pistolMagazineVisuals[Inventory.Instance.GetPistolSelectedLevel()-1].magazineVisual;
        magazineVisual[1] = magazineData.smgMagazineVisuals[Inventory.Instance.GetSMGSelectedLevel() - 1].magazineVisual;
        magazineVisual[2] = magazineData.rifleMagazineVisuals[Inventory.Instance.GetRifleSelectedLevel() - 1].magazineVisual;
        magazineVisual[3] = magazineData.shotgunMagazineVisuals[Inventory.Instance.GetShotgunSelectedLevel() - 1].magazineVisual;
        magazineVisual[4] = magazineData.sniperMagazineVisuals[Inventory.Instance.GetSniperSelectedLevel() - 1].magazineVisual;
        magazineVisual[0].gameObject.SetActive(true);
    }

    public void DisableAllMagazines()
    {
        for (int i=0; i<magazineData.pistolMagazineVisuals.Length; i++)
        {
            magazineData.pistolMagazineVisuals[i].magazineVisual.gameObject.SetActive(false);
            magazineData.smgMagazineVisuals[i].magazineVisual.gameObject.SetActive(false);
            magazineData.rifleMagazineVisuals[i].magazineVisual.gameObject.SetActive(false);
            magazineData.shotgunMagazineVisuals[i].magazineVisual.gameObject.SetActive(false);
            magazineData.sniperMagazineVisuals[i].magazineVisual.gameObject.SetActive(false);

        }
    }
    void OnDrawGizmos()
    {
        Vector3 boxCastCenter = new Vector3(0, 0.5f, 0.1f);
        Vector3 boxCastHalfExtend = new Vector3(0.18f*2, 0.5f, .21f);
        Gizmos.DrawRay(transform.position + boxCastCenter, transform.forward * 1f);
        Gizmos.DrawWireCube(transform.position + boxCastCenter, boxCastHalfExtend * 2);
    }

}



[Serializable]
public struct MagazineData
{
    [Header("PISTOL")]
    public MagazineLevelData[] pistolMagazineVisuals;
    [Header("SMG")]
    public MagazineLevelData[] smgMagazineVisuals;
    [Header("RIFLE")]
    public MagazineLevelData[] rifleMagazineVisuals;
    [Header("SHOTGUN")]
    public MagazineLevelData[] shotgunMagazineVisuals;
    [Header("SNIPER")]
    public MagazineLevelData[] sniperMagazineVisuals;
}

[Serializable]
public struct MagazineLevelData
{

    public int levelOfMagazine;
    public MagazineVisual magazineVisual;
}
