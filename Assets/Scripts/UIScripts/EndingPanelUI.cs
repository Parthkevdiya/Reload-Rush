using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndingPanelUI : MonoBehaviour
{
    public static EndingPanelUI Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI bulletsText;
    [SerializeField] private TextMeshProUGUI ad3xButtonText;

    [SerializeField] private Button ad3xButton;
    [SerializeField] private Button nextButton;

    private void Awake()
    {
        Instance = this;

        moneyText.text = ReloadRushGameManager.Instance.GetCollectedCash().ToString();
        bulletsText.text = ReloadRushGameManager.Instance.GetCollectedBullets().ToString();
        ad3xButtonText.text = (ReloadRushGameManager.Instance.GetCollectedCash() * 3).ToString();

        ReloadRushGameManager.Instance.SetCameraToPriority(ReloadRushGameManager.CameraType.InventoryCamera);

        ad3xButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayClickSound();

            // Pass This Action in RewardedAdFunction
            AdmobAdManager.Instance.rewardNum = 1;
            AdmobAdManager.Instance.Show_Reward_Admob();

            /*AdManager.Instance.ShowRewardedAd(() => 
            {
                ReloadRushGameManager.Instance.AddMoney(ReloadRushGameManager.Instance.GetCollectedCash() * 2);
                ReloadRushGameManager.Instance.AddXpInInventorySkins();
                Destroy(this.gameObject);
            });*/
            
        });

        nextButton.onClick.AddListener(() =>
        {
            //ReloadRushGameManager.Instance.AddMoney(ReloadRushGameManager.Instance.GetCollectedCash());
            Destroy(this.gameObject);
            ReloadRushGameManager.Instance.AddXpInInventorySkins();
            SoundManager.Instance.PlayClickSound();
        });
    }

    public void ConvertMoney3xAndDestroyPanel()
    {
        ReloadRushGameManager.Instance.AddMoney(ReloadRushGameManager.Instance.GetCollectedCash() * 2);
        ReloadRushGameManager.Instance.AddXpInInventorySkins();
        Destroy(this.gameObject);
    }
}
