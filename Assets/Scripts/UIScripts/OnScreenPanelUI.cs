using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OnScreenPanelUI : MonoBehaviour
{
    public static OnScreenPanelUI Instance { get; private set; }

    [SerializeField] private Button settingButton;
    [SerializeField] private Button reloadButton;
    [SerializeField] private Button reloadButtonViaAd;
    [SerializeField] private Button inventoryButton;
    [SerializeField] private Button MoneyByAdButton;

    [SerializeField] private GameObject settingPanelUI;
    [SerializeField] private GameObject InventoryPanelUI;
    [SerializeField] private GameObject canvasParent;

    private int costOfBullets = 100;
    [SerializeField] private TextMeshProUGUI costOfBulletText;

    [SerializeField] private GameObject bulletIncEffectPrefeb;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        costOfBullets = 100;
        costOfBulletText.text = costOfBullets.ToString();

        if (costOfBullets > ReloadRushGameManager.Instance.GetMoneyCount())
        {
            Debug.Log("Present The Add Button");
            ShowReloadViaAdButton();
        }

        settingButton.onClick.AddListener(() => 
        {
            Instantiate(settingPanelUI , canvasParent.transform);
            SoundManager.Instance.PlayClickSound();
        });

        reloadButton.onClick.AddListener(() =>
        {
            if (costOfBullets <= ReloadRushGameManager.Instance.GetMoneyCount())
            {
                ReloadRushGameManager.Instance.RemoveMoney(costOfBullets);
                
                costOfBullets *= 2;
                costOfBulletText.text = costOfBullets.ToString();

                AddRandomNumberOfBullet(1 , 5);

                if (costOfBullets > ReloadRushGameManager.Instance.GetMoneyCount())
                {
                    Debug.Log("Present The Add Button");
                    ShowReloadViaAdButton();
                }
                SoundManager.Instance.PlayClickSound();
            }
        });

        reloadButtonViaAd.onClick.AddListener(() =>
        {
            // Show Ad Here 
            //AdManager.Instance.ShowRewardedAd(() => AddRandomNumberOfBullet(5, 10));
            AdmobAdManager.Instance.rewardNum = 3;
            AdmobAdManager.Instance.Show_Reward_Admob();

            SoundManager.Instance.PlayClickSound();
        });

        inventoryButton.onClick.AddListener(() =>
        {
            ReloadRushGameManager.Instance.SetCameraToPriority(ReloadRushGameManager.CameraType.InventoryCamera);
            Instantiate(InventoryPanelUI , canvasParent.transform);
            SetActiveOnScreenUI(false);
            SoundManager.Instance.PlayClickSound();
        });

        MoneyByAdButton.onClick.AddListener(() => 
        {
            AdmobAdManager.Instance.rewardNum = 4;
            AdmobAdManager.Instance.Show_Reward_Admob();
        });

    }

    public void AddMoneyByRewardedAd()
    {
        ReloadRushGameManager.Instance.AddMoney(200);
    }

    void ShowReloadViaAdButton()
    {
        if (!reloadButtonViaAd.IsActive())
        {
            reloadButtonViaAd.gameObject.SetActive(true);
            reloadButtonViaAd.transform.DOLocalMoveX(335, 1);
        }
    }

    public void AddRandomNumberOfBullet(int randomMin , int randomMix)
    {
        int incBulletNumber = Random.Range(randomMin, randomMix);
        Debug.Log("random Inc is : " + incBulletNumber);
        for (int i = 0; i < incBulletNumber; i++)
        {   
            Magazine.Instance.AddBulletInMagazine();
        }
        Magazine.Instance.AddBulletViaButtonEffects(incBulletNumber);
    }

    public void SetActiveOnScreenUI(bool value)
    {
        this.gameObject.SetActive(value);
    }

    public Vector3 GetReloadButtonPosition()
    {
        Debug.Log("LOCAL pOSTIO :" + reloadButton.transform.position);
        return reloadButton.transform.position;
    }

    public void AddBulletShowEffect(int bulletCount)
    {
        GameObject addBulletEffect = Instantiate(bulletIncEffectPrefeb, GetReloadButtonPosition() + new Vector3(0, 150, 0), Quaternion.identity, this.transform);
        addBulletEffect.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "+" + bulletCount;
        addBulletEffect.transform.DOLocalMoveY(addBulletEffect.transform.localPosition.y + 150, 1f).OnComplete(() =>
        {
            addBulletEffect.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() => Destroy(addBulletEffect.gameObject));
        });
    }
}
