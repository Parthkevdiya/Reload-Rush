using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageCharger : MonoBehaviour
{
    [SerializeField] private TextMeshPro damageText;
    [SerializeField] private int damage;
    [SerializeField] private MeshRenderer meshRenderer;

    [SerializeField] private Color32 colorBlue;
    [SerializeField] private Color32 colorRed;
    private int damageAc
    {
        get
        {
            return damage;
        }
        set
        {
            damage = value;
            if (damage < 0)
            {
                meshRenderer.materials[1].color = colorRed;
            }
            else
            {
                meshRenderer.materials[1].color = colorBlue;
            }
            damageText.text = damage.ToString();
        }
    }

    private void Start()
    {
        damageAc = damage;
    }

    public void TakeDamage(int damage)
    {
        damageAc += damage;
    }

    public int GetDamageValue()
    {
        return damageAc;
    }

    public void SetDamage(int damageValue)
    {
        damageAc = damageValue;
    }
}
