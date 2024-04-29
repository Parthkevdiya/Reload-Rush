using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGridLayout : MonoBehaviour
{
    public List<Transform> gridObject;

    [SerializeField] private float distace;
    [SerializeField] private float offSetZ;
    [SerializeField] private float dampingZ;
    [SerializeField] private float dampingY;
    private void Start()
    {
        UpdateGridPositions();
    }

    private void Update()
    {
        // Testing...
        UpdateGridPositions();
    }
    public void UpdateGridPositions()
    {
        
        for (int i = 0; i< gridObject.Count; i++)
        {
            gridObject[i].localPosition = new Vector3(0, i* distace + dampingY, 0)  ;
            gridObject[i].localPosition = gridObject[i].localPosition + new Vector3 (0, 0 , i * offSetZ + dampingZ);
            
        }
    }


}
