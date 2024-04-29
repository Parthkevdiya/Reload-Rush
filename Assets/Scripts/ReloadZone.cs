using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class ReloadZone : MonoBehaviour
{
    public static ReloadZone Instance { get; private set; }

    [SerializeField] private GunData gunData;


    [Header("  ")]
    [SerializeField] private Gun[] allGuns;
    [SerializeField] private int reloadNumber = 0;
    [SerializeField] private int reloadCamNum = 1;
    private void Start()
    {
        Instance = this;
        SetUpGuns();
    }
    public void ReloadGun()
    {
        if (CompletedMagazine.Instance.GetGunMagazine(0) == null)
        {
            Debug.Log("Tumse Na Ho Payega");
            ReloadRushGameManager.Instance.SpawnTheFailPanelUI();
            return;
        }

        if (CompletedMagazine.Instance.GetGunMagazine(reloadNumber) != null)
        {
            if (reloadCamNum == 1)
            {
                Transform magazineTransform = CompletedMagazine.Instance.GetGunMagazine(reloadNumber).transform;
                ReloadRushGameManager.Instance.SetReload1CameraFollowAndLookTo(magazineTransform, magazineTransform);
                ReloadRushGameManager.Instance.SetCameraToPriority(ReloadRushGameManager.CameraType.Reload1Camera);
                reloadCamNum = 2;
            }
            else
            {
                Transform magazineTransform = CompletedMagazine.Instance.GetGunMagazine(reloadNumber).transform;
                ReloadRushGameManager.Instance.SetReload2CameraFollowAndLookTo(magazineTransform, magazineTransform);
                ReloadRushGameManager.Instance.SetCameraToPriority(ReloadRushGameManager.CameraType.Reload2Camera);
                reloadCamNum = 1;
            }

            allGuns[reloadNumber].ReloadMagazine(CompletedMagazine.Instance.GetGunMagazine(reloadNumber));
        }
        else
        {
            Debug.Log("All Guns Loaded");
            EndReloadState();
            return;
        }
    }

    public void EndReloadState()
    {
        ReloadRushGameManager.Instance.SetGameState(ReloadRushGameManager.GameState.GunRoundPowerUpRound);
        // SetUp For Next State
        for (int i=0; i<allGuns.Length; i++)
        {
            if (allGuns[i].GetIsGunActivated())
            {
                allGuns[i].transform.parent = PlayerGuns.Instance.transform;
                PlayerGuns.Instance.AddGunInGunHolder(allGuns[i]);
            }
        }
        
        ReloadRushGameManager.Instance.SetCameraToPriority(ReloadRushGameManager.CameraType.GameplayCamera);
        ReloadRushGameManager.Instance.SetGamePlayCameraFollowAndLookTo(PlayerGuns.Instance.transform , PlayerGuns.Instance.transform);
        
        Vector3 playerGunScale = new Vector3(0.6f , 0.6f , 0.6f);
        PlayerGuns.Instance.transform.DOScale(playerGunScale , 1f);

        PlayerGuns.Instance.transform.GetComponent<PlayerMovement>().enabled = true;
        PlayerGuns.Instance.StartGunsFire();
        PlayerGuns.Instance.SetGunPositionsAsGunUpgradeRound();


    }

    public void SetUpGuns()
    {
        DisableAllGuns();

        allGuns[0] = gunData.pistolGun[Inventory.Instance.GetPistolSelectedLevel() - 1].gun;
        allGuns[1] = gunData.smgGun[Inventory.Instance.GetSMGSelectedLevel() - 1].gun;
        allGuns[2] = gunData.rifleGun[Inventory.Instance.GetRifleSelectedLevel() - 1].gun;
        allGuns[3] = gunData.shotgunGun[Inventory.Instance.GetShotgunSelectedLevel() - 1].gun;
        allGuns[4] = gunData.sniperGun[Inventory.Instance.GetSniperSelectedLevel() - 1].gun;

        for (int i=0; i < allGuns.Length; i++)
        {
            allGuns[i].gameObject.SetActive(true);
        }
    }

    public void DisableAllGuns()
    {
        for (int i = 0; i < gunData.pistolGun.Length; i++)
        {
            gunData.pistolGun[i].gun.gameObject.SetActive(false);
            gunData.smgGun[i].gun.gameObject.SetActive(false);
            gunData.rifleGun[i].gun.gameObject.SetActive(false);
            gunData.shotgunGun[i].gun.gameObject.SetActive(false);
            gunData.sniperGun[i].gun.gameObject.SetActive(false);

        }
    }

    public void AddReloadNumber()
    {
        reloadNumber++;
    }
}

[Serializable]
public struct GunData
{
    [Header("PISTOL")]
    public GunLevelData[] pistolGun;
    [Header("SMG")]
    public GunLevelData[] smgGun;
    [Header("RIFLE")]
    public GunLevelData[] rifleGun;
    [Header("SHOTGUN")]
    public GunLevelData[] shotgunGun;
    [Header("SNIPER")]
    public GunLevelData[] sniperGun;
}

[Serializable]
public struct GunLevelData
{

    public int levelOfGun;
    public Gun gun;
}
