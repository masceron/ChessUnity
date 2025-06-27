using UnityEngine;

namespace Board.Interaction
{
    public class Marker : MonoBehaviour
    {
        void Update()
        {
           transform.Rotate(Vector3.down, 120f * Time.deltaTime);
        }
    }
}