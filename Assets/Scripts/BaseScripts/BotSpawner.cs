using UnityEngine;

public class BotSpawner : Spawner<Bot>
{
    [SerializeField] private Transform _spawnPosition;
    
    protected override void SetAction(Bot bot)
    {
        bot.transform.position = _spawnPosition.position;
        
        base.SetAction(bot);
    }

    public Bot GetBot()
    {
        return _pool.Get();
    }
}