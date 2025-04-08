
using System;
using UnityEngine;

public class BuilderBase : MonoBehaviour
{
    [SerializeField] private SpawnBoxBuilding _spawnBoxBuilding;
    [SerializeField] private SpawnNewBase _spawnNewBase;

    private BoxBuilding _boxBuilding;
    private Transform _transformBuildBase;

    public event Action<Base> FinishedBuildsNewBase;
    
    public void BuildsBox(Transform transform)
    {
        _boxBuilding = _spawnBoxBuilding.GetBoxBuilding(transform);

        _transformBuildBase = _boxBuilding.transform;
        
        _boxBuilding.FinishedBuild += BuildNewBase;
    }

    private void BuildNewBase()
    {
        _boxBuilding.FinishedBuild -= BuildNewBase;
        
        _spawnBoxBuilding.ReturnOnPoolBoxBuilding(_boxBuilding);

        _boxBuilding = null;

        Base newBase = _spawnNewBase.GetNewBase(_transformBuildBase);
        
        FinishedBuildsNewBase?.Invoke(newBase);
    }
}