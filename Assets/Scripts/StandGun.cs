using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class StandGun : MonoBehaviour
{
    public enum GunName
    {
        Pistol,
        SMG,
        Rifle,
        ShotGun,
        Sniper,
    }

    [SerializeField] private GunName name;
    [SerializeField] private int level;
    [SerializeField] private bool unLocked = false;
    [SerializeField] private bool readyToUnlock = false;

    [SerializeField] private MeshRenderer gunMeshRenderer;
    [SerializeField] private MeshRenderer magazineMeshRenderer;
    [SerializeField] private MeshRenderer gaugeMeshRenderer;

    [SerializeField] private Transform gauge;
    [SerializeField] private TextMeshPro stateText;

    [SerializeField] private GameObject standGunUpgradeVisual;

    [SerializeField] private GameObject getGunAd;
    [SerializeField] private GameObject getGunAdEffect;

    private void Start()
    {
        SetUp();
    }

    public void SetActiveUpgradeGunVisual()
    {
        standGunUpgradeVisual.SetActive(true);
    }

    public void SetUpgradeVisualToWork()
    {
        standGunUpgradeVisual.transform.parent = this.transform;
        standGunUpgradeVisual.transform.DOLocalMove(gunMeshRenderer.transform.localPosition , 1f);
        standGunUpgradeVisual.transform.DORotate(new Vector3(0 , 90 , 0) , 1f).OnComplete(() =>
        {
            Instantiate(Inventory.Instance.getGunPartical , transform.position , Quaternion.identity);
            Destroy(standGunUpgradeVisual);
            SoundManager.Instance.PlayReloadSound();
            Inventory.Instance.UpdateAllStandGunSetUp();

            StartCoroutine(WaitAndRecallGunUpgradeFunction());
        });

        switch (name)
        {
            case GunName.Pistol:
                Inventory.Instance.SetPistolCurrentLevelProgress( Inventory.Instance.GetPistolCurrentLevelProgress() + Random.Range(0 , 25));
                break;

            case GunName.SMG:
                Inventory.Instance.SetSMGCurrentLevelProgress(Inventory.Instance.GetSMGCurrentLevelProgress() + Random.Range(0, 20));
                break;

            case GunName.Rifle:
                Inventory.Instance.SetRifleCurrentLevelProgress(Inventory.Instance.GetRifleCurrentLevelProgress() + Random.Range(0, 15));
                break;

            case GunName.ShotGun:
                Inventory.Instance.SetShotgunCurrentLevelProgress(Inventory.Instance.GetShotgunCurrentLevelProgress() + Random.Range(0, 10));
                break;

            case GunName.Sniper:
                Inventory.Instance.SetSniperCurrentLevelProgress(Inventory.Instance.GetSniperCurrentLevelProgress() + Random.Range(0, 5));
                break;
        }

        SetUp();
    }

    public IEnumerator WaitAndRecallGunUpgradeFunction()
    {
        yield return new WaitForSeconds(0.5f);
        Inventory.Instance.SetUpGunUpgrade();
    }

    public void SetUp()
    {
        switch (name)
        {
            case GunName.Pistol:
                PistolSetUp();
                break;

            case GunName.SMG:
                SmgSetUp();
                break;

            case GunName.Rifle:
                RifleSetUp();
                break;

            case GunName.ShotGun:
                ShotGunSetup();
                break;

            case GunName.Sniper:
                SniperSetUp();
                break;
        }
    }

    public void PistolSetUp()
    {
        if (level < Inventory.Instance.GetPistolCurrentLevel())
        {
            SetUpGunMaterial();
            SetUpCompletedGaugeMaterial();
            SetTextToComplete();
            unLocked = true;
        }
        else if (level == Inventory.Instance.GetPistolCurrentLevel())
        {
            SetUpGunMaterial();
            float gaugeScaleX = Inventory.Instance.GetPistolCurrentLevelProgress() / 100;
            SetGaugeLevel(gaugeScaleX , 0.5f);
            stateText.text = Inventory.Instance.GetPistolCurrentLevelProgress() + "%";
            if (Inventory.Instance.GetPistolCurrentLevelProgress() == 100)
            {
                SetTextToComplete();
            }
            unLocked = true;
        }

        if (level == Inventory.Instance.GetPistolCurrentLevel()+1 && Inventory.Instance.GetPistolCurrentLevelProgress() == 100)
        {
            readyToUnlock = true;
            getGunAd = Instantiate(Inventory.Instance.getGunByAdGameObjectButton , this.transform);
            getGunAd.transform.localPosition = new Vector3(0, -0.019f, 0.187f);
            getGunAdEffect = Instantiate(Inventory.Instance.getGunEffect , this.transform);
            getGunAdEffect.transform.localPosition = new Vector3(0 , 1.2f, 0);
        }
    }

    public void SmgSetUp()
    {
        if (level < Inventory.Instance.GetSMGCurrentLevel())
        {
            SetUpGunMaterial();
            SetUpCompletedGaugeMaterial();
            SetTextToComplete();
        }
        else if (level == Inventory.Instance.GetSMGCurrentLevel())
        {
            SetUpGunMaterial();
            float gaugeScaleX = Inventory.Instance.GetSMGCurrentLevelProgress() / 100;
            SetGaugeLevel(gaugeScaleX, 0.5f);
            stateText.text = Inventory.Instance.GetSMGCurrentLevelProgress() + "%";
            if (Inventory.Instance.GetPistolCurrentLevelProgress() == 100)
            {
                SetTextToComplete();
            }
        }

        if (level == Inventory.Instance.GetSMGCurrentLevel() + 1 && Inventory.Instance.GetSMGCurrentLevelProgress() == 100)
        {
            readyToUnlock = true;
            getGunAd = Instantiate(Inventory.Instance.getGunByAdGameObjectButton, this.transform);
            getGunAd.transform.localPosition = new Vector3(0, -0.019f, 0.187f);
            getGunAdEffect = Instantiate(Inventory.Instance.getGunEffect, this.transform);
            getGunAdEffect.transform.localPosition = new Vector3(0, 1.2f, 0);
        }
    }

    public void RifleSetUp() 
    {
        if (level < Inventory.Instance.GetRifleCurrentLevel())
        {
            SetUpGunMaterial();
            SetUpCompletedGaugeMaterial();
            SetTextToComplete();
        }
        else if (level == Inventory.Instance.GetRifleCurrentLevel())
        {
            SetUpGunMaterial();
            float gaugeScaleX = Inventory.Instance.GetRifleCurrentLevelProgress() / 100;
            SetGaugeLevel(gaugeScaleX, 0.5f);
            stateText.text = Inventory.Instance.GetRifleCurrentLevelProgress() + "%";
            if (Inventory.Instance.GetPistolCurrentLevelProgress() == 100)
            {
                SetTextToComplete();
            }
        }

        if (level == Inventory.Instance.GetRifleCurrentLevel() + 1 && Inventory.Instance.GetRifleCurrentLevelProgress() == 100)
        {
            readyToUnlock = true;
            getGunAd = Instantiate(Inventory.Instance.getGunByAdGameObjectButton, this.transform);
            getGunAd.transform.localPosition = new Vector3(0, -0.019f, 0.187f);
            getGunAdEffect = Instantiate(Inventory.Instance.getGunEffect, this.transform);
            getGunAdEffect.transform.localPosition = new Vector3(0, 1.2f, 0);
        }
    }

    public void ShotGunSetup()
    {
        if (level < Inventory.Instance.GetShotgunCurrentLevel())
        {
            SetUpGunMaterial();
            SetUpCompletedGaugeMaterial();
            SetTextToComplete();
        }
        else if (level == Inventory.Instance.GetShotgunCurrentLevel())
        {
            SetUpGunMaterial();
            float gaugeScaleX = Inventory.Instance.GetShotgunCurrentLevelProgress() / 100;
            SetGaugeLevel(gaugeScaleX, 0.5f);
            stateText.text = Inventory.Instance.GetShotgunCurrentLevelProgress() + "%";
            if (Inventory.Instance.GetPistolCurrentLevelProgress() == 100)
            {
                SetTextToComplete();
            }
        }

        if (level == Inventory.Instance.GetShotgunCurrentLevel() + 1 && Inventory.Instance.GetShotgunCurrentLevelProgress() == 100)
        {
            readyToUnlock = true;
            getGunAd = Instantiate(Inventory.Instance.getGunByAdGameObjectButton, this.transform);
            getGunAd.transform.localPosition = new Vector3(0, -0.019f, 0.187f);
            getGunAdEffect = Instantiate(Inventory.Instance.getGunEffect, this.transform);
            getGunAdEffect.transform.localPosition = new Vector3(0, 1.2f, 0);
        }
    }

    public void SniperSetUp() 
    {
        if (level < Inventory.Instance.GetSniperCurrentLevel())
        {
            SetUpGunMaterial();
            SetUpCompletedGaugeMaterial();
            SetTextToComplete();
        }
        else if (level == Inventory.Instance.GetSniperCurrentLevel())
        {
            SetUpGunMaterial();
            float gaugeScaleX = Inventory.Instance.GetSniperCurrentLevelProgress() / 100;
            SetGaugeLevel(gaugeScaleX, 0.5f);
            stateText.text = Inventory.Instance.GetSniperCurrentLevelProgress() + "%";
            if (Inventory.Instance.GetPistolCurrentLevelProgress() == 100)
            {
                SetTextToComplete();
            }
        }

        if (level == Inventory.Instance.GetSniperCurrentLevel() + 1 && Inventory.Instance.GetSniperCurrentLevelProgress() == 100)
        {
            readyToUnlock = true;
            getGunAd = Instantiate(Inventory.Instance.getGunByAdGameObjectButton, this.transform);
            getGunAd.transform.localPosition = new Vector3(0, -0.019f, 0.187f);
            getGunAdEffect = Instantiate(Inventory.Instance.getGunEffect, this.transform);
            getGunAdEffect.transform.localPosition = new Vector3(0, 1.2f, 0);
        }
    }

    #region variable Access Functtion

    public GunName GetGunType()
    {
        return name;
    }

    public int GetGunLevel()
    {
        return level;
    }

    public bool GetUnlockStatus()
    {
        return unLocked;
    }

    public bool GetReadyToUnlockStatus()
    {
        return readyToUnlock;
    }
    #endregion

    #region Helper Functions
    public void SetUpGunMaterial()
    {
        if (level != 5)
        {
            gunMeshRenderer.material = Inventory.Instance.GetAllGunsMaterial();
            magazineMeshRenderer.material = Inventory.Instance.GetAllGunsMaterial();
        }
        else
        {
            gunMeshRenderer.material = Inventory.Instance.GetLevel5GunMaterial();
            magazineMeshRenderer.material = Inventory.Instance.GetLevel5GunMaterial();
        }
    }

    public void SetUpCompletedGaugeMaterial()
    {
        gaugeMeshRenderer.material = Inventory.Instance.GetCompletedGaugeMaterial();
    }

    public void SetTextToComplete()
    {
        stateText.text = "COMPLETE!";
    }

    public void SetGaugeLevel(float setValue , float setTime)
    {
        gauge.DOScaleX(setValue , setTime);
    }

    public void SetUpUpgrade()
    {
        Debug.Log("Gun Name :" + name.ToString() + "Level : " + level);
        switch (name)
        {
            case GunName.Pistol:
                Inventory.Instance.SetPistolCurrentLevelProgress(0);
                Inventory.Instance.SetPistolCurrentLevel(Inventory.Instance.GetPistolCurrentLevel() + 1);
                
                break;

            case GunName.SMG:
                Inventory.Instance.SetSMGCurrentLevelProgress(0);
                Inventory.Instance.SetSMGCurrentLevel(Inventory.Instance.GetSMGCurrentLevel() + 1);
                
                break;

            case GunName.Rifle:
                Inventory.Instance.SetRifleCurrentLevelProgress(0);
                Inventory.Instance.SetRifleCurrentLevel(Inventory.Instance.GetRifleCurrentLevel() + 1);
                
                break;

            case GunName.ShotGun:
                Inventory.Instance.SetShotgunCurrentLevelProgress(0);
                Inventory.Instance.SetShotgunCurrentLevel(Inventory.Instance.GetShotgunCurrentLevel() + 1);
                
                break;

            case GunName.Sniper:
                Inventory.Instance.SetSniperCurrentLevelProgress(0);
                Inventory.Instance.SetSniperCurrentLevel(Inventory.Instance.GetSniperCurrentLevel() + 1);
                
                break;
        }
        Inventory.Instance.UpdateAllStandGunSetUp();
        Destroy(getGunAd.gameObject);
        Destroy(getGunAdEffect.gameObject);
    }
    #endregion
}
