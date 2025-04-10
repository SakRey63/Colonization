using System;
using System.Collections;
using UnityEngine;

public class BoxBuilding : MonoBehaviour
{
    [SerializeField] private float _delayBulding;
    
    private WaitForSeconds _delayWaitForSeconds;

    public event Action FinishedBuild;

    private void Awake()
    {
        _delayWaitForSeconds = new WaitForSeconds(_delayBulding);
    }

    private IEnumerator BuildBase()
    {
        yield return _delayWaitForSeconds;
        
        FinishedBuild?.Invoke();
    }
    
    public void CreateNewBase()
    {
        StartCoroutine(BuildBase());
    }
}