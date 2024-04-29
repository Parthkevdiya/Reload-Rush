using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class Inventory : MonoBehaviour
{
    public const string PISTOL_CURRENT_LEVEL_PLAYER_PREFS = "PistolCurrentLevel";
    public const string PISTOL_CURRENT_LEVEL_PROGRESS_PLAYER_PREFS = "PistolCurrentLevelProgress";
    public const string PISTOL_SELECTED_LEVEL_PLAYER_PREFS = "PistolSelectedLevel";

    public const string SMG_CURRENT_LEVEL_PLAYER_PREFS = "SMGCurrentLevel";
    public const string SMG_CURRENT_LEVEL_PROGRESS_PLAYER_PREFS = "SMGCurrentLevelProgress";
    public const string SMG_SELECTED_LEVEL_PLAYER_PREFS = "SMGSelectedLevel";

    public const string RIFLE_CURRENT_LEVEL_PLAYER_PREFS = "RifleCurrentLevel";
    public const string RIFLE_CURRENT_LEVEL_PROGRESS_PLAYER_PREFS = "RifleCurrentLevelProgress";
    public const string RIFLE_SELECTED_LEVEL_PLAYER_PREFS = "RifleSelectedLevel";

    public const string SHOTGUN_CURRENT_LEVEL_PLAYER_PREFS = "ShotgunCurrentLevel";
    public const string SHOTGUN_CURRENT_LEVEL_PROGRESS_PLAYER_PREFS = "ShotgunCurrentLevelProgress";
    public const string SHOTGUN_SELECTED_LEVEL_PLAYER_PREFS = "ShotgunSelectedLevel";

    public const string SNIPER_CURRENT_LEVEL_PLAYER_PREFS = "SniperCurrentLevel";
    public const string SNIPER_CURRENT_LEVEL_PROGRESS_PLAYER_PREFS = "SniperCurrentLevelProgress";
    public const string SNIPER_SELECTED_LEVEL_PLAYER_PREFS = "SniperSelectedLevel";

    public static Inventory Instance { get; private set; }

    [SerializeField] private Material allGunMaterial;
    [SerializeField] private Material level5gunMaterial;
    [SerializeField] private Material completedGaugeMaterial;

    [SerializeField] private Transform inventoryMovePivot;
    [SerializeField, Range(0, 1)] private float inventoryXposition;
    [SerializeField, Range(0, 1)] private float inventoryXpositionSmooth;

    [SerializeField] private Transform[] selectedGunCheckMark;
    [SerializeField] private Vector3[] checkMarkPositionsY;

    [SerializeField] private StandGunTypeData[] standGunTypeDatas;

    [SerializeField] public GameObject getGunPartical;
    [SerializeField] public GameObject getGunByAdGameObjectButton;
    [SerializeField] public GameObject getGunEffect;
    public float inventoryXpostionAC 
    {
        get { return inventoryXposition; } 
        set 
        {
            inventoryXposition = value;
            float PivotPosX = Mathf.Lerp(0, 60, inventoryXposition);
            inventoryMovePivot.transform.localPosition = new Vector3(-PivotPosX, 0, 0);
        }
    }

    #region Pistol Accessor
    private int pistolCurrentLevel
    {
        get { return PlayerPrefs.GetInt(PISTOL_CURRENT_LEVEL_PLAYER_PREFS , 1); }
        set { PlayerPrefs.SetInt(PISTOL_CURRENT_LEVEL_PLAYER_PREFS, value); }
    }

    private float pistolCurrentLevelProgress
    {
        get { return PlayerPrefs.GetFloat(PISTOL_CURRENT_LEVEL_PROGRESS_PLAYER_PREFS, 0); }
        set { value = Mathf.Clamp(value , 0 , 100); PlayerPrefs.SetFloat(PISTOL_CURRENT_LEVEL_PROGRESS_PLAYER_PREFS , value); }
    }

    private int pistolSelectedLevel
    {
        get { return PlayerPrefs.GetInt(PISTOL_SELECTED_LEVEL_PLAYER_PREFS, 1); }
        set { PlayerPrefs.SetInt(PISTOL_SELECTED_LEVEL_PLAYER_PREFS, value); }
    }

    #endregion

    #region SMG Accessor
    private int smgCurrentLevel
    {
        get { return PlayerPrefs.GetInt(SMG_CURRENT_LEVEL_PLAYER_PREFS, 1); }
        set { PlayerPrefs.SetInt(SMG_CURRENT_LEVEL_PLAYER_PREFS, value); }
    }

    private float smgCurrentLevelProgress
    {
        get { return PlayerPrefs.GetFloat(SMG_CURRENT_LEVEL_PROGRESS_PLAYER_PREFS, 0); }
        set { value = Mathf.Clamp(value, 0, 100); PlayerPrefs.SetFloat(SMG_CURRENT_LEVEL_PROGRESS_PLAYER_PREFS, value);  }
    }

    private int smgSelectedLevel
    {
        get { return PlayerPrefs.GetInt(SMG_SELECTED_LEVEL_PLAYER_PREFS, 1); }
        set { PlayerPrefs.SetInt(SMG_SELECTED_LEVEL_PLAYER_PREFS, value); }
    }

    #endregion

    #region Rifle Accessor
    private int rifleCurrentLevel
    {
        get { return PlayerPrefs.GetInt(RIFLE_CURRENT_LEVEL_PLAYER_PREFS, 1); }
        set { PlayerPrefs.SetInt(RIFLE_CURRENT_LEVEL_PLAYER_PREFS, value); }
    }

    private float rifleCurrentLevelProgress
    {
        get { return PlayerPrefs.GetFloat(RIFLE_CURRENT_LEVEL_PROGRESS_PLAYER_PREFS, 0); }
        set { value = Mathf.Clamp(value, 0, 100); PlayerPrefs.SetFloat(RIFLE_CURRENT_LEVEL_PROGRESS_PLAYER_PREFS, value); }
    }

    private int rifleSelectedLevel
    {
        get { return PlayerPrefs.GetInt(RIFLE_SELECTED_LEVEL_PLAYER_PREFS, 1); }
        set { PlayerPrefs.SetInt(RIFLE_SELECTED_LEVEL_PLAYER_PREFS, value); }
    }
    #endregion

    #region Shotgun Accessor
    private int shotgunCurrentLevel
    {
        get { return PlayerPrefs.GetInt(SHOTGUN_CURRENT_LEVEL_PLAYER_PREFS, 1); }
        set { PlayerPrefs.SetInt(SHOTGUN_CURRENT_LEVEL_PLAYER_PREFS, value); }
    }

    private float shotgunCurrentLevelProgress
    {
        get { return PlayerPrefs.GetFloat(SHOTGUN_CURRENT_LEVEL_PROGRESS_PLAYER_PREFS, 0); }
        set { value = Mathf.Clamp(value, 0, 100); PlayerPrefs.SetFloat(SHOTGUN_CURRENT_LEVEL_PROGRESS_PLAYER_PREFS, value); }
    }

    private int shotgunSelectedLevel
    {
        get { return PlayerPrefs.GetInt(SHOTGUN_SELECTED_LEVEL_PLAYER_PREFS, 1); }
        set { PlayerPrefs.SetInt(SHOTGUN_SELECTED_LEVEL_PLAYER_PREFS, value); }
    }

    #endregion

    #region Sniper Accessor
    private int sniperCurrentLevel
    {
        get { return PlayerPrefs.GetInt(SNIPER_CURRENT_LEVEL_PLAYER_PREFS, 1); }
        set { PlayerPrefs.SetInt(SNIPER_CURRENT_LEVEL_PLAYER_PREFS, value); }
    }

    private float sniperCurrentLevelProgress
    {
        get { return PlayerPrefs.GetFloat(SNIPER_CURRENT_LEVEL_PROGRESS_PLAYER_PREFS, 0); }
        set { value = Mathf.Clamp(value, 0, 100); PlayerPrefs.SetFloat(SNIPER_CURRENT_LEVEL_PROGRESS_PLAYER_PREFS, value); }
    }

    private int sniperSelectedLevel
    {
        get { return PlayerPrefs.GetInt(SNIPER_SELECTED_LEVEL_PLAYER_PREFS, 1); }
        set { PlayerPrefs.SetInt(SNIPER_SELECTED_LEVEL_PLAYER_PREFS, value); }
    }
    #endregion

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetUpSelectedCheckMark();
    }

    private void Update()
    {
        /*float PivotPosX = Mathf.Lerp(0, 60, inventoryXposition);
        inventoryMovePivot.transform.localPosition = new Vector3(-PivotPosX, 0, 0);*/

        if (inventoryXposition != inventoryXpositionSmooth)
        {
            inventoryXpostionAC = Mathf.Lerp(inventoryXposition , inventoryXpositionSmooth , Time.deltaTime*5);
        }
    }

    #region Accessors Access Functions

    #region Pistol Accessor
    public int GetPistolCurrentLevel()
    {
        return pistolCurrentLevel;
    }

    public void SetPistolCurrentLevel(int currentLevel)
    {
        pistolCurrentLevel = currentLevel;
    }

    public float GetPistolCurrentLevelProgress()
    {
        return pistolCurrentLevelProgress;
    }

    public void SetPistolCurrentLevelProgress(float progressLevel)
    {
        pistolCurrentLevelProgress = progressLevel;
    }
    public int GetPistolSelectedLevel()
    {
        return pistolSelectedLevel;
    }

    public void SetPistolSelectedLevel(int selectedLevel)
    {
        pistolSelectedLevel = selectedLevel;
    }

    #endregion
    #region SMG Accessor
    public int GetSMGCurrentLevel()
    {
        return smgCurrentLevel;
    }

    public void SetSMGCurrentLevel(int currentLevel)
    {
        smgCurrentLevel = currentLevel;
    }

    public float GetSMGCurrentLevelProgress()
    {
        return smgCurrentLevelProgress;
    }

    public void SetSMGCurrentLevelProgress(float progressLevel)
    {
        smgCurrentLevelProgress = progressLevel;
    }
    public int GetSMGSelectedLevel()
    {
        return smgSelectedLevel;
    }

    public void SetSMGSelectedLevel(int selectedLevel)
    {
        smgSelectedLevel = selectedLevel;
    }
    #endregion
    #region Rifle Accessor

    public int GetRifleCurrentLevel()
    {
        return rifleCurrentLevel;
    }

    public void SetRifleCurrentLevel(int currentLevel)
    {
        rifleCurrentLevel = currentLevel;
    }

    public float GetRifleCurrentLevelProgress()
    {
        return rifleCurrentLevelProgress;
    }

    public void SetRifleCurrentLevelProgress(float progressLevel)
    {
        rifleCurrentLevelProgress = progressLevel;
    }
    public int GetRifleSelectedLevel()
    {
        return rifleSelectedLevel;
    }

    public void SetRifleSelectedLevel(int selectedLevel)
    {
        rifleSelectedLevel = selectedLevel;
    }

    #endregion
    #region Shotgun Accessor

    public int GetShotgunCurrentLevel()
    {
        return shotgunCurrentLevel;
    }

    public void SetShotgunCurrentLevel(int currentLevel)
    {
        shotgunCurrentLevel = currentLevel;
    }

    public float GetShotgunCurrentLevelProgress()
    {
        return shotgunCurrentLevelProgress;
    }

    public void SetShotgunCurrentLevelProgress(float progressLevel)
    {
        shotgunCurrentLevelProgress = progressLevel;
    }
    public int GetShotgunSelectedLevel()
    {
        return shotgunSelectedLevel;
    }

    public void SetShotgunSelectedLevel(int selectedLevel)
    {
        shotgunSelectedLevel = selectedLevel;
    }

    #endregion
    #region Sniper Accessor

    public int GetSniperCurrentLevel()
    {
        return sniperCurrentLevel;
    }

    public void SetSniperCurrentLevel(int currentLevel)
    {
        sniperCurrentLevel = currentLevel;
    }

    public float GetSniperCurrentLevelProgress()
    {
        return sniperCurrentLevelProgress;
    }

    public void SetSniperCurrentLevelProgress(float progressLevel)
    {
        sniperCurrentLevelProgress = progressLevel;
    }
    public int GetSniperSelectedLevel()
    {
        return sniperSelectedLevel;
    }

    public void SetSniperSelectedLevel(int selectedLevel)
    {
        sniperSelectedLevel = selectedLevel;
    }

    #endregion

    #endregion

    #region Variable Access Functions
    public Material GetAllGunsMaterial()
    {
        return allGunMaterial;
    }

    public Material GetLevel5GunMaterial()
    {
        return level5gunMaterial;
    }

    public Material GetCompletedGaugeMaterial()
    {
        return completedGaugeMaterial;
    }

    public float GetinventoryXpositionSmooth()
    {
        return inventoryXpositionSmooth;
    }

    #endregion

    #region Helper Functions

    public void UpdateAllStandGunSetUp()
    {
        for (int i=0; i< standGunTypeDatas.Length; i++)
        {
            for (int j=0; j< standGunTypeDatas[i].standGunData.Length; j++)
            {
                standGunTypeDatas[i].standGunData[j].standGun.SetUp();
            }
            
        }
    }

    public void SetinventoryXposition(float value)
    {
        inventoryXpositionSmooth = value;
    }

    public void InventoryLeftSwipe()
    {
        inventoryXpositionSmooth -= 0.25f;
        
    }

    public void InventoryRightSwipe()
    {
        inventoryXpositionSmooth += 0.25f;
    }

    public void SetUpSelectedCheckMark()
    {
        selectedGunCheckMark[0].localPosition = checkMarkPositionsY[GetPistolSelectedLevel()-1];
        selectedGunCheckMark[1].localPosition = checkMarkPositionsY[GetSMGSelectedLevel()-1];
        selectedGunCheckMark[2].localPosition = checkMarkPositionsY[GetRifleSelectedLevel() - 1];
        selectedGunCheckMark[3].localPosition = checkMarkPositionsY[GetShotgunSelectedLevel() - 1];
        selectedGunCheckMark[4].localPosition = checkMarkPositionsY[GetSniperSelectedLevel() - 1];
    }

    public void SetActiveStandGunVisual(int visualCount)
    {
        for (int i =0; i<visualCount; i++)
        {
            switch (i)
            {
                case 0:
                    standGunTypeDatas[i].standGunData[GetPistolSelectedLevel() - 1].standGun.SetActiveUpgradeGunVisual();
                    break;

                case 1:
                    standGunTypeDatas[i].standGunData[GetSMGSelectedLevel() - 1].standGun.SetActiveUpgradeGunVisual();
                    break;

                case 2:
                    standGunTypeDatas[i].standGunData[GetRifleSelectedLevel() - 1].standGun.SetActiveUpgradeGunVisual();
                    break;

                case 3:
                    standGunTypeDatas[i].standGunData[GetShotgunSelectedLevel() - 1].standGun.SetActiveUpgradeGunVisual();
                    break;

                case 4:
                    standGunTypeDatas[i].standGunData[GetSniperSelectedLevel() - 1].standGun.SetActiveUpgradeGunVisual();
                    break;
            }
            
        }
    }

    int i = 0;
    public void SetUpGunUpgrade()
    {
        if (i < PlayerGuns.Instance.GetActivatedGunsCount())
        {
            switch (i)
            {
                case 0:

                    SetinventoryXposition(0);
                    standGunTypeDatas[i].standGunData[GetPistolSelectedLevel() - 1].standGun.SetUpgradeVisualToWork();

                    i++;
                    break;

                case 1:
                    SetinventoryXposition(.25f);
                    standGunTypeDatas[i].standGunData[GetSMGSelectedLevel() - 1].standGun.SetUpgradeVisualToWork();
                    i++;
                    break;

                case 2:
                    SetinventoryXposition(.5f);
                    standGunTypeDatas[i].standGunData[GetRifleSelectedLevel() - 1].standGun.SetUpgradeVisualToWork();
                    i++;
                    break;

                case 3:
                    SetinventoryXposition(.75f);
                    standGunTypeDatas[i].standGunData[GetShotgunSelectedLevel() - 1].standGun.SetUpgradeVisualToWork();
                    i++;
                    break;

                case 4:
                    SetinventoryXposition(1f);
                    standGunTypeDatas[i].standGunData[GetSniperSelectedLevel() - 1].standGun.SetUpgradeVisualToWork();
                    i++;
                    break;
            }
        }
        else
        {
            StartCoroutine(waitAndLoadScene());
        }
    }

    public IEnumerator waitAndLoadScene()
    {
        yield return new WaitForSeconds(0.5f);
        ReloadRushGameManager.Instance.SetGamePlayCameraFollowAndLookTo(Magazine.Instance.transform, Magazine.Instance.transform);
        ReloadRushGameManager.Instance.SetCameraToPriority(ReloadRushGameManager.CameraType.GameplayCamera);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion
}

[Serializable]
public struct StandGunTypeData
{
    public string typeName;
    public StandGunData[] standGunData;
}

[Serializable]
public struct StandGunData
{
    public int level;
    public StandGun standGun;
}
