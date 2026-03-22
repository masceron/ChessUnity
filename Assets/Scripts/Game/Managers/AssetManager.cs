using System;
using System.Collections.Generic;
using Game.Augmentation;
using Game.Common;
using Game.ScriptableObjects;
using Game.ScriptableObjects.Collections;
using Game.Tile;
using UnityEngine;

namespace Game.Managers
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class AssetManager : Singleton<AssetManager>
    {
        [SerializeField] public UDictionary<Color, Tile.Tile> tileData;
        [SerializeField] private PiecesData pieceData;
        [SerializeField] private EffectsData effectsData;
        [SerializeField] private RelicsData relicsData;
        [SerializeField] private AugmentationData augmentationData;
        [SerializeField] public RegionalsData regionalsData;
        [SerializeField] private FormationsData formationDataSo;
        [NonSerialized] public Dictionary<AugmentationName, AugmentationInfo> AugmentationData;
        [NonSerialized] public Dictionary<string, EffectInfo> EffectData;
        [NonSerialized] public Dictionary<FormationType, FormationInfo> FormationData;
        [NonSerialized] public Dictionary<string, PieceInfo> PieceData;
        [NonSerialized] public Dictionary<string, RelicInfo> RelicData;

        protected override void Awake()
        {
            Load();
        }

        private void Load()
        {
            PieceData = new Dictionary<string, PieceInfo>();
            foreach (var piece in pieceData.piecesData)
            {
                if (piece == null)
                {
                    Debug.LogError("[AssetManager] Null PieceInfo found in PiecesData. Please remove missing entries from the asset list.");
                    continue;
                }

                PieceData.Add(piece.key, piece);
            }

            EffectData = new Dictionary<string, EffectInfo>();
            foreach (var effect in effectsData.effectsData)
            {
                if (effect == null)
                {
                    Debug.LogError("[AssetManager] Null EffectInfo found in EffectsData. Please remove missing entries from the asset list.");
                    continue;
                }

                EffectData.Add(effect.key, effect);
            }

            RelicData = new Dictionary<string, RelicInfo>();
            foreach (var relic in relicsData.relicsData)
            {
                if (relic == null)
                {
                    Debug.LogError("[AssetManager] Null RelicInfo found in RelicsData. Please remove missing entries from the asset list.");
                    continue;
                }

                RelicData.Add(relic.key, relic);
            }

            FormationData = new Dictionary<FormationType, FormationInfo>(formationDataSo.formationsData);
            AugmentationData = new Dictionary<AugmentationName, AugmentationInfo>(augmentationData.augmentationsData);
        }
    }
}