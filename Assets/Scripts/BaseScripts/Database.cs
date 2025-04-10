using System;
using System.Collections.Generic;
using UnityEngine;

public class Database : MonoBehaviour
{
    [SerializeField] private BaseScanner _scanner;
    
    private Dictionary<int, Resource> _freeResource;
    private HashSet<int> _assignedResource;
    private List<int> _indexFreeResource;
    
    public int CountFreeResource => _freeResource.Count;

    public event Action FoundedNewResource;

    private void Awake()
    {
        _indexFreeResource = new List<int>();
        _freeResource = new Dictionary<int, Resource>();
        _assignedResource = new HashSet<int>();
    }
    
    public void ProcessDetectedResource(Resource resource)
    {
        if (_assignedResource.Contains(resource.Index) == false && _freeResource.ContainsKey(resource.Index) == false)
        {
            _freeResource.Add(resource.Index, resource);
                
            _indexFreeResource.Add(resource.Index);
                    
            FoundedNewResource?.Invoke();
        }
    }
    
    public void RemoveToAssignedResource(int index)
    {
        _assignedResource.Remove(index);
    }

    public Resource GetFreeResource()
    {
        int number = _indexFreeResource[0];
        
        Resource resource = _freeResource[number];
        
        _indexFreeResource.Remove(number);
        
        ChangeStatusResource(_freeResource[number]);

        return resource;
    }

    private void ChangeStatusResource(Resource resource)
    {
        _freeResource.Remove(resource.Index);
        
        _assignedResource.Add(resource.Index);
    }
}