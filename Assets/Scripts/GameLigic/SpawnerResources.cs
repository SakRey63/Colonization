using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnerResources : Spawner<Resource>
{
    [SerializeField] private float _spawnRadius;
    [SerializeField] private float _positionY;
    [SerializeField] private float _delaySpawnResource;
    [SerializeField] private Transform _transformSpawn;
    
    private Quaternion _quaternionSpawn = Quaternion.Euler(Vector3.zero);
    private int _index;

    private void Start()
    {
        StartCoroutine(RepeatResource());
    }

    public void SetAreaResource()
    {
        _pool.Get();
    }
    
    protected override void SetSpawnPosition(Resource resource)
    {
        _index++;
        
        GetRandomPosition();
        
        resource.Delivered += ReleaseResource;
        
        resource.transform.position = GetRandomPosition();
        resource.transform.rotation = _quaternionSpawn;
        resource.SetIndex(_index);
        
        base.SetSpawnPosition(resource);
    }
    
    private IEnumerator RepeatResource()
    {
        WaitForSeconds delay = new WaitForSeconds(_delaySpawnResource);

        while (enabled)
        {
            SetAreaResource();

            yield return delay;
        }
    }

    private void ReleaseResource(Resource resource)
    {
        resource.Delivered -= ReleaseResource;
        
        _pool.Release(resource);
    }

    private Vector3 GetRandomPosition()
    {
        Vector2 randomCircle = Random.insideUnitCircle * _spawnRadius;
        
        return new Vector3(_transformSpawn.position.x + randomCircle.x, _positionY, _transformSpawn.position.z + randomCircle.y);
    }
}