using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompletedMagazine : MonoBehaviour
{
    public static CompletedMagazine Instance;

    [SerializeField] private Vector3[] magazinePositions;
    [SerializeField] private MagazineVisual[] completedMagazines;


    private void Start()
    {
        Instance = this;
    }

    public void SetUpCompletedMagazine(MagazineVisual completedMagazine)
    {
        for(int i=0 ; i<completedMagazines.Length; i++)
        {
            if (completedMagazines[i] == null)
            {
                completedMagazine.transform.DOLocalJump(magazinePositions[i] , 1 , 1 , 0.5f).OnComplete( () => { completedMagazine.transform.parent = this.transform; });
                completedMagazines[i] = completedMagazine;
                break;
            }
        }
    }

    public void SetUpLastMagazine(MagazineVisual completedMagazine)
    {
        for (int i = 0; i < completedMagazines.Length; i++)
        {
            if (completedMagazines[i] == null)
            {
                //completedMagazine.transform.DOLocalJump(magazinePositions[i], 1, 1, 0.5f).OnComplete(() => { completedMagazine.transform.parent = this.transform; });
                completedMagazine.transform.parent = this.transform;
                completedMagazines[i] = completedMagazine;
                break;
            }
        }
    }

    public MagazineVisual GetGunMagazine(int gunIndex)
    {
        if (completedMagazines[gunIndex] != null)
        {
            return completedMagazines[gunIndex];
        }
        else 
        {
            return null; 
        }
        
    }
}
