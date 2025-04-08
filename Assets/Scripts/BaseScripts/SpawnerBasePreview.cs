using UnityEngine;

public class SpawnerBasePreview : Spawner<BasePreview>
{
    private Transform _positionSpawn;

    protected override void SetAction(BasePreview basePreview)
    {
        base.SetAction(basePreview);
    }

    public BasePreview GetBasePreview()
    {
        return _pool.Get();
    }

    public void ReturnInPool(BasePreview basePreview)
    {
        _pool.Release(basePreview);
    }
}
