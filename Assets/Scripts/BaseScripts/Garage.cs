using System;
using System.Collections.Generic;
using UnityEngine;

public class Garage : MonoBehaviour
{
    [SerializeField] private List<Bot> _allBots;
    [SerializeField] private Transform[] _parkingsPositionBot;
    [SerializeField] private Transform[] _parkingsExitsPositionBot;
    [SerializeField] private Transform[] _stockroomsExitsPositionBot;
    [SerializeField] private Transform[] _stockroomsPositionsBot;

    private Transform _parkingPosition;
    private Transform _parkingExitPosition;
    private Transform _stockroomExitPosition;
    private Transform _stockroomPosition;
    private int _indexPosition;
    
    private Queue<Bot> _freeBots;

    public event Action FreeBot;
    
    public int CountFreeBots => _freeBots.Count;

    private void Awake()
    {
        CreateStartsPosition(_indexPosition);
        
        _allBots = new List<Bot>();
        _freeBots = new Queue<Bot>();
    }
    
    public Bot GetBot()
    {
        return _freeBots.Dequeue();
    }

    public void AddBot(Bot bot)
    {
        bot.Released += ChangeStatus;

        if (bot.IsBuildsNewBase)
        {
            bot.ChangeStatusBuildsNewBase();
        }
        
        bot.SetAllTargetPosition(_parkingPosition, _parkingExitPosition, _stockroomPosition, _stockroomExitPosition);

        CreateStartsPosition(_indexPosition);
        
        _allBots.Add(bot);
        
        bot.ReturnToParking();
    }

    public Bot SendBotToBuildNewBase()
    {
        Bot bot = _freeBots.Dequeue();
        
        bot.Released -= ChangeStatus;
        
        bot.ChangeStatusBuildsNewBase();
        
        _allBots.Remove(_allBots[GetIndexBot()]);
        
        ResetAllPosition();

        return bot;
    }

    private int GetIndexBot()
    {
        int index = 0;

        for (int i = 0; i < _allBots.Count; i++)
        {
            if (_allBots[i].IsBuildsNewBase)
            {
                index = i;
                
                break;
            }
        }

        return index;
    }

    private void SetBot()
    {
        foreach (Bot bot in _allBots)
        {
            bot.SetAllTargetPosition(_parkingPosition, _parkingExitPosition, _stockroomPosition, _stockroomExitPosition);
            
            CreateStartsPosition(_indexPosition);
        }
    }

    private void CreateStartsPosition(int index)
    {
        _parkingPosition = _parkingsPositionBot[index];
        _parkingExitPosition = _parkingsExitsPositionBot[index];
        _stockroomPosition = _stockroomsPositionsBot[index];
        _stockroomExitPosition = _stockroomsExitsPositionBot[index];

        _indexPosition++;
    }

    private void ResetAllPosition()
    {
        _indexPosition = 0;

        CreateStartsPosition(_indexPosition);
        
        SetBot();
    }

    private void ChangeStatus(Bot bot)
    {
        _freeBots.Enqueue(bot);
        
        FreeBot?.Invoke();
    }
}