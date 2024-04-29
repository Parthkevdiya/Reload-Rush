using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanelUI : MonoBehaviour
{
    [SerializeField] private Button volumeButton;
    [SerializeField] private Button vibrationButton;
    [SerializeField] private Button closeButton;

    [SerializeField] private Image volumeImage;
    [SerializeField] private Image vibrationImage;

    [SerializeField] private Sprite OnSprite;
    [SerializeField] private Sprite OffSprite;

    private void Start()
    {
        SetUpVisual();

        volumeButton.onClick.AddListener(() => 
        {
            if (ReloadRushGameManager.Instance.GetVolumeState())
            {
                PlayerPrefs.SetInt(ReloadRushGameManager.VOLUME_STATE_PLAYER_PREFS, 0);
            }
            else
            {
                PlayerPrefs.SetInt(ReloadRushGameManager.VOLUME_STATE_PLAYER_PREFS, 1);
            }
            ReloadRushGameManager.Instance.UpdateSettingState();
            SetUpVisual();
            SoundManager.Instance.PlayClickSound();
        });

        vibrationButton.onClick.AddListener(() =>
        {
            if (ReloadRushGameManager.Instance.GetVibrationState())
            {
                PlayerPrefs.SetInt(ReloadRushGameManager.VIBRATION_STATE_PLAYER_PREFS, 0);
            }
            else
            {
                PlayerPrefs.SetInt(ReloadRushGameManager.VIBRATION_STATE_PLAYER_PREFS, 1);
            }
            ReloadRushGameManager.Instance.UpdateSettingState();
            SetUpVisual();
            SoundManager.Instance.PlayClickSound();
        });

        closeButton.onClick.AddListener(() =>
        {
            Destroy(this.gameObject);
            SoundManager.Instance.PlayClickSound();
        });
    }

    private void SetUpVisual()
    {
        
        if (ReloadRushGameManager.Instance.GetVolumeState())
        {
            volumeImage.sprite = OnSprite;
        }
        else
        {
            volumeImage.sprite = OffSprite;
        }

        if (ReloadRushGameManager.Instance.GetVibrationState())
        {
            vibrationImage.sprite = OnSprite;
        }
        else
        {
            vibrationImage.sprite = OffSprite;
        }
    }
}
