using Buildings;
using Objects.Vehicles;
using UnityEngine;

public class CableConnectionManager : MonoBehaviour // could not be needed
{
    [SerializeField] private GameObject _cablePrefab; 
    [SerializeField] private GameObject _energyPoint; 
    [SerializeField] private LineRenderer _activeCable;
    [SerializeField] private float _maxDistanceBetweenPoints;
    
    private bool _isConnecting = false;
    private Building _startBuilding;
    private Vehicle _vehicle;

    public void StartConnecting(Vehicle p_vehicle, Building p_building)
    {
        _startBuilding = p_building;
        _isConnecting = true;

        GameObject newCable = Instantiate(_cablePrefab);
        _activeCable = newCable.GetComponent<LineRenderer>();
        _activeCable.positionCount = 2;

        _activeCable.SetPosition(0, p_building.transform.position);
    }

    public void Update()
    {
        if (!_isConnecting || _activeCable == null)
            return;

        _activeCable.SetPosition(1, _vehicle.transform.position);
    }

    public void CompleteConnection(Building p_endBuilding)
    {
        // Finalize the connection between startBuilding and endBuilding
        _activeCable.SetPosition(1, p_endBuilding.transform.position);

        // Add logic to connect buildings to the grid or manage energy distribution
        // For example, add the buildings to a connected list or update energy stats

        _isConnecting = false;  // Stop the connection process
        _activeCable = null;  // Clear the active cable
        _vehicle = null;  
    }
    
    public void StopConnection()
    {
        Destroy(_activeCable);
        _isConnecting = false; 
        _activeCable = null;  
        _vehicle = null;  
    }
}
