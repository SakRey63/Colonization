using UnityEngine;

public class SpawnBoxBuilding : Spawner<BoxBuilding>
{
    private Transform _transformSpawn;

    protected override void SetAction(BoxBuilding boxBuilding)
    {
        boxBuilding.transform.position = _transformSpawn.position;
        boxBuilding.transform.rotation = _transformSpawn.rotation;
        
        base.SetAction(boxBuilding);
    }

    public BoxBuilding GetBoxBuilding(Transform transform)
    {
        _transformSpawn = transform;

        return _pool.Get();
    }

    public void ReturnOnPoolBoxBuilding(BoxBuilding boxBuilding)
    {
        _pool.Release(boxBuilding);
    }
}
