using System;
using System.Collections;
using UnityEngine;

public class BoxBuilding : MonoBehaviour
{
    [SerializeField] private float _delayBulding;

    public event Action FinishedBuild;
    
    private IEnumerator BuildBase()
    {
        WaitForSeconds delay = new WaitForSeconds(_delayBulding);

        yield return delay;
        
        FinishedBuild?.Invoke();
    }
    
    public void CreateNewBase()
    {
        StartCoroutine(BuildBase());
    }
}