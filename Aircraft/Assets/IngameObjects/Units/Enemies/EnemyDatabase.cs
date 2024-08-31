using System.Linq;
using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName = "EnemyDatabase", menuName = "Enemies/BuildingsDatabase", order = 3)]
    public class EnemyDatabase: ScriptableObject
    {
        public EnemySO[] Enemies;

        public EnemySO GetEnemyByType(EnemyType p_type)
        {
            return Enemies.FirstOrDefault(x => x.Type == p_type);
        }
    }
}