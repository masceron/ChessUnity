using System.Collections.Generic;
using Game.Piece;
using UnityEngine;

namespace Game.Statue
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Statue : MonoBehaviour
    {
        [Header("Black Piece Configuration")] [SerializeField]
        private List<PieceConfig> blackPieceConfigs = new()
        {
            new PieceConfig("piece_blue_dragon", true, 20),
            new PieceConfig("piece_archelon", true, 25)
        };

        public List<PieceConfig> GetBlackPieceConfigs()
        {
            return blackPieceConfigs;
        }

        public void SetBlackPieceConfigs(List<PieceConfig> configs)
        {
            blackPieceConfigs = configs;
        }
    }
}