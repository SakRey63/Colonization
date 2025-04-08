using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private BaseScanner _scanner;
    [SerializeField] private Garage _garage;
    [SerializeField] private Stockroom _stockroom;
    [SerializeField] private Transform _areaScanningBase;
    [SerializeField] private Database _database;
    [SerializeField] private BotSpawner _spawnerBot;
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
    
    public Transform AreaScanningBase => _areaScanningBase;
    public int CountBots => _countBots;

    public event Action<Bot> AssignedBot;

    private void OnEnable()
    {
        _scanner.Detected += SetAllResource;
        _database.FoundedNewResource += AssignBot;
        _garage.FreeBot += AssignBot;
        _stockroom.AcceptedResource += ChangeCountResource;
    }

    private void OnDisable()
    {
        _scanner.Detected -= SetAllResource;
        _database.FoundedNewResource -= AssignBot;
        _garage.FreeBot -= AssignBot;
        _stockroom.AcceptedResource -= ChangeCountResource;
    }

    private void Awake()
    {
        _scanner.SetAreaScan(_areaScanningBase);
    }

    private void Update()
    {
        ChooseStatus();
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
    
    public void ArrangeNewBots(Bot bot)
    {
        bot.transform.parent = transform;

        _countBots++;
        
        _garage.AddBot(bot);
    }

    public void ChangeStatus()
    {
        _isBuilds = true;
    }

    private void ChooseStatus()
    {
        if (_isBuilds)
        {
            Build();
        }
        else
        {
            CreateNewBot();
        }
    }

    private void Build()
    {
        if (_countResources >= _basePrice && _garage.CountFreeBots > 0)
        {
            Bot bot = _garage.SendBotToBuildNewBase();
            
            AssignedBot?.Invoke(bot);
            
            bot.MoveOnToBuildNewBase(_flag.transform);
            
            _spawnerFlag.ReturnInPool(_flag);
            
            _stockroom.SpendResources(_basePrice);

            _countBots--;

            _flag = null;

            _isFlagSet = false;

            _isBuilds = false;
        }
    }

    private void CreateNewBot()
    {
        if (_countResources >= _botPrice && _countBots < _maxCountBots)
        {
            Bot bot = _spawnerBot.GetBot();
            
            ArrangeNewBots(bot);

            _stockroom.SpendResources(_botPrice);
        }
    }

    private void SetAllResource(Dictionary<int, Resource> resources)
    {
        _database.DetermineStatusResource(resources);
    }

    private void ChangeCountResource()
    {
        _countResources = _stockroom.BaseResourceCount;
        
        _baseView.SetCountResource(_countResources);
    }

    private void AssignBot()
    {
        if (_database.CountFreeResource > 0 && _garage.CountFreeBots > 0)
        {
            Bot bot = _garage.GetBot();
                
            bot.SetTarget(_database.GetFreeResource());
        }
    }
}