using System;
using System.Collections.Generic;
using System.Linq;
using Buildings;
using Objects.Vehicles;
using Resources;
using Resources.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Objects
{
    public abstract class Unit: IngameObject
    {
        [Header("Unit"), SerializeField] private Rigidbody2D _rigidbody2D;

        private UnitSO _unitData;

        public Action<Unit, bool> OnUnitClicked;
        public Action<Unit> OnUnitDied;
        public Action<Unit, Unit> OnUnitAttack;

        public int CurrentHp;

        public Rigidbody2D Rigidbody2D => _rigidbody2D;

        public UnitSO UnitData
        {
            get => _unitData;
            protected set => _unitData = value;
        }

        public void Initialize(UnitSO p_data)
        {
            base.Initialize();
            
            CurrentHp = p_data.MaxHp;

            CanvasInfo.HealthBar.maxValue = p_data.MaxHp;
            CanvasInfo.HealthBar.minValue = 0;
            CanvasInfo.HealthBar.value = CurrentHp;

            // Rigidbody2D.drag = EnemyData.Drag;  // Adjust drag to control sliding
            // Rigidbody2D.angularDrag = EnemyData.AngularDrag;  // Control rotational drag
        }
        
        // Rozbić na podklasy
        public override void MakeAction(Actions p_actionType, IngameObject p_giver, IngameObject p_receiver)
        {
            switch (p_actionType)
            {
                case Actions.Refill:
                    if (p_receiver is Vehicle receiver)
                    {
                        var vehicle = receiver;
                        var availableFuel = p_giver.Inventory.GetResourceAmount(Resource.Petroleum);
                        var neededFuel = vehicle.VehicleData.MaxFuel - vehicle.CurrentFuel;

                        if (availableFuel >= neededFuel)
                        {
                            p_giver.Inventory.RemoveResource(Resource.Petroleum, neededFuel);
                            vehicle.RiseFuel(neededFuel);
                        }
                        else
                        {
                            p_giver.Inventory.RemoveResource(Resource.Petroleum, availableFuel);
                            vehicle.RiseFuel(availableFuel);
                        }
                    }

                    break;
                case Actions.Arm:
                    if (p_receiver is CombatVehicle combatVehicle)
                    {
                        foreach (var weapon in combatVehicle.Weapons)
                        {
                            var availableAmmo = p_giver.Inventory.GetResourceAmount(weapon.Data.AmmoType);
                            var ammoNeededForMax = weapon.Data.MaxAmmo - weapon.CurrentAmmo;

                            if (availableAmmo >= ammoNeededForMax)
                            {
                                p_giver.Inventory.RemoveResource(weapon.Data.AmmoType, ammoNeededForMax);
                                weapon.CurrentAmmo += ammoNeededForMax;
                            }
                            else
                            {
                                p_giver.Inventory.RemoveResource(weapon.Data.AmmoType, availableAmmo);
                                weapon.CurrentAmmo += availableAmmo;
                            }
                        }
                    }

                    break;
                case Actions.Repair:
                    break;

                case Actions.Collect:
                    if (p_receiver is Vehicle collector)
                    {
                        if (p_giver is ProductionBuilding prodB)
                        {
                            var producedResource = p_giver.Inventory.GetResourceAmount(prodB.OutputProduction.Type);
                            var spaceLeft = collector.Inventory.GetFreeSpace(prodB.OutputProduction.Type);

                            if (producedResource >= spaceLeft)
                            {
                                p_giver.Inventory.RemoveResource(prodB.OutputProduction.Type, spaceLeft);
                                p_receiver.Inventory.AddResource(prodB.OutputProduction.Type, spaceLeft);
                            }
                            else
                            {
                                p_giver.Inventory.RemoveResource(prodB.OutputProduction.Type, producedResource);
                                p_receiver.Inventory.AddResource(prodB.OutputProduction.Type, producedResource);
                            }
                        }
                    }

                    break;

                case Actions.Deposit:
                    Debug.Log("trying to depo" + p_receiver + "gover:" + p_giver);
                    //przemyśleć depositing i collection z transporting vehicles i jak czołgi można dozbrajać
                    break;
            }
        }

        #region Abstracs

        public abstract void AttackTarget(GameObject p_target);
        public abstract void ReceiveDamage(int p_damage);
        public abstract void DestroyHandler();

        #endregion
    }
}