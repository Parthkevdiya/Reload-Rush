using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MagazineVisual : MonoBehaviour
{
    [SerializeField] private TextMeshPro magazineBulletCountText;
    [SerializeField] private Transform[] bullets;
    [SerializeField] private int maxBullet;
    [SerializeField] private bool magazineCompleted;

    private void Start()
    {
        maxBullet = bullets.Length;
    }
    public void SetMagazineBullet(int bulletCount , float maxInterpolationValue)
    {
        magazineBulletCountText.text = bulletCount + "/" + maxInterpolationValue;
        for (int i=0; i<bullets.Length; i++)
        {
            bullets[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < bulletCount; i++)
        {
            bullets[i].gameObject.SetActive(true);
        }

        if (bulletCount == maxBullet)
        {
            SetMagazineCompleted();
        }
    }

    public int GetMagazineMaxBullet()
    {
        return maxBullet;
    }

    public bool GetMagazineCompleted()
    {
        return magazineCompleted;
    }

    public void SetMagazineCompleted()
    {
        magazineCompleted = true;
    }
}
