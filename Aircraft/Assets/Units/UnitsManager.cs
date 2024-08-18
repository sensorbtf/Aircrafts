using System;
using System.Collections.Generic;
using System.Linq;
using Buildings;
using Enemies;
using Resources;
using UnityEngine;
using UnityEngine.Serialization;
using Vehicles;
using Units.Vehicles;

namespace Units
{
    public class UnitsManager : MonoBehaviour
    {
        [SerializeField] private Transform _enemySpawnPoint;
        [SerializeField] private EnemyDatabase _enemyDatabase;
        [SerializeField] private BuildingsDatabase _buildingsDatabase;
        [SerializeField] private VehiclesDatabase _vehiclesDatabase;
        [SerializeField] private InventoriesManager _inventoriesManager;

        [SerializeField] private CameraController _cameraController;

        public List<Enemy> AllEnemies = new List<Enemy>();
        public List<Vehicle> AllVehicles = new List<Vehicle>();
        public List<Building> AllBuildings = new List<Building>();

        private SelectedUnitController _selectedUnitController;
        private Unit _selectedUnit;

        public Unit SelectedUnit => _selectedUnit;
        
        public Action<Unit> OnUnitCreated;
        public Action<Unit> OnUnitSelected;

        private void Start()
        {
            _selectedUnitController = new SelectedUnitController();
        }

        public void CustomStart()
        {
            foreach (var enemy in _enemyDatabase.Enemies)
            {
                var newEnemy = Instantiate(enemy.Prefab, gameObject.transform);
                newEnemy.transform.position = _enemySpawnPoint.localPosition;

                switch (enemy.Type)
                {
                    case EnemyType.GroundMelee:
                        AllEnemies.Add(newEnemy.GetComponent<GroundEnemy>());
                        newEnemy.GetComponent<GroundEnemy>().Initialize(enemy);
                        break;
                    case EnemyType.GroundRange:
                        break;
                    case EnemyType.Flying:
                        break;
                }

                var unit = newEnemy.GetComponent<Unit>();

                unit.OnUnitClicked += SelectUnit;
                unit.OnUnitAttack += UnitAttacked;
                unit.OnUnitDied += UnitDied;

                unit.PostInitialize(_inventoriesManager.CreateInventory(unit));

                OnUnitCreated?.Invoke(unit);
            }

            foreach (var vehicleSo in _vehiclesDatabase.Vehicles)
            {
                var newVehicle = Instantiate(vehicleSo.Prefab, gameObject.transform, true);
                newVehicle.transform.position = GetMainBase().transform.position;

                switch (vehicleSo.Type)
                {
                    case VehicleType.Combat:
                        AllVehicles.Add(newVehicle.GetComponent<CombatVehicle>());
                        newVehicle.GetComponent<CombatVehicle>().Initialize(vehicleSo);
                        break;
                    case VehicleType.GeneralPurpose:
                        AllVehicles.Add(newVehicle.GetComponent<GPUVehicle>());
                        newVehicle.GetComponent<GPUVehicle>().Initialize(vehicleSo);
                        break;
                    case VehicleType.Transporter:
                        AllVehicles.Add(newVehicle.GetComponent<TransportingVehicle>());
                        newVehicle.GetComponent<TransportingVehicle>().Initialize(vehicleSo);
                        break;
                    case VehicleType.Underground:
                        AllVehicles.Add(newVehicle.GetComponent<UndergroundVehicle>());
                        newVehicle.GetComponent<UndergroundVehicle>().Initialize(vehicleSo);
                        break;
                }

                var unit = newVehicle.GetComponent<Unit>();
                unit.OnUnitClicked += SelectUnit;
                unit.OnUnitAttack += UnitAttacked;
                unit.OnUnitDied += UnitDied;
                unit.PostInitialize(_inventoriesManager.CreateInventory(unit));

                OnUnitCreated?.Invoke(unit);
            }

            foreach (var building in AllBuildings)
            {
                building.Initialize(building.BuildingData);

                var unit = building.GetComponent<Unit>();
                unit.OnUnitClicked += SelectUnit;
                unit.OnUnitAttack += UnitAttacked;
                unit.OnUnitDied += UnitDied;
                unit.PostInitialize(_inventoriesManager.CreateInventory(unit));

                OnUnitCreated?.Invoke(unit);
            }
        }

