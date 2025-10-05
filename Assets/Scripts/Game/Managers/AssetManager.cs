using System;
using System.Collections.Generic;
using Game.Common;
using Game.Effects;
using Game.Tile;
using Game.Piece;
using Game.ScriptableObjects;
using Game.ScriptableObjects.Collections;
using UnityEngine;

namespace Game.Managers
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class AssetManager : Singleton<AssetManager>
    {
        [NonSerialized] public Dictionary<PieceType, PieceInfo> PieceData;
        [NonSerialized] public Dictionary<EffectName, EffectInfo> EffectData;
        [NonSerialized] public Dictionary<FormationType, GameObject> EnviromentData;
        [SerializeField] public UDictionary<Color, Tile.Tile> TileData;
        [SerializeField] private PiecesData pieceData;
        [SerializeField] private EffectsData effectsData;
        [SerializeField] private EnviromentsData enviromentsData;

        public void Load()
        {
            PieceData = new Dictionary<PieceType, PieceInfo>(pieceData.piecesData);
            EffectData = new Dictionary<EffectName, EffectInfo>(effectsData.effectsData);
            EnviromentData = new Dictionary<FormationType, GameObject>(enviromentsData.enviromentsData);
        }
    }
}
