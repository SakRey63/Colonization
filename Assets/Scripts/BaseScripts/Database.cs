using System;
using System.Collections.Generic;
using UnityEngine;

public class Database : MonoBehaviour
{
    [SerializeField] private BaseScanner _scanner;
    
    private List<Resource> _freeResource;
    private HashSet<int> _assignedResource;
    
    public event Action FoundedNewResource;
    
    public int CountFreeResource => _freeResource.Count;

    private void Awake()
    {
        _freeResource = new List<Resource>();
        _assignedResource = new HashSet<int>();
    }
    
    public void ProcessDetectedResource(Resource resource)
    {
        if (_assignedResource.Contains(resource.Index) == false)
        {
            bool isFinding = false;
            
            foreach (Resource freeResource in _freeResource)
            {
                if (resource.Index == freeResource.Index)
                {
                    isFinding = true;
                }
            }

            if (isFinding == false)
            {
                _freeResource.Add(resource);
                    
                FoundedNewResource?.Invoke();
            }
        }
    }
    
    public void RemoveToAssignedResource(int index)
    {
        _assignedResource.Remove(index);
    }

    public Resource GetFreeResource()
    {
        Resource resource = _freeResource[0];
        
        _freeResource.Remove(_freeResource[0]);
        
        ChangeStatusResource(resource);

        return resource;
    }

    private void ChangeStatusResource(Resource resource)
    {
        _assignedResource.Add(resource.Index);
    }
}