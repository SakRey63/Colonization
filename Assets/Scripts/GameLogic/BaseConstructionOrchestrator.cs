using System;
using UnityEngine;

public class BaseConstructionOrchestrator : MonoBehaviour
{
    [SerializeField] private SpawnerBase _spawnerBase;

    private Base _baseToBuildFrom;
    private Bot _buildingBot;
    
    public event Action<Base, BaseConstructionOrchestrator> FinishedBuildsNewBase;

    public void Reset()
    {
        _buildingBot = null;
        _baseToBuildFrom = null;
    }

    public void PrepareNewBaseBuilding(Base baseToBuild)
    {
        _baseToBuildFrom = baseToBuild;
        
        _baseToBuildFrom.AssignedBot += SubscribeToBotReadyToSpawn;
    }

    private void AddBotToBase(Base baseToAdd, Bot botToAdd)
    {
        baseToAdd.ArrangeBots(botToAdd);
        
        FinishedBuildsNewBase?.Invoke(baseToAdd, this);
    }
    
    private void SubscribeToBotReadyToSpawn(Bot builderBot)
    {
        _baseToBuildFrom.AssignedBot -= SubscribeToBotReadyToSpawn;
        
        _buildingBot = builderBot;
        
        builderBot.ReadyToSpawnBase += SpawnNewBaseAtPosition;
    }
    
    private void SpawnNewBaseAtPosition(Transform spawnPosition)
    {
        _buildingBot.ReadyToSpawnBase -= SpawnNewBaseAtPosition;
        
        if (_spawnerBase != null && _baseToBuildFrom != null)
        {
            Base newBase = _spawnerBase.GetNewBase(spawnPosition);
            
            AddBotToBase(newBase, _buildingBot);
        }
    }
}