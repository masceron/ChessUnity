using System;
using System.Collections.Generic;
using Game.Common;
using Game.Tile;
using Game.ScriptableObjects;
using Game.ScriptableObjects.Collections;
using UnityEngine;
using Game.Augmentation;

namespace Game.Managers
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class AssetManager : Singleton<AssetManager>
    {
        [NonSerialized] public Dictionary<string, PieceInfo> PieceData;
        [NonSerialized] public Dictionary<string, EffectInfo> EffectData;
        [NonSerialized] public Dictionary<string, RelicInfo> RelicData;
        [NonSerialized] public Dictionary<FormationType, FormationInfo> FormationData;
        [NonSerialized] public Dictionary<AugmentationName, AugmentationInfo> AugmentationData;
        
        [SerializeField] public UDictionary<Color, Tile.Tile> tileData;
        [SerializeField] private PiecesData pieceData;
        [SerializeField] private EffectsData effectsData;
        [SerializeField] private RelicsData relicsData;
        [SerializeField] private AugmentationData augmentationData;
        [SerializeField] public RegionalsData regionalsData;
        [SerializeField] private FormationsData formationDataSo;
        protected override void Awake()
        {
            Load();
        }

        private void Load()
        {
            PieceData = new Dictionary<string, PieceInfo>();
            foreach (var piece in pieceData.piecesData)
            {
                PieceData.Add(piece.key, piece);
            }
            
            EffectData = new Dictionary<string, EffectInfo>();
            foreach (var effect in effectsData.effectsData)
            {
                EffectData.Add(effect.key, effect);
            }
            
            RelicData = new Dictionary<string, RelicInfo>();
            foreach (var relic in relicsData.relicsData)
            {
                RelicData.Add(relic.key, relic);
            }
            
            FormationData = new Dictionary<FormationType, FormationInfo>(formationDataSo.formationsData);
            AugmentationData = new Dictionary<AugmentationName, AugmentationInfo>(augmentationData.augmentationsData);
        }
    }
}
