using UnityEngine;

public class BasePreview : MonoBehaviour
{
    [SerializeField] private Material _materialError;
    [SerializeField] private Material _materialPossibleBuild;
    [SerializeField] private float _maxPosition;
    [SerializeField] private float _minPosition;
    
    private Renderer _renderer;
    private bool _isPossibleBuild;
    private bool _isMonitoring = true;
    private float _deltaX = 15;
    private float _deltaZ = 20;

    public bool IsPossibleBuild => _isPossibleBuild;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        MonitoredPosition();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Base>(out _)|| other.TryGetComponent<Flag>(out _) || other.TryGetComponent<BoxBuilding>(out _))
        {
            _renderer.material = _materialError;

            _isPossibleBuild = false;

            _isMonitoring = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Base>(out _)|| other.TryGetComponent<Flag>(out _) || other.TryGetComponent<BoxBuilding>(out _))
        {
            _renderer.material = _materialPossibleBuild;

            _isPossibleBuild = true;

            _isMonitoring = true;
        }
    }

    public void CreateRotation(float rotationAngle)
    {
        transform.Rotate(transform.rotation.x, transform.rotation.y + rotationAngle, transform.rotation.z);
        
        ChangeParametersMargins();
    }

    public void SetPosition(Vector3 position)
    {
        Vector3 positionTarget = new Vector3(position.x, 0.5f, position.z);
        
        transform.position = positionTarget;
    }

    private void ChangeParametersMargins()
    {
        float tempDelta = _deltaX;
        
        _deltaX = _deltaZ;
        _deltaZ = tempDelta;
    }

    private void MonitoredPosition()
    {
        if (_isMonitoring)
        {
             if (transform.position.x > _maxPosition - _deltaX || transform.position.x < _minPosition + _deltaX || transform.position.z > _maxPosition - _deltaZ || transform.position.z < _minPosition + _deltaZ)
             {
                 _renderer.material = _materialError;
            
                 _isPossibleBuild = false;
             }
             else
             {
                 _renderer.material = _materialPossibleBuild;
            
                 _isPossibleBuild = true;
             }
        }
    }
}
