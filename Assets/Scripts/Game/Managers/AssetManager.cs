using System;
using System.Collections.Generic;
using Game.Common;
using Game.Effects;
using Game.Tile;
using Game.Piece;
using Game.Relics;
using Game.ScriptableObjects;
using Game.ScriptableObjects.Collections;
using UnityEngine;
using Game.Augmentation;

namespace Game.Managers
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class AssetManager : Singleton<AssetManager>
    {
        [NonSerialized] public Dictionary<PieceType, PieceInfo> PieceData;
        [NonSerialized] public Dictionary<EffectName, EffectInfo> EffectData;
        [NonSerialized] public Dictionary<RelicType, RelicInfo> RelicData;
        [NonSerialized] public Dictionary<FormationType, GameObject> EnviromentData;
        [NonSerialized] public Dictionary<AugmentationName, AugmentationInfo> AugmentationData;
        [SerializeField] public UDictionary<Color, Tile.Tile> TileData;

        [SerializeField] private PiecesData pieceData;
        [SerializeField] private EffectsData effectsData;
        [SerializeField] private RelicsData relicsData;
        [SerializeField] private FormationsData enviromentsData;
        [SerializeField] private AugmentationData augmentationData;
        public RegionalsData RegionalsData;

        protected override void Awake()
        {
            base.Awake();
            Load();
        }
        public void Load()
        {
            PieceData = new Dictionary<PieceType, PieceInfo>(pieceData.piecesData);
            EffectData = new Dictionary<EffectName, EffectInfo>(effectsData.effectsData);
            RelicData = new Dictionary<RelicType, RelicInfo>(relicsData.relicsData);
            EnviromentData = new Dictionary<FormationType, GameObject>(enviromentsData.enviromentsData);
            AugmentationData = new Dictionary<AugmentationName, AugmentationInfo>(augmentationData.augmentationsData);
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
