using System;
using System.Collections.Generic;
using System.Linq;
using Buildings;
using Enemies;
using UnityEngine;
using UnityEngine.Serialization;
using Vehicles;

namespace Units
{
    public class UnitsManager : MonoBehaviour
    {
        [SerializeField] private Transform _playerBase;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private EnemyDatabase _enemyDatabase;
        [SerializeField] private BuildingsDatabase _buildingsDatabase;
        [SerializeField] private VehiclesDatabase _vehiclesDatabase;

        [SerializeField] private CameraController _cameraController;
        
        public List<Enemy> AllEnemies = new List<Enemy>();
        public List<Vehicle> AllVehicles = new List<Vehicle>();
        public List<Building> AllBuildings = new List<Building>();

        public VehicleController VehicleController;
        public Unit SelectedUnit;
        public Action<Unit> OnUnitCreated;
        
        private void Start()
        {
            VehicleController = new VehicleController();
        }
        
        public void CustomStart()
        {
            foreach (var enemy in _enemyDatabase.Enemies)
            {
                var newEnemy = Instantiate(enemy.Prefab, gameObject.transform);
                newEnemy.transform.position = _spawnPoint.localPosition;

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
                
                OnUnitCreated?.Invoke(unit);
            }
            
            foreach (var building in AllBuildings)
            {
                switch (building.BuildingData.Type)
                {
   
                }
                var unit = building.GetComponent<Unit>();
                unit.OnUnitClicked += SelectUnit;
                unit.OnUnitAttack += UnitAttacked;
                unit.OnUnitDied += UnitDied;
                
                OnUnitCreated?.Invoke(unit);
            }
        }

        private void Update()
        {
            foreach (var enemy in AllEnemies)
            {
                enemy.HandleSpecialAction();
            }
            
            if (VehicleController != null)
            {
                VehicleController.Update();
            }
        }

        private void FixedUpdate()
        {
            foreach (var enemy in AllEnemies)
            {
                enemy.HandleMovement(GetNearestTransformOfPlayerUnit(enemy.transform));
            }
            
            if (VehicleController != null)
            {
                VehicleController.FixedUpdate();
            }
        }

        private Transform GetNearestTransformOfPlayerUnit(Transform p_enemyTransform)
        {
            Transform closestTransform = null;
            float closestDistance = float.MaxValue; // Start with the maximum possible value

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

        public void SelectUnit(Unit p_unit)
        {
            if (SelectedUnit != null)
            {
                SelectedUnit.UnSelectUnit();
            }
            SelectedUnit = p_unit;
            
            if (VehicleController.CurrentVehicle != null)
            {
                UnselectVehicle();
            }
            
            if (p_unit is Vehicle vehicle)
            {
                VehicleController.SetNewVehicle(vehicle);
                VehicleController.CurrentVehicle.SelectUnit();
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
                if (SelectedUnit == p_unit)
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

        public void SelectVehicle(Vehicle p_vehicle)
        {
            VehicleController.SetNewVehicle(p_vehicle);
            VehicleController.CurrentVehicle.SelectUnit();
            _cameraController.UnitTransform = VehicleController.CurrentVehicle.transform;
            SelectedUnit = p_vehicle;
        }
        
        private void UnselectVehicle()
        {
            VehicleController.CurrentVehicle.UnSelectUnit();
            VehicleController.SetNewVehicle(null);
            _cameraController.UnitTransform = null;
        }
    }
}