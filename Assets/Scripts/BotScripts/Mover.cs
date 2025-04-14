using System;
using System.Collections;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _delayDriveBack;
    
    private Vector3 _directionMovementForward = Vector3.forward;
    private Coroutine _coroutine;
    
    public event Action DeliveredResource;

    public void MoveTarget()
    {
        _coroutine = StartCoroutine(MoveTo());
    }

    public void StoppedMovement()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
    }

    public void SetBotMoveBack()
    {
        _coroutine = StartCoroutine(DriveBack());
    }
    
    private IEnumerator DriveBack()
    {
        _directionMovementForward = -Vector3.forward;

        float currentTime = 0;

        while (currentTime < _delayDriveBack)
        {
            currentTime += Time.deltaTime;
            
            Move();
            
            yield return null;
        }

        _directionMovementForward = Vector3.forward;
        
        DeliveredResource?.Invoke();
    }

    private IEnumerator MoveTo()
    {
        while (enabled)
        {
            Move();
            
            yield return null;
        }
    }

    private void Move()
    {
        transform.Translate(_directionMovementForward * _moveSpeed * Time.deltaTime, Space.Self);
    }
}