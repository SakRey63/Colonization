using UnityEngine;

public class TargetDirection : MonoBehaviour
{
    [SerializeField] private Rotator _rotator;
    
    private Vector3 _parking; 
    private Vector3 _parkingExit;
    private Vector3 _stockroom;
    private Vector3 _stockroomExit;

    public void SetPositionTargets(Transform parking, Transform parkingExit, Transform stockroom, Transform stockroomExit)
    {
        _parking = parking.position;
        _parkingExit = parkingExit.position;
        _stockroom = stockroom.position;
        _stockroomExit = stockroomExit.position;
    }

    public void ChangeDirectionNewBase(Transform position)
    {
        _rotator.ChangeDirection(position.position);
    }

    public void ChangeDirectionParkingExit()
    {
        _rotator.ChangeDirection(_parkingExit);
    }

    public void ChangeDirectionResourcePosition(Transform position)
    {
        _rotator.ChangeDirection(position.position);
    }
    
    public void ChangeDirectionStockroomExit()
    {
        _rotator.ChangeDirection(_stockroomExit);
    }
    
    public void ChangeDirectionStockroom()
    {
        _rotator.ChangeDirection(_stockroom);
    }
    
    public void ChangeDirectionParking()
    {
        _rotator.ChangeDirection(_parking);
    }
}