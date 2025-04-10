using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnerResources : Spawner<Resource>
{
    [SerializeField] private float _spawnRadius;
    [SerializeField] private float _positionY;

    private Transform _transformSpawn;
    private Vector3 _positionSpawn;
    private Quaternion _quaternionSpawn = Quaternion.Euler(Vector3.zero);
    private int _index;

    protected override void SetAction(Resource resource)
    {
        _index++;
        
        SetRandomPosition();
        
        resource.Delivered += ReleaseResource;
        
        resource.transform.position = _positionSpawn;
        resource.transform.rotation = _quaternionSpawn;
        resource.SetIndex(_index);
        
        base.SetAction(resource);
    }

    public void SetAreaResource(Transform position)
    {
        _transformSpawn = position;
        
        GetGameObject();
    }

    private void ReleaseResource(Resource resource)
    {
        resource.Delivered -= ReleaseResource;
        
        Release(resource);
    }

    private void SetRandomPosition()
    {
        Vector2 randomCircle = Random.insideUnitCircle * _spawnRadius;
        
        _positionSpawn = new Vector3(_transformSpawn.position.x + randomCircle.x, _positionY, _transformSpawn.position.z + randomCircle.y);
    }
}