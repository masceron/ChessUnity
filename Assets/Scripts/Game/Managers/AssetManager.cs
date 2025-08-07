using System;
using System.Collections.Generic;
using Game.Common;
using Game.Data.Effects;
using Game.Data.Pieces;
using Game.Effects;
using Game.Piece;
using UnityEngine;

namespace Game.Managers
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class AssetManager : Singleton<AssetManager>
    {
        [NonSerialized] public Dictionary<PieceType, PieceObject> PieceData;
        [NonSerialized] public Dictionary<EffectName, EffectObject> EffectData;
        
        [SerializeField] public UDictionary<Color, Tile.Tile> TileData;
        [SerializeField] private PiecesData pieceData;
        [SerializeField] private EffectsData effectsData;

        public void Load()
        {
            PieceData = new Dictionary<PieceType, PieceObject>(pieceData.piecesData);
            EffectData = new Dictionary<EffectName, EffectObject>(effectsData.effectsData);
        }
    }
}
