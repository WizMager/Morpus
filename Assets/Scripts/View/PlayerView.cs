using UnityEngine;

namespace View
{
    public class PlayerView : MonoBehaviour
    {
        public void Move(Vector2 direction)
        {
            transform.Translate(new Vector3(direction.x, 0, direction.y));
        }
    }
}