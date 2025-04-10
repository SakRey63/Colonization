using System;
using UnityEngine;

public class BaseConstructionOrchestrator : MonoBehaviour
{
    [SerializeField] private SpawnerBase _spawnerBase;

    private Base _baseToBuildFrom;
    private Bot _buildingBot;
    
    public event Action<Base> FinishedBuildsNewBase;

    private void OnDisable()
    {
        
        if (_baseToBuildFrom != null)
        {
            _baseToBuildFrom.AssignedBot -= SubscribeToBotReadyToSpawn;
        }
    }
    
    public void PrepareNewBaseBuilding(Base baseToBuild)
    {
        _baseToBuildFrom = baseToBuild;
        _baseToBuildFrom.AssignedBot += SubscribeToBotReadyToSpawn;
    }

    private void AddBotToBase(Base baseToAdd, Bot botToAdd)
    {
        baseToAdd.ArrangeBots(botToAdd);
        
        FinishedBuildsNewBase?.Invoke(baseToAdd);
    }
    
    private void SubscribeToBotReadyToSpawn(Bot builderBot)
    {
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
            
            _baseToBuildFrom.AssignedBot -= SubscribeToBotReadyToSpawn;

            _baseToBuildFrom = null;
        }
    }
}