using UnityEngine;

public class SpawnerFlag : Spawner<Flag>
{
    private Transform _transform;
    
    public Flag GetFlag(Transform transform)
    {
        _transform = transform;
        
        return _pool.Get();
    }

    public void ReturnInPool(Flag flag)
    {
        _pool.Release(flag);
    }
    
    protected override void SetSpawnPosition(Flag flag)
    {
        flag.transform.position = _transform.position;
        flag.transform.rotation = _transform.rotation;
        
        base.SetSpawnPosition(flag);
    }
}
