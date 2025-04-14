using System;
using UnityEngine;

public class BotScanner : MonoBehaviour
{
    private bool _isUnloadingResource;
    
    public event Action AchievedCheckPoint;
    public event Action<Resource> AchievedResource;
    public event Action AchievedWaitingPoint;
    public event Action<Stockroom> AchievedStockroom;
    public event Action<BoxConstruction> AchievedBoxBuilding;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ParkingExit>(out _))
        {
            AchievedCheckPoint?.Invoke();
        }
        else if(other.TryGetComponent(out Resource resource))
        {
            AchievedResource?.Invoke(resource);
        }
        else if(other.TryGetComponent<StockroomExit>(out _))
        {
            AchievedCheckPoint?.Invoke();
        }
        else if (other.TryGetComponent(out Stockroom stockroom) && _isUnloadingResource == false)
        {
            _isUnloadingResource = true;
            
             AchievedStockroom?.Invoke(stockroom);
        }
        else if (other.TryGetComponent<Parking>(out _))
        {
            AchievedWaitingPoint?.Invoke();
        }
        else if (other.TryGetComponent(out BoxConstruction boxBuilding))
        {
            AchievedBoxBuilding?.Invoke(boxBuilding);
        }
    }
    
    public void ResetUnloadingState()
    {
        _isUnloadingResource = false;
    }
}