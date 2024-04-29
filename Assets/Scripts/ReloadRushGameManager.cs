
using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReloadRushGameManager : MonoBehaviour
{
    public const string VOLUME_STATE_PLAYER_PREFS = "VolumeState";
    public const string VIBRATION_STATE_PLAYER_PREFS = "VibrationState";
    public const string MONEY_PLAYER_PREFS = "_Money";
    public const string CURRENT_LEVEL_NUM_PLAYER_PREFS = "CurrentLevel";
    
    public static ReloadRushGameManager Instance { get; private set; }

    public event EventHandler OnGameStart;

    [SerializeField] private bool gameVolumeIs;
    [SerializeField] private bool gameVibrationIs;

    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI levelNumText;

    private int money_Count
    {
        get { return PlayerPrefs.GetInt(MONEY_PLAYER_PREFS, 100); }
        set { PlayerPrefs.SetInt(MONEY_PLAYER_PREFS, value); moneyText.text = money_Count.ToString("00"); }
    }

    private int currentLevel
    {
        get { return PlayerPrefs.GetInt(CURRENT_LEVEL_NUM_PLAYER_PREFS, 1); }
        set { PlayerPrefs.SetInt(CURRENT_LEVEL_NUM_PLAYER_PREFS, value); levelNumText.text = "Level " + currentLevel.ToString(); }
    }

    [SerializeField] private CinemachineVirtualCamera inventoryVirtualCamera;
    [SerializeField] private CinemachineVirtualCamera gameplayVirtualCamera;
    [SerializeField] private CinemachineVirtualCamera reloadGuns1VirtualCamera;
    [SerializeField] private CinemachineVirtualCamera reloadGuns2VirtualCamera;

    public enum CameraType
    {
        InventoryCamera,
        GameplayCamera,
        Reload1Camera,
        Reload2Camera,
    }

    public enum GameState
    {
        Idle,
        MagazineRound,
        MagazineReloadTime,
        GunRoundPowerUpRound,
        FinalRound,
    }
    [SerializeField] private GameState gameState = GameState.Idle;

    [Header("UI Stuff")]
    [SerializeField] private GameObject parentCanvas;
    [SerializeField] private GameObject failUIPanel;
    [SerializeField] private GameObject endingPanel;
    [SerializeField] private GameObject OnScreenUIPanel;

    [Header("Data For LevelCompleteUI")]
    [SerializeField] private int collectedBullets = 0;
    [SerializeField] private int collectedCash = 0;
    private void Awake()
    {
        Instance = this;

        gameVolumeIs = PlayerPrefs.GetInt(VOLUME_STATE_PLAYER_PREFS, 1) == 1;
        gameVibrationIs = PlayerPrefs.GetInt(VIBRATION_STATE_PLAYER_PREFS, 1) == 1;
        levelNumText.text = "Level " + currentLevel.ToString();
        moneyText.text = money_Count.ToString("00");
    }

    private void Start()
    {
        AdmobAdManager.Instance.Load_Banner_Admob();
    }

    private void Update()
    {
        /*if (gameState == GameState.Idle)
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnGameStart?.Invoke(this, EventArgs.Empty);
                gameState = GameState.MagazineRound;
            }
        }*/
    }

    public void StartTheGame()
    {
        if (gameState == GameState.Idle)
        {
            OnGameStart?.Invoke(this, EventArgs.Empty);
            gameState = GameState.MagazineRound;
            OnScreenPanelUI.Instance.SetActiveOnScreenUI(false);
        }
    }

    public void UpdateSettingState()
    {
        gameVolumeIs = PlayerPrefs.GetInt(VOLUME_STATE_PLAYER_PREFS, 1) == 1;
        gameVibrationIs = PlayerPrefs.GetInt(VIBRATION_STATE_PLAYER_PREFS, 1) == 1;
    }

    #region Variable Accsess Functions
    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public void SetNextLevel()
    {
        currentLevel ++;
    }

    public GameState GetGameState()
    {
        return gameState;
    }

    public void SetGameState(GameState state)
    {
        gameState = state;
    }

    public bool GetVolumeState()
    {
        return gameVolumeIs;
    }

    public bool GetVibrationState()
    {
        return gameVibrationIs;
    }

    public int GetMoneyCount()
    {
        return money_Count;
    }

    public void SetMoney(int value)
    {
        money_Count = value;
    }

    public void AddMoney(int value)
    {
        money_Count += value;
    }

    public void RemoveMoney(int value)
    {
        money_Count -= value;
    }

    public void AddCollectedBullets()
    {
        collectedBullets++;
    }

    public int GetCollectedBullets()
    {
        return collectedBullets;
    }

    public void AddCollectedCash(int incValue)
    {
        collectedCash += incValue;
    }

    public int GetCollectedCash()
    {
        return collectedCash;
    }

    

    #endregion

    #region Helper Fuctions

    public void SetCameraToPriority(CameraType cameraType)
    {
        inventoryVirtualCamera.Priority = 10;
        gameplayVirtualCamera.Priority = 10;
        reloadGuns1VirtualCamera.Priority = 10;

        switch (cameraType)
        {
            case CameraType.InventoryCamera:
                inventoryVirtualCamera.Priority = 11;
                break;
            case CameraType.GameplayCamera:
                gameplayVirtualCamera.Priority = 11;
                break;
            case CameraType.Reload1Camera:
                reloadGuns1VirtualCamera.Priority = 11;
                break;
            case CameraType.Reload2Camera:
                reloadGuns2VirtualCamera.Priority = 11;
                break;
            default:
                gameplayVirtualCamera.Priority = 11;
                break;
        }
    }

    public void SetReload1CameraFollowAndLookTo(Transform follow, Transform lookTo)
    {
        reloadGuns1VirtualCamera.Follow = follow;
        reloadGuns1VirtualCamera.LookAt = lookTo;
    }

    public void SetReload2CameraFollowAndLookTo(Transform follow, Transform lookTo)
    {
        reloadGuns2VirtualCamera.Follow = follow;
        reloadGuns2VirtualCamera.LookAt = lookTo;
    }

    public void SetGamePlayCameraFollowAndLookTo(Transform follow , Transform lookTo)
    {
        gameplayVirtualCamera.Follow = follow;
        gameplayVirtualCamera.LookAt = lookTo;
    }

    public void AddXpInInventorySkins()
    {
        Debug.Log("Showing And Adding Xp");
        
        Inventory.Instance.SetinventoryXposition(0);
        Inventory.Instance.SetActiveStandGunVisual(PlayerGuns.Instance.GetActivatedGunsCount());
        Inventory.Instance.SetUpGunUpgrade();

    }

    private void OnApplicationQuit()
    {
        
    }

    public void SpawnTheFailPanelUI()
    {
        Instantiate(failUIPanel , parentCanvas.transform);
    }

    public void SpawnTheEndingPanelUI()
    {
        SoundManager.Instance.PlayHappySound();
        Instantiate(endingPanel , parentCanvas.transform);
    }

    public void ActiveOnScreenUIPanel()
    {
        OnScreenUIPanel.SetActive(true);
    }

    #endregion
}
