using UnityEngine;

public class SpawnerBase : Spawner<Base>
{
    private Transform _transformSpawn;

    public Base GetNewBase(Transform transform)
    { 
        _transformSpawn = transform;
           
        return _pool.Get();
    }
    
    protected override void SetSpawnPosition(Base newBase)
    {
        newBase.transform.position = _transformSpawn.position;
        newBase.transform.rotation = _transformSpawn.rotation;
        
        base.SetSpawnPosition(newBase);
    }
}
