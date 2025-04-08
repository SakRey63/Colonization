using System;
using System.Collections.Generic;
using UnityEngine;

public class Garage : MonoBehaviour
{
    [SerializeField] private List<Bot> _allBots;
    [SerializeField] private Transform _parkingPositionBot;
    [SerializeField] private Transform _parkingExitPositionBot;
    [SerializeField] private Transform _stockroomExitPositionBot;
    [SerializeField] private Transform _stockroomPositionBot;
    [SerializeField] private float _distanceX;

    private Transform _parkingPosition;
    private Transform _parkingExitPosition;
    private Transform _stockroomExitPosition;
    private Transform _stockroomPosition;
    
    private Queue<Bot> _freeBots;

    public int CountFreeBots => _freeBots.Count;
    
    public event Action FreeBot;

    private void Awake()
    {
        CreateStartsPosition();
        
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
        
        ChangeAllPositionsNextBot();
        
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

            ChangeAllPositionsNextBot();
        }
    }

    private void CreateStartsPosition()
    {
        _parkingPosition = _parkingPositionBot;
        _parkingExitPosition = _parkingExitPositionBot;
        _stockroomPosition = _stockroomPositionBot;
        _stockroomExitPosition = _stockroomExitPositionBot;
    }

    private void ResetAllPosition()
    {
        CreateStartsPosition();
        
        SetBot();
    }

    private void ChangeAllPositionsNextBot()
    {
        _parkingPosition.position = new Vector3(_parkingPosition.position.x - _distanceX, _parkingPosition.position.y, _parkingPosition.position.z);
        _parkingExitPosition.position = new Vector3(_parkingExitPosition.position.x - _distanceX, _parkingExitPosition.position.y, _parkingExitPosition.position.z);
        _stockroomPosition.position = new Vector3(_stockroomPosition.position.x - _distanceX, _stockroomPosition.position.y, _stockroomPosition.position.z);
        _stockroomExitPosition.position = new Vector3(_stockroomExitPosition.position.x - _distanceX, _stockroomExitPosition.position.y, _stockroomExitPosition.position.z);
    }

    private void ChangeStatus(Bot bot)
    {
        _freeBots.Enqueue(bot);
        
        FreeBot?.Invoke();
    }
}