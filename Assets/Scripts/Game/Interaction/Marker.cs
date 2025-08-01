using UnityEngine;

namespace Game.Interaction
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Marker : MonoBehaviour
    {
        private void Update()
        {
           transform.Rotate(Vector3.down, 120f * Time.deltaTime);
        }
    }
}