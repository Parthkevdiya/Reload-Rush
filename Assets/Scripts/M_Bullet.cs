using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class M_Bullet : MonoBehaviour
{
    [SerializeField] private int bulletCount;
    [SerializeField] private TextMeshPro bulletNumberText;

    private void Start()
    {
        bulletNumberText.text = "+" + bulletCount.ToString();
    }

    public int GetBulletCount()
    {
        return bulletCount;
    }

    public void SetBuletCount(int bulletCountValue)
    {
        bulletCount = bulletCountValue;
        bulletNumberText.text = "+" + bulletCount.ToString();
    }
}
