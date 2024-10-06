using Objects;
using Resources;
using Resources.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Buildings
{
    public class ProductionBuilding: Building// oil/ sand 
    {
        [SerializeField] private int _timeToComplete;
        [SerializeField] private GameObject _oilCanPrefab;
        [SerializeField] private Transform _oilCanSpot;
        [SerializeField] private ResourceSO _outputProduction;

        private float _currentProductionTime;
        private bool _isPaused;
        private float _timer;

        public ResourceSO OutputProduction => _outputProduction;
        public float CurrentProductionTime => _currentProductionTime;
        public int TimeToComplete => _timeToComplete;
        public bool IsPaused => _isPaused;

        public override void Initialize(BuildingSO p_buildingData)
        {
            base.Initialize(p_buildingData);
            _currentProductionTime = 0;
        }

        public override void Update()
        {
            base.Update();

            _timer += Time.deltaTime;

            if (Inventory.GetResourceAmount(_outputProduction) > 0)
            { 
                SetNewStateTexts(Actions.Collect);
            }

            if (_timer >= 1f)
            {
                _currentProductionTime++;

                _timer = 0f;

                if (_currentProductionTime >= _timeToComplete)
                {
                    var res = Inventory.AddResource(_outputProduction, 1);
                    _currentProductionTime = 0;

                    var itemOnGround = Instantiate(_oilCanPrefab, _oilCanSpot.position, _oilCanSpot.rotation);
                    var refs = itemOnGround.GetComponent<ItemOnGround>();
                    refs.Initialize(res);
                }
            }
        }
    }

    public enum ProductionType
    {
        Oil,
        Sand
    }
}