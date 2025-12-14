using UnityEngine;

namespace Views
{
    public class EnemyView : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        
        public Renderer Renderer => _renderer;

        public void VisibleEnable(bool isVisible)
        {
            gameObject.SetActive(isVisible);
        }
    }
}