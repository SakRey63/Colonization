using System;
using System.Collections.Generic;
using UnityEngine;

public class Stockroom : MonoBehaviour
{
    [SerializeField] private Database _database;
    
    private List<Resource> _baseResources;

    public event Action AcceptedResource;
    
    public int BaseResourceCount => _baseResources.Count;

    private void Awake()
    {
        _baseResources = new List<Resource>();
    }

    public void SpendResources(int price)
    {
        for (int i = 0; i < price; i++)
        {
            _baseResources.Remove(_baseResources[0]);
            
            AcceptedResource?.Invoke();
        }
    }

    public void TransferResource(Resource resource)
    {
        _database.RemoveToAssignedResource(resource.Index);
        
        _baseResources.Add(resource);
        
        resource.Reset();
        
        AcceptedResource?.Invoke();
    }
}