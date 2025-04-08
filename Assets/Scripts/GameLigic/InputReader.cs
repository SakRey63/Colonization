using System;
using UnityEngine;

public class InputReader : MonoBehaviour
{
    private float _right = 90;
    private float _left = -90;
    
    public event Action ClickedMouse;
    public event Action<float> TurnToRight;
    public event Action<float> TurnToLeft;
        
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TurnToLeft?.Invoke(_left);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            TurnToRight?.Invoke(_right);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ClickedMouse?.Invoke();
        }
    }
}
