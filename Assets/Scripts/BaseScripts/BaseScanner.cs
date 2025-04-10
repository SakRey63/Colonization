using System;
using System.Collections;
using UnityEngine;

public class BaseScanner : MonoBehaviour
{
    [SerializeField] private float _delay;
    [SerializeField] private float _scanRadius;
    [SerializeField] private Transform _transformScan;
    
    public event Action<Resource> DetectedResource;
    
    private IEnumerator RepeatedScanningTerritory()
    {
        WaitForSeconds delay = new WaitForSeconds(_delay);

        while (enabled)
        {
            ScanTerritory();
            
            yield return delay;
        }
    }
    
    public void SetAreaScan()
    {
        StartCoroutine(RepeatedScanningTerritory());
    }

    private void ScanTerritory()
    {
        Collider[] hitColliders = Physics.OverlapSphere(_transformScan.position, _scanRadius);

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent(out Resource resource))
            {
                DetectedResource?.Invoke(resource);
            }
        }
    }
}