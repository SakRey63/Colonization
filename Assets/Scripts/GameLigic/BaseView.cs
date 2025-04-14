using System;
using TMPro;
using UnityEngine;

public class BaseView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _numberBase;
    [SerializeField] private TextMeshProUGUI _countResourceBase;
    [SerializeField] private Canvas _canvas;
    
    public void SetCamera(Camera camera)
    {
        _canvas.worldCamera = camera;
    }
    public void SetNumberBase(int number)
    {
        _numberBase.SetText(Convert.ToString(number));
    }
    
    public void SetCountResource(int number)
    {
        _countResourceBase.SetText(Convert.ToString(number));
    }
}