using UnityEngine;

public class SpawnBoxBuilding : Spawner<BoxConstruction>
{
    private Transform _transformSpawn;

    public BoxConstruction GetBoxBuilding(Transform transform)
    {
        _transformSpawn = transform;
        
        return _pool.Get();
    }

    public void ReturnOnPoolBoxBuilding(BoxConstruction boxConstruction)
    {
        _pool.Release(boxConstruction); 
    }
    
    protected override void SetSpawnPosition(BoxConstruction boxConstruction)
    {
        if (_transformSpawn != null)
        {
            boxConstruction.transform.position = _transformSpawn.position;
            boxConstruction.transform.rotation = _transformSpawn.rotation;
        }
        
        base.SetSpawnPosition(boxConstruction);
    }
}