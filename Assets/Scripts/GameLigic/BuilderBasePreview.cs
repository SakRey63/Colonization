using UnityEngine;

public class BuilderBasePreview : MonoBehaviour
{
    [SerializeField] private SpawnerBasePreview _spawnerBasePreview;
    
    private Transform _positionBuild;
    private RaycastHit _raycastHit;
    private bool _isChoosingPositions;
    private BasePreview _basePreview;

    public Transform PositionBuild => _positionBuild;
    public BasePreview BasePreview => _basePreview;
    public bool IsChoosingPositions => _isChoosingPositions;
    
    private void FixedUpdate()
    {
        if (_isChoosingPositions)
        {
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out _raycastHit);
            
            if (_raycastHit.transform != null)
            {
                _basePreview.SetPosition(_raycastHit.point);
            }
        }
    }

    public void SetRotationBase(float rotationAngle)
    {
        if (_basePreview != null)
        {
            _basePreview.CreateRotation(rotationAngle);
        }
    }

    public void CreateBasePreview(Transform position)
    {
        _basePreview = _spawnerBasePreview.GetBasePreview();

        _basePreview.transform.position = position.position;
        
        _isChoosingPositions = true;
    }

    public void ReturnToPoolBasePreview()
    {
        _positionBuild = _basePreview.transform;
                    
        _isChoosingPositions = false;
            
        _spawnerBasePreview.ReturnInPool(_basePreview);
    }
}
