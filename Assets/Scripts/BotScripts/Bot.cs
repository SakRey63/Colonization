using System;
using UnityEngine;

public class Bot : MonoBehaviour
{
    [SerializeField] private BotScanner _botScanner;
    [SerializeField] private TargetDirection _targetDirection;
    [SerializeField] private Mover _mover;
    [SerializeField] private Rotator _rotator;
    [SerializeField] private LiftingMechanism _liftingMechanism;
    [SerializeField] private ResourceTransfer _resourceTransfer;
    [SerializeField] private FlashingLight _flashingLight;
    [SerializeField] private BuilderBase _builderBase;
    
    private bool _isDeliveredToBase;
    private bool _isReleased;
    private bool _isUploaded;
    private bool _isMoving;
    private bool _isRotating;
    private Resource _resource;
    private bool _isBuildsNewBase;

    public bool IsBuildsNewBase => _isBuildsNewBase;

    public event Action<Bot> Released;
    public event Action<Transform> ReadyToSpawnBase;

    private void OnEnable()
    {
        _botScanner.AchievedCheckPoint += ChooseDirection;
        _botScanner.AchievedResource += RaisingResource;
        _botScanner.AchievedStockroom += UnloadingResource;
        _botScanner.AchievedWaitingPoint += ChangeStatus;
        _botScanner.AchievedBoxBuilding += BuildBase;
        _liftingMechanism.AscentFinished += DeliveringResource;
        _liftingMechanism.Unloaded += DriveBack;
        _mover.DeliveredResource += ReturnToParking;
        _builderBase.FinishedBuildsBox += OnFinishedBuildsBox;
    }

    private void OnDisable()
    {
        _botScanner.AchievedCheckPoint -= ChooseDirection;
        _botScanner.AchievedResource -= RaisingResource;
        _botScanner.AchievedStockroom -= UnloadingResource;
        _botScanner.AchievedWaitingPoint -= ChangeStatus;
        _botScanner.AchievedBoxBuilding -= BuildBase;
        _liftingMechanism.AscentFinished -= DeliveringResource;
        _liftingMechanism.Unloaded -= DriveBack;
        _mover.DeliveredResource -= ReturnToParking;
        _builderBase.FinishedBuildsBox -= OnFinishedBuildsBox;
    }

    private void Start()
    {
        _flashingLight.ChangeEffectWaiting();
    }

    public void ChangeStatusBuildsNewBase()
    {
        if (_isBuildsNewBase == false)
        {
            _isBuildsNewBase = true;
        }
        else
        {
            _isBuildsNewBase = false;
        }
    }

    public void SetAllTargetPosition(Transform parking, Transform parkingExit, Transform stockroom, Transform stockroomExit)
    {
        _targetDirection.SetPositionTargets(parking, parkingExit, stockroom, stockroomExit);
    }

    public void SetTarget(Resource resource)
    {
        _resource = resource;
        
        _isReleased = false;

        _targetDirection.ChangeDirectionParkingExit();
        
        ChangeMovementRotation();
        
        _flashingLight.ChangeEffectFollowing();
    }

    public void MoveOnToBuildNewBase(Transform transformSpawn)
    {
        _isDeliveredToBase = false;

        transform.parent = null;
        
        _builderBase.BuildsBox(transformSpawn);
        
        _targetDirection.ChangeDirectionNewBase(transformSpawn);
        
        ChangeMovementRotation();
    }
    
    public void ReturnToParking()
    {
        _isDeliveredToBase = true;
        
        _targetDirection.ChangeDirectionParkingExit();

        ChangeMovementRotation();
    }
    
    private void OnFinishedBuildsBox(Transform transformBase)
    {
        ReadyToSpawnBase?.Invoke(transformBase);
        
        _isReleased = false;
    }

    private void BuildBase(BoxBuilding boxBuilding)
    {
        ChangeMovementRotation();
        
        boxBuilding.CreateNewBase();
    }

    private void ChangeStatus()
    {
        if (_isReleased == false)
        {
            _isReleased = true;

            ChangeMovementRotation();
                    
            Released?.Invoke(this);
            
            _flashingLight.ChangeEffectWaiting();
        }
    }

    private void ChangeMovementRotation()
    {
        if (_isRotating && _isMoving)
        {
            _isRotating = false;
            _isMoving = false;
            
            _rotator.ChangeRotation(_isRotating);
            _mover.ChangeMove(_isMoving);
        }
        else
        {
            _isRotating = true;
            _isMoving = true;
            
            _rotator.ChangeRotation(_isRotating);
            _mover.ChangeMove(_isMoving);
        }
    }

    private void ChooseDirection()
    {
        if (_isDeliveredToBase)
        {
            if (_isUploaded == false)
            {
                if (_resource != null)
                {
                    _targetDirection.ChangeDirectionResourcePosition(_resource.transform);
                }
                else
                {
                    _targetDirection.ChangeDirectionParking();
                }
            }
            else
            {
                _targetDirection.ChangeDirectionStockroom();
            }
        }
    }

    private void DriveBack()
    {
        _isUploaded = false;
        
        _mover.SetBotMoveBack();
        
        _flashingLight.ChangeEffectFollowing();
        
        _botScanner.ResetUnloadingState();
    }

    private void UnloadingResource(Stockroom stockroom)
    {
        if (_resource != null)
        {
            ChangeMovementRotation();
                    
            bool isUploaded = false;
                    
            _liftingMechanism.ChangeElevator(_resource, isUploaded);
                    
            _resourceTransfer.TransferResource(stockroom, _resource);
            
            _resource = null;
        }
    }

    private void DeliveringResource()
    {
        _isUploaded = true;
        
        _targetDirection.ChangeDirectionStockroomExit();
        
        ChangeMovementRotation();
    }

    private void RaisingResource(Resource resource)
    {
        if (_resource != null && _resource.Index == resource.Index)
        {
            bool isUploaded = true;
            
            ChangeMovementRotation();
            
            _liftingMechanism.ChangeElevator(resource, isUploaded);
            
            _flashingLight.ChangeEffectWork();
        }
    }
}