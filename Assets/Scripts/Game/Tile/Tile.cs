using System;
using Game.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using static Game.Common.BoardUtils;
using Color = Game.Managers.Color;

namespace Game.Tile
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Tile : MonoBehaviour
    {
        [NonSerialized] public int Rank;
        [NonSerialized] public int File;
        [SerializeField] public Color color;

        // đỉnh trong ô 
        public Corner Corner { get; private set; }

        public void Start()
        {
            if (color == Color.None) gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }

        public void Spawn(int pos)
        {
            Rank = RankOf(pos);
            File = FileOf(pos);

            transform.position = new Vector3(Rank, 1, File);
        }

        public int GetTileValue()
        {
            var formation = GetFormation(IndexOf(Rank, File));
            return formation?.GetValueForAI() ?? 0;
        }
    }
}