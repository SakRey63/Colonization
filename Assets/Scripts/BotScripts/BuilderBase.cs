using System;
using UnityEngine;

public class BuilderBase : MonoBehaviour
{
    [SerializeField] private SpawnBoxBuilding _spawnBoxBuilding;
    
    private BoxBuilding _boxBuilding;
    private Transform _transformBuildBase;

    public event Action<Transform> FinishedBuildsBox;

    public void BuildsBox(Transform transform)
    {
        _boxBuilding = _spawnBoxBuilding.GetBoxBuilding(transform);

        _transformBuildBase = _boxBuilding.transform;
        
        _boxBuilding.FinishedBuild += OnBoxBuilt;
    }

    private void OnBoxBuilt()
    {
        _boxBuilding.FinishedBuild -= OnBoxBuilt;
        
        _spawnBoxBuilding.ReturnOnPoolBoxBuilding(_boxBuilding);

        _boxBuilding = null;
        
        FinishedBuildsBox?.Invoke(_transformBuildBase);
    }
}