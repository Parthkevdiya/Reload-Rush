using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance { get; private set; }

    [SerializeField] private Button leftArrowButton;
    [SerializeField] private Button rightArrowButton;
    [SerializeField] private Button back;

    [SerializeField] private StandGun standGunReffForReward;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        leftArrowButton.onClick.AddListener(() => 
        {
            Inventory.Instance.InventoryLeftSwipe();
            rightArrowButton.gameObject.SetActive(true);
            if (Inventory.Instance.GetinventoryXpositionSmooth() == 0)
            {
                leftArrowButton.gameObject.SetActive(false);
            }
            SoundManager.Instance.PlayClickSound();
        });

        rightArrowButton.onClick.AddListener(() => 
        {
            Inventory.Instance.InventoryRightSwipe();
            leftArrowButton.gameObject.SetActive(true);
            if (Inventory.Instance.GetinventoryXpositionSmooth() == 1)
            {
                rightArrowButton.gameObject.SetActive(false);
            }
            SoundManager.Instance.PlayClickSound();
        });

        back.onClick.AddListener(() => 
        {
            ReloadRushGameManager.Instance.SetCameraToPriority(ReloadRushGameManager.CameraType.GameplayCamera);
            OnScreenPanelUI.Instance.SetActiveOnScreenUI(true);
            Destroy(this.gameObject);
            SoundManager.Instance.PlayClickSound();
        });
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hitInfo, 100))
            {
                Vector3 POS = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y , hitInfo.distance - 1));

                if (Physics.Raycast(POS, Camera.main.transform.forward, out RaycastHit hitInfo1, 100))
                {
                    Debug.Log("Inventory" + hitInfo1.transform.name);
                    if (hitInfo1.transform.TryGetComponent<StandGun>(out StandGun standGun))
                    {
                        standGunReffForReward = standGun;
                        if (standGun.GetUnlockStatus())
                        {
                            FindAndSetCurrentSelectedGun(standGun);
                            Magazine.Instance.SetUpMagazines();
                            ReloadZone.Instance.SetUpGuns();
                        }
                        else if (standGun.GetReadyToUnlockStatus())
                        {
                            AdmobAdManager.Instance.rewardNum = 2;
                            AdmobAdManager.Instance.Show_Reward_Admob();
                            //AdManager.Instance.ShowRewardedAd(() => standGun.SetUpUpgrade());
                        }
                    }
                }
            }
        }
    }

    public void UnlockStanGunReward()
    {
        standGunReffForReward.SetUpUpgrade();
    }

    public void FindAndSetCurrentSelectedGun(StandGun standGun)
    {
        

        switch (standGun.GetGunType())
        {
            case StandGun.GunName.Pistol:
                Inventory.Instance.SetPistolSelectedLevel(standGun.GetGunLevel());
                break;

            case StandGun.GunName.SMG:
                Inventory.Instance.SetSMGSelectedLevel(standGun.GetGunLevel());
                break;

            case StandGun.GunName.Rifle:
                Inventory.Instance.SetRifleSelectedLevel(standGun.GetGunLevel());
                break;

            case StandGun.GunName.ShotGun:
                Inventory.Instance.SetShotgunSelectedLevel(standGun.GetGunLevel());
                break;

            case StandGun.GunName.Sniper:
                Inventory.Instance.SetSniperSelectedLevel(standGun.GetGunLevel());
                break;
        }

        Inventory.Instance.SetUpSelectedCheckMark();
    }
}
