using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class LevelSetup : MonoBehaviour
{
    public static LevelSetup Instance { get; private set; }

    [SerializeField] public LevelData[] levelDatas;

    [SerializeField] private GameObject[] bullets;
    [SerializeField] private GameObject bulletCanvas;
    [SerializeField] private GameObject enemyStand;
    [SerializeField] private GameObject fireRateCharger;
    [SerializeField] private GameObject damageCharger;
    [SerializeField] private GameObject moneyTowersPrefeb;

    [SerializeField] private Transform proceduralStuffParent;
    [SerializeField] private Transform enviroment;

    [SerializeField] private GameObject moneyTowerHold;
    [SerializeField] private GameObject spawnHandPrefeb;

    private void Awake()
    {
        Instance = this;
    }
    public void Start()
    {
        SetUpLevel();
    }

    public void SpawnHandMagazine()
    {
        Instantiate(spawnHandPrefeb , Vector3.zero + new Vector3(0 , 0.126f , 0) , Quaternion.identity);
    }

    public void CleanUp()
    {
        for (int i=0; i<proceduralStuffParent.childCount; i++)
        {
            Destroy(proceduralStuffParent.GetChild(i).gameObject);
        }
    }

    public void SetUpLevel()
    {
        Destroy(moneyTowerHold);
        moneyTowerHold = Instantiate(moneyTowersPrefeb, new Vector3(0, 0, 170f), Quaternion.identity, enviroment);

        for (int i=0; i<proceduralStuffParent.transform.childCount; i++)
        {
            Destroy(proceduralStuffParent.transform.GetChild(i));
        }

        int currentLevel = ReloadRushGameManager.Instance.GetCurrentLevel();

        if (currentLevel >= levelDatas.Length)
        {
            currentLevel = UnityEngine.Random.Range(1, levelDatas.Length - 1);
        }
        SetUpBullets(levelDatas[currentLevel - 1]);
        SetUpBulletCanvas(levelDatas[currentLevel - 1]);
        SetUpEnemyStand(levelDatas[currentLevel - 1]);
        SetUpFireRateCharger(levelDatas[currentLevel - 1]);
        SetUpDamageCharger(levelDatas[currentLevel - 1]);
    }

    private void SetUpBullets(LevelData levelData)
    {
        for (int i=0; i <levelData.reloadBulletData.Length; i++)
        {
            Instantiate(bullets[levelData.reloadBulletData[i].bulletCount-1] , levelData.reloadBulletData[i].bulletPos , Quaternion.identity , proceduralStuffParent);
        }
    }

    private void SetUpBulletCanvas(LevelData levelData)
    {
        for (int i=0; i<levelData.reloadBulletCanvasData.Length; i++)
        {
            BulletCanvas bulletCanvasS = Instantiate(bulletCanvas , levelData.reloadBulletCanvasData[i].bulletCanvasPos , Quaternion.identity , proceduralStuffParent).GetComponent<BulletCanvas>();
            bulletCanvasS.GetLeftM_Bullet().SetBuletCount(levelData.reloadBulletCanvasData[i].bulletValueL);
            bulletCanvasS.GetRightM_Bullet().SetBuletCount(levelData.reloadBulletCanvasData[i].bulletValueR);
        }
    }

    private void SetUpEnemyStand(LevelData levelData)
    {
        for (int i = 0; i < levelData.enemyData.Length; i++)
        {
            Enemy enemy = Instantiate(enemyStand, levelData.enemyData[i].enemyPos, Quaternion.identity, proceduralStuffParent).GetComponent<Enemy>();
            enemy.SetHealth(levelData.enemyData[i].enemyHealth);
        }
    }

    private void SetUpFireRateCharger(LevelData levelData)
    {
        for (int i=0; i< levelData.fireRateChargerData.Length; i++)
        {
            FireRateCharger fireRateChargerS = Instantiate(fireRateCharger , levelData.fireRateChargerData[i].fireRateChargerPos , Quaternion.identity , proceduralStuffParent).GetComponent<FireRateCharger>();
            fireRateChargerS.SetFireRate(levelData.fireRateChargerData[i].fireRatevalue);
        }
    }

    private void SetUpDamageCharger(LevelData levelData)
    {
        for (int i=0; i< levelData.damageChargerData.Length; i++)
        {
            DamageCharger damageChargerS = Instantiate(damageCharger, levelData.damageChargerData[i].damageCharherPos, Quaternion.identity, proceduralStuffParent).GetComponent<DamageCharger>();
            damageChargerS.SetDamage(levelData.damageChargerData[i].damageChargerValue);
        }
    }
}

[Serializable]
public struct LevelData
{
    public string levelNum;
    public ReloadBulletData[] reloadBulletData;
    public ReloadBulletCanvasData[] reloadBulletCanvasData;
    public EnemyData[] enemyData;
    public FireRateChargerData[] fireRateChargerData;
    public DamageChargerData[] damageChargerData;
}

[Serializable]
public struct ReloadBulletData
{
    public Vector3 bulletPos;
    [Range(1 , 5)] public int bulletCount;
}

[Serializable]
public struct ReloadBulletCanvasData
{
    public Vector3 bulletCanvasPos;
    [Range(1 , 15)] public int bulletValueL;
    [Range(1 , 15)] public int bulletValueR;
}

[Serializable]
public struct EnemyData
{
    public Vector3 enemyPos;
    [Range(1, 100)] public int enemyHealth;
}

[Serializable]
public struct FireRateChargerData
{
    public Vector3 fireRateChargerPos;
    [Range(-50, 50)] public int fireRatevalue;
}

[Serializable]
public struct DamageChargerData
{
    public Vector3 damageCharherPos;
    [Range(-50, 50)] public int damageChargerValue;
}


