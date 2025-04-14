using UnityEngine;

public class BotSpawner : Spawner<Bot>
{
    [SerializeField] private Transform _spawnPosition;
    
    public Bot GetBot()
    {
        return _pool.Get();
    }
    protected override void SetSpawnPosition(Bot bot)
    {
        bot.transform.position = _spawnPosition.position;
        
        base.SetSpawnPosition(bot);
    }
}