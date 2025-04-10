using System;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private BaseScanner _scanner;
    [SerializeField] private Garage _garage;
    [SerializeField] private Stockroom _stockroom;
    [SerializeField] private Transform _areaScanningBase;
    [SerializeField] private Database _database;
    [SerializeField] private BotSpawner _botSpawner;
    [SerializeField] private SpawnerFlag _spawnerFlag;
    [SerializeField] private int _maxCountBots;
    [SerializeField] private int _minCountBots;
    [SerializeField] private int _botPrice;
    [SerializeField] private int _basePrice;

    private BaseView _baseView;
    private Flag _flag;
    private int _countResources;
    private int _countBots;
    private bool _isBuilds;
    private bool _isFlagSet;
    private bool _canBuildNow;
    private bool _canCreateBotNow;
    
    public Transform AreaScanningBase => _areaScanningBase;
    public int CountBots => _countBots;

    public event Action<Bot> AssignedBot;
    
    private void Awake()
    {
        _scanner.SetAreaScan();
    }

    private void OnEnable()
    {
        _scanner.DetectedResource += _database.ProcessDetectedResource;;
        _database.FoundedNewResource += AssignBot;
        _garage.FreeBot += AssignBot;
        _stockroom.AcceptedResource += ChangeCountResource;
    }

    private void OnDisable()
    {
        _scanner.DetectedResource -= _database.ProcessDetectedResource;
        _database.FoundedNewResource -= AssignBot;
        _garage.FreeBot -= AssignBot;
        _stockroom.AcceptedResource -= ChangeCountResource;
    }

    private void Update()
    {
        if (_canBuildNow)
        {
            Build();
            
            _canBuildNow = false;
        }
        else if (_canCreateBotNow)
        {
            CreateBot();
            
            _canCreateBotNow = false;
        }
    }

    public void CreateFlag(Transform position)
    {
        position.position = new Vector3(position.position.x, 0, position.position.z);
        
        if (_isFlagSet == false)
        {
            _flag = _spawnerFlag.GetFlag(position);
            
            _isFlagSet = true;
        }
        else
        {
            _spawnerFlag.ReturnInPool(_flag);
            
            _flag = _spawnerFlag.GetFlag(position);
            
            _isFlagSet = true;
        }
    }

    public void CreateDisplayView(BaseView baseView, int number)
    {
        _baseView = baseView;
        
        _baseView.SetCountResource(_countResources);
        
        _baseView.SetNumberBase(number);
    }
    
    public void ArrangeBots(Bot bot)
    {
        bot.transform.parent = transform;

        _countBots++;
        
        _garage.AddBot(bot);
    }

    public void EnableBuildMode()
    {
        _isBuilds = true;
        
        CheckBuildAndCreateBotConditions();
    }

    private void Build()
    {
        if (_countResources >= _basePrice && _garage.CountFreeBots > 0)
        {
            Bot bot = _garage.SendBotToBuildNewBase();
            
            AssignedBot?.Invoke(bot);
            
            bot.MoveOnToBuildNewBase(_flag.transform);

            _stockroom.SpendResources(_basePrice);

            _spawnerFlag.ReturnInPool(_flag);

            _countBots--;

            _flag = null;

            _isFlagSet = false;

            _isBuilds = false;
            
            CheckCreateNewBotCondition();
        }
    }
    
    private void CheckCreateNewBotCondition()
    {
        if (!_isBuilds && _countResources >= _botPrice && _countBots < _maxCountBots)
        {
            _canCreateBotNow = true;
        }
    }

    private void CreateBot()
    {
        if (_countResources >= _botPrice && _countBots < _maxCountBots)
        {
            Bot bot = _botSpawner.GetBot();
            
            ArrangeBots(bot);

            _stockroom.SpendResources(_botPrice);
        }
    }

    private void ChangeCountResource()
    {
        if (_stockroom != null)
        {
            _countResources = _stockroom.BaseResourceCount;
            
            _baseView.SetCountResource(_countResources);
            
            CheckBuildAndCreateBotConditions();
        }
    }

    private void AssignBot()
    {
        if (_database.CountFreeResource > 0 && _garage.CountFreeBots > 0)
        {
            Bot bot = _garage.GetBot();
                
            bot.SetTarget(_database.GetFreeResource());
        }
        
        CheckBuildAndCreateBotConditions();
    }
    
    private void CheckBuildAndCreateBotConditions()
    {
        if (_isBuilds && _countResources >= _basePrice && _garage.CountFreeBots > 0)
        {
            _canBuildNow = true;
        }
        
        if (!_isBuilds && _countResources >= _botPrice && _countBots < _maxCountBots)
        {
            _canCreateBotNow = true;
        }
    }
}