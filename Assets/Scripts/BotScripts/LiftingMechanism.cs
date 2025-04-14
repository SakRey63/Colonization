using System;
using System.Collections;
using UnityEngine;

public class LiftingMechanism : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _delay;

    private bool _isUploaded;
    private Resource _resource;

    public event Action AscentFinished;
    public event Action Unloaded;

    public void UploadResource()
    {
        _isUploaded = false;

        StartCoroutine(WorkingElevator());
    }

    public void SelectResource(Resource resource)
    {
        _resource = resource;
        
        _isUploaded = true;
        
        StartCoroutine(WorkingElevator());
    }
    
    private IEnumerator WorkingElevator()
    {
        WaitForSeconds delay = new WaitForSeconds(_delay);
        
        _animator.SetBool(AnimatorData.Params.Raise, _isUploaded);

        yield return delay;

        if (_isUploaded)
        {
            _resource.transform.parent = transform;
            
            AscentFinished?.Invoke();
        }
        else
        {
            if (_resource != null)
            {
                _resource.transform.parent = null;
                
                _resource = null;
                            
                Unloaded?.Invoke();
            }
            
        }
    }
}