using Buildings;
using Enemies;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.HUD
{
    public class EnemyPanel: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _enemyHeader;

        private void Start()
        {
            ClosePanel();
        }
        
        public void OpenPanel(Enemy p_enemy)
        {
            gameObject.SetActive(true);

            _enemyHeader.text = p_enemy.EnemyData.Type.ToString();
        }

        public void ClosePanel()
        {
            gameObject.SetActive(false);
        }
    }
}