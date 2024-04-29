using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private TextMeshPro healthText;
    [SerializeField] private int health;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private GameObject deathPartical;
    private int healthAc
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
            
            healthText.text = health.ToString();
        }
    }

    private void Start()
    {
        healthAc = health;
        //SetColorOfObstacle();
    }

    public void TakeDamage(int damage)
    {
        healthAc -= damage;
        if (healthAc <= 0)
        {
            Instantiate(deathPartical , transform.position , Quaternion.identity);
            int randomCash = UnityEngine.Random.Range(100, 500);
            ReloadRushGameManager.Instance.AddCollectedCash(randomCash);
            ReloadRushGameManager.Instance.AddMoney(randomCash);
            Destroy(this.gameObject);
        }
    }

    private void SetColorOfObstacle()
    {
        if (healthAc <= 10000)
        {
            meshRenderer.material.color = Color.red;
        }

        if (healthAc <= 5000)
        {
            meshRenderer.material.color = Color.green;
        }

        if (healthAc <= 1200)
        {
            meshRenderer.material.color = Color.yellow;
        }

        if (healthAc <= 100)
        {
            meshRenderer.material.color = Color.blue;
        }
    }
}
