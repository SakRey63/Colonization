using System;
using UnityEngine;

public class InputReader : MonoBehaviour
{
    private float _rightRotationAngle = 90;
    private float _leftRotationAngle  = -90;
    private KeyCode _rotateLeftKey = KeyCode.Q;
    private KeyCode _rotateRightKey = KeyCode.E;
    private KeyCode _clickKeyMouse = KeyCode.Mouse0;
    
    public event Action ClickedMouse;
    public event Action<float> TurnToRight;
    public event Action<float> TurnToLeft;
        
    private void Update()
    {
        if (Input.GetKeyDown(_rotateLeftKey))
        {
            TurnToLeft?.Invoke(_leftRotationAngle);
        }

        if (Input.GetKeyDown(_rotateRightKey))
        {
            TurnToRight?.Invoke(_rightRotationAngle);
        }

        if (Input.GetKeyDown(_clickKeyMouse))
        {
            ClickedMouse?.Invoke();
        }
    }
}