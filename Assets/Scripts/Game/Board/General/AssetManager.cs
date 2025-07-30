using System.Collections.Generic;
using Game.Board.Effects;
using Game.Board.Piece;
using UnityEngine;

namespace Game.Board.General
{
    public class AssetManager : MonoBehaviour
    {
        [SerializeField] private PieceObject[] pieceData;
        [SerializeField] private EffectObject[] effectData;
        [SerializeField] private Tile.Tile[] tileData;
        
        public Dictionary<PieceType, PieceObject> PieceData;
        public Dictionary<EffectType, EffectObject> EffectData;
        public Dictionary<Color, Tile.Tile> TileData;

        public void Init()
        {
            PieceData = new Dictionary<PieceType, PieceObject>();
            foreach (var piece in pieceData)
            {
                PieceData.Add(piece.type, piece);
            }

            EffectData = new Dictionary<EffectType, EffectObject>();
            foreach (var effect in effectData)
            {
                EffectData.Add(effect.typeName, effect);
            }

            TileData = new Dictionary<Color, Tile.Tile>();

            foreach (var tile in tileData)
            {
                TileData.Add(tile.color, tile);
            }
        }
    }
}
