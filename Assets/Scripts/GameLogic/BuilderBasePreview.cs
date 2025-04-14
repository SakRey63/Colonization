using UnityEngine;

public class BuilderBasePreview : MonoBehaviour
{
    [SerializeField] private SpawnerBasePreview _spawnerBasePreview;
    [SerializeField] private BuildPreviewMover _previewMover;
    
    private Transform _positionBuild;
    private bool _isChoosingPositions;
    private BasePreview _basePreview;

    public Transform PositionBuild => _positionBuild;
    public BasePreview BasePreview => _basePreview;
    public bool IsChoosingPositions => _isChoosingPositions;

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
        
        _previewMover.MoveBasePreview(_basePreview);
        
        _isChoosingPositions = true;
    }

    public void ReturnToPoolBasePreview()
    {
        _positionBuild = _basePreview.transform;
                    
        _isChoosingPositions = false;
        
        _previewMover.ResetBasePreview();
            
        _spawnerBasePreview.ReturnInPool(_basePreview);
    }
}
