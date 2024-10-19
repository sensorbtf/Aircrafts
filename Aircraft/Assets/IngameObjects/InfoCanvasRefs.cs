using UnityEngine;
using UnityEngine.UI;

public class InfoCanvasRefs : MonoBehaviour
{
    [SerializeField] private Slider _healthBar;
    [SerializeField] private CanvasInteractionRefs[] _stateInfo; // Refuell, Repair, Arm
    private Vector3 _initialPosition;
    public Slider HealthBar => _healthBar;
    public CanvasInteractionRefs[] StateInfo => _stateInfo;

    void Start()
    {
        _initialPosition = Vector3.zero;
    }
    
    void LateUpdate()
    {
        var transform1 = transform;
        transform1.rotation = Quaternion.identity;
        if (_initialPosition != Vector3.zero)
        {
            transform1.position = _initialPosition;
        }
    }
    
    private void OnCollisionEnter2D(Collision2D p_collision)
    {
        if (_initialPosition == Vector3.zero && p_collision.gameObject.layer == LayerTagsManager.GroundLayerIndex)
        {
            _initialPosition = transform.position;
        }
    }

    public void Reorder(bool p_show)
    {
        if (p_show)
        {
            GetComponent<Canvas>().sortingOrder = 2;
        }
        else
        {
            GetComponent<Canvas>().sortingOrder = 1;
        }
    }
}