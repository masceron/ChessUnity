using Game.Board.Effects;
using Game.Board.Piece;
using Game.Common;
using UnityEngine;

namespace Game.Board.General
{
    public class AssetManager : Singleton<AssetManager>
    {
        [SerializeField] public UDictionary<PieceType, PieceObject> PieceData;
        [SerializeField] public UDictionary<EffectName, EffectObject> EffectData;
        [SerializeField] public UDictionary<Color, Tile.Tile> TileData;
    }
}
