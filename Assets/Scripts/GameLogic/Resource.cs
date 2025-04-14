using System;
using System.Collections;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] private float _delay;

    public event Action<Resource> Delivered;
    
    public int Index { get; private set; }
    
    public void Reset()
    {
        Index = 0;
                
        StartCoroutine(WaitingСall());
    }
        
    public void SetIndex(int index)
    {
        Index = index;
    }
    
    private IEnumerator WaitingСall()
    {
        WaitForSeconds delay = new WaitForSeconds(_delay);
    
        yield return delay;
            
        Delivered?.Invoke(this);
    }
}