using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FireRateCharger : MonoBehaviour
{
    [SerializeField] private TextMeshPro fireRateText;
    [SerializeField] private int fireRate;
    [SerializeField] private MeshRenderer meshRenderer;

    [SerializeField] private Color32 colorBlue;
    [SerializeField] private Color32 colorRed;
    private int fireRateAc
    {
        get
        {
            return fireRate;
        }
        set
        {
            fireRate = value;
            if (fireRate < 0)
            {
                meshRenderer.materials[1].color = colorRed;
            }
            else
            {
                meshRenderer.materials[1].color = colorBlue;
            }
            fireRateText.text = fireRate.ToString();
        }
    }

    private void Start()
    {
        fireRateAc = fireRate;
    }

    public void TakeDamage(int damage)
    {
        fireRateAc += damage;
    }

    public int GetFireRateValue()
    {
        return fireRateAc;
    }

    public void SetFireRate(int fireRateValue)
    {
        fireRateAc = fireRateValue;
    }
}
