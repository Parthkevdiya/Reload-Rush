using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCanvas : MonoBehaviour
{
    [SerializeField] private M_Bullet leftM_Bullet;
    [SerializeField] private M_Bullet rightM_Bullet;

    public M_Bullet GetLeftM_Bullet()
    {
        return leftM_Bullet;
    }

    public M_Bullet GetRightM_Bullet()
    {
        return rightM_Bullet;
    }
}
