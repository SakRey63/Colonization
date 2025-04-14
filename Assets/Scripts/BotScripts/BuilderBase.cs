using System;
using UnityEngine;

public class BuilderBase : MonoBehaviour
{
    [SerializeField] private SpawnBoxBuilding _spawnBoxBuilding;
    
    private BoxConstruction _boxConstruction;
    private Transform _transformBuildBase;

    public event Action<Transform> FinishedBuildsBox;

    public void BuildsBox(Transform transform)
    {
        _boxConstruction = _spawnBoxBuilding.GetBoxBuilding(transform);

        _transformBuildBase = _boxConstruction.transform;
        
        _boxConstruction.FinishedBuild += OnBoxBuilt;
    }

    private void OnBoxBuilt()
    {
        _boxConstruction.FinishedBuild -= OnBoxBuilt;
        
        _spawnBoxBuilding.ReturnOnPoolBoxBuilding(_boxConstruction);

        _boxConstruction = null;
        
        FinishedBuildsBox?.Invoke(_transformBuildBase);
    }
}