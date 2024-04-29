using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class FailPanelUI : MonoBehaviour
{
    [SerializeField] private Button retry;
    private void Start()
    {
        retry.onClick.AddListener( () => 
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            SoundManager.Instance.PlayClickSound();
        });
    }
}
