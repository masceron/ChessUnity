using Board.Interaction;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.EventSystems;

namespace Board.Tile
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Tile : MonoBehaviour, IPointerClickHandler
    {
        private int rank;
        private int file;
        private GameObject cell;
        private MeshRenderer floor;
        private bool isActive;
        
        public void Spawn(int r, int f, GameObject prefab, bool active)
        {
            rank = r;
            file = f;
            cell = Instantiate(prefab, transform);
            cell.transform.position = new Vector3(rank, 1, file);
            isActive = active;
        }

        public void OnPointerClick(PointerEventData data)
        {
            switch (data.button)
            {
                case PointerEventData.InputButton.Left:
                    if (!isActive) return;
                    InteractionManager.Select(rank, file);
                    break;
                default:
                    return;
            }
        }
    }
}
