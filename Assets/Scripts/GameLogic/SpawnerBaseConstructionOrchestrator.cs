public class SpawnerBaseConstructionOrchestrator : Spawner<BaseConstructionOrchestrator>
{
    public BaseConstructionOrchestrator GetBaseConstructionOrchestrator()
    {
        return _pool.Get();
    }

    public void ReturnInPool(BaseConstructionOrchestrator baseConstructionOrchestrator)
    {
        baseConstructionOrchestrator.Reset();
        
        _pool.Release(baseConstructionOrchestrator);
    }
}