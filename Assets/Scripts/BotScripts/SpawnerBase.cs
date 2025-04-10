using UnityEngine;

public class SpawnerBase : Spawner<Base>
{
    private Transform _transformSpawn;

    protected override void SetAction(Base newBase)
    {
        newBase.transform.position = _transformSpawn.position;
        newBase.transform.rotation = _transformSpawn.rotation;
        
        base.SetAction(newBase);
    }

    public Base GetNewBase(Transform transform)
    {
        _transformSpawn = transform;

        return _pool.Get();
    }
}
