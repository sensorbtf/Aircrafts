using Resources.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Buildings
{
    public class ProductionBuilding: Building// oil/ sand 
    {
        [SerializeField] private int _timeToComplete;
        [FormerlySerializedAs("_productionIco")] [SerializeField] private ResourceSO _outputProduction;
        
        private ProductionType _productionType;
        private float _currentProductionTime;
        private bool _isPaused;
        private float _timer;

        public ProductionType ProductionType => _productionType;
        public ResourceSO OutputProduction => _outputProduction;
        public float CurrentProductionTime => _currentProductionTime;
        public int TimeToComplete => _timeToComplete;
        public bool IsPaused => _isPaused;

        public void Initialize(BuildingSO p_buildingData, ProductionType p_oil)
        {
            _productionType = p_oil;
            base.Initialize(p_buildingData);
            _currentProductionTime = 0;
        }

        public override void Update()
        {
            base.Update();

            _timer += Time.deltaTime;

            if (_timer >= 1f)
            {
                _currentProductionTime++;

                _timer = 0f;
                
                if (_currentProductionTime >= _timeToComplete)
                {
                    Inventory.AddResource(Resource.Sand, 1);
                    _currentProductionTime = 0;
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