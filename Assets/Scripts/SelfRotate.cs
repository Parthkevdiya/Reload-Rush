using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfRotate : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;
    [SerializeField] private Vector3 rotateAngle;
    private void Update()
    {

        transform.Rotate(rotateAngle * rotateSpeed);
    }
}