        private void Update()
        {
            foreach (var enemy in AllEnemies)
            {
                enemy.HandleSpecialAction();
            }

            _selectedUnitController?.Update();
        }

        private void FixedUpdate()
        {
            foreach (var enemy in AllEnemies)
            {
                enemy.HandleMovement(GetNearestTransformOfPlayerUnit(enemy.transform));
            }

            _selectedUnitController?.FixedUpdate();
        }

        private Transform GetNearestTransformOfPlayerUnit(Transform p_enemyTransform)
        {
            Transform closestTransform = null;
            float closestDistance = float.MaxValue;

            foreach (var vehicle in AllVehicles)
            {
                float distanceToVehicle = Vector2.Distance(p_enemyTransform.position, vehicle.transform.position);
                if (distanceToVehicle < closestDistance)
                {
                    closestDistance = distanceToVehicle;
                    closestTransform = vehicle.transform;
                }
            }

            foreach (var building in AllBuildings)
            {
                float distanceToBuilding = Vector2.Distance(p_enemyTransform.position, building.transform.position);
                if (distanceToBuilding < closestDistance)
                {
                    closestDistance = distanceToBuilding;
                    closestTransform = building.transform;
                }
            }

            return closestTransform;
        }

        public void SelectUnit(Unit p_unit, bool p_invokeEvent = false)
        {
            if (_selectedUnit != null)
            {
                _selectedUnit.UnSelectUnit();

                if (_selectedUnitController.CurrentUnit != null)
                {
                    UnselectVehicle();
                }
            }

            _selectedUnit = p_unit;

            if (p_unit is Vehicle vehicle)
            {
                SelectUnit(vehicle);
            }
            else if (p_unit is Enemy enemy)
            {
            }
            else if (p_unit is Building building)
            {
            }
            else
            {
                Debug.LogError("What have I clicked? " + p_unit);
            }

            _cameraController.UnitTransform = p_unit.transform;
            
            if (p_invokeEvent)
            {
                OnUnitSelected?.Invoke(p_unit);
            }
        }

        private void UnitAttacked(Unit p_attacker, Unit p_defender)
        {
            if (p_defender is Vehicle vehicle)
            {
                vehicle.ReceiveDamage(p_attacker.UnitData.AttackDamage);
            }
            else if (p_defender is Enemy enemy)
            {
                enemy.ReceiveDamage(p_attacker.UnitData.AttackDamage);
            }
            else if (p_defender is Building building)
            {
                building.ReceiveDamage(p_attacker.UnitData.AttackDamage);
            }
        }

        private void UnitDied(Unit p_unit)
        {
            p_unit.GetComponent<Unit>().OnUnitClicked -= SelectUnit;
            p_unit.GetComponent<Unit>().OnUnitAttack -= UnitAttacked;
            p_unit.GetComponent<Unit>().OnUnitDied -= UnitDied;

            if (p_unit is Vehicle vehicle)
            {
                vehicle.DestroyHandler();
                AllVehicles.Remove(vehicle);
                if (_selectedUnit == p_unit)
                {
                    SelectUnit(GetMainBase());
                }
            }
            else if (p_unit is Enemy enemy)
            {
                enemy.DestroyHandler();
                AllEnemies.Remove(enemy);
            }
            else if (p_unit is Building building)
            {
                if (building.BuildingData.Type == BuildingType.Main_Base)
                {
                    // Restart/menu
                }

                building.DestroyHandler();
                AllBuildings.Remove(building);
            }
            else
            {
                Debug.LogError("What have I clicked? " + p_unit);
            }
        }

        public Building GetMainBase()
        {
            return AllBuildings.FirstOrDefault(x => x.BuildingData.Type == BuildingType.Main_Base);
        }

        public void SelectUnit(Unit p_unit)
        {
            _selectedUnitController.SetNewUnit(p_unit);
            _selectedUnitController.CurrentUnit.SelectUnit();
            _cameraController.UnitTransform = _selectedUnitController.CurrentUnit.transform;
            _selectedUnit = p_unit;
        }

        private void UnselectVehicle()
        {
            _selectedUnitController.CurrentUnit.UnSelectUnit();
            _selectedUnitController.SetNewUnit(null);
            _cameraController.UnitTransform = null;
        }
    }
}