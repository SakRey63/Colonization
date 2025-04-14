using UnityEngine;
using UnityEngine.Pool;

public class Spawner<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private int _poolCapaciti = 15;
    [SerializeField] private int _poolMaxSize = 30;
    [SerializeField] private T _prefab;

    protected ObjectPool<T> _pool;
    
    protected void Awake()
    {
        _pool = new ObjectPool<T>(
            createFunc: () => Instantiate(_prefab),
            actionOnGet: (obj) => SetSpawnPosition(obj),
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: true,
            defaultCapacity: _poolCapaciti,
            maxSize: _poolMaxSize);
    }
    
    protected virtual void SetSpawnPosition(T obj)
    {
        obj.gameObject.SetActive(true);
    }
}