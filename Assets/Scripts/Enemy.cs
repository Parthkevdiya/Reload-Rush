using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private TextMeshPro strengthText;
    [SerializeField] private int helath;
    [SerializeField] private GameObject moneyBundle;
    [SerializeField] private GameObject deathPartical;
    [SerializeField] private int HealthAc
    {
        get 
        {
            return helath; 
        }
        set 
        {
            helath = value;
            strengthText.text = helath.ToString();
        }
    }

    private void Start()
    {
        HealthAc = helath;
    }

    public void TakeDamage(int damage)
    {
        HealthAc -= damage;
        if (HealthAc <= 0)
        {
            Instantiate(deathPartical , transform.position , Quaternion.identity);
            Instantiate(moneyBundle , transform.position + new Vector3(0 , 0.5f , 0) , Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    public void SetHealth(int health)
    {
        HealthAc = health;
    }

}
