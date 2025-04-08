using UnityEngine;

public class SpawnerBaseView : Spawner<BaseView>
{
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private float _deltaPositionY;

    protected override void SetAction(BaseView baseView)
    {
        baseView.transform.position = _spawnPosition.position;
        baseView.transform.parent = _spawnPosition.parent;
        
        ChangePositionTextInfo();
            
        base.SetAction(baseView);
    }

    public BaseView GetTextBaseInfo()
    {
        return _pool.Get();
    }

    private void ChangePositionTextInfo()
    {
        _spawnPosition.position = new Vector3(_spawnPosition.position.x, _spawnPosition.position.y - _deltaPositionY,
            _spawnPosition.position.z);
    }
}