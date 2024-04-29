using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestory : MonoBehaviour
{
    [SerializeField] private float destroyDelay;

    private void Start()
    {
        Destroy(this.gameObject , destroyDelay);
    }
}
