using UnityEngine;

public class SpawnBoxBuilding : Spawner<BoxBuilding>
{
    private Transform _transformSpawn;

    public BoxBuilding GetBoxBuilding(Transform transform)
    {
        _transformSpawn = transform;
        
        return _pool.Get();
    }

    public void ReturnOnPoolBoxBuilding(BoxBuilding boxBuilding)
    {
        _pool.Release(boxBuilding); 
    }
    
    protected override void SetSpawnPosition(BoxBuilding boxBuilding)
    {
        if (_transformSpawn != null)
        {
            boxBuilding.transform.position = _transformSpawn.position;
            boxBuilding.transform.rotation = _transformSpawn.rotation;
        }
        
        base.SetSpawnPosition(boxBuilding);
    }
}