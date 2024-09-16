using Resources.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Resources.Scripts
{
   [CreateAssetMenu(fileName = "ResourcesDatabase", menuName = "Resource/Database", order = 3)]
    public class ResourceDatabase: ScriptableObject
    {
        [SerializeField] private ResourceSO[] resources;

        public ResourceSO[] Resources => resources;
    }
}