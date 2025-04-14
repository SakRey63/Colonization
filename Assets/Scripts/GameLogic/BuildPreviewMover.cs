using UnityEngine;

public class BuildPreviewMover : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private BasePreview _basePreview;
    private RaycastHit _raycastHit;
    
    private void FixedUpdate()
    {
        if (_basePreview != null)
        {
            Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out _raycastHit);
                                
            if (_raycastHit.transform != null)
            {
                _basePreview.SetPosition(_raycastHit.point);
            }
        }
    }

    public void MoveBasePreview(BasePreview basePreview)
    {
        _basePreview = basePreview;
    }

    public void ResetBasePreview()
    {
        _basePreview = null;
    }
}
