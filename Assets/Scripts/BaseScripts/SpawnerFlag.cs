using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerFlag : Spawner<Flag>
{
    private Transform _transform;
    protected override void SetAction(Flag flag)
    {
        flag.transform.position = _transform.position;
        flag.transform.rotation = _transform.rotation;
        
        base.SetAction(flag);
    }

    public Flag GetFlag(Transform transform)
    {
        _transform = transform;
        
        return _pool.Get();
    }

    public void ReturnInPool(Flag flag)
    {
        _pool.Release(flag);
    }
}
