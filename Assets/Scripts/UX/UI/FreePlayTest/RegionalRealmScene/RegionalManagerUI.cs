
using Game.Common;
using System.Collections.Generic;
using UX.UI.Loader;
using Game.Piece;
using UX.UI.Army.DesignArmy;
using Game.Save.Army;
using UnityEngine;
using Game.Managers;
using Game.Effects.RegionalEffect;
using System.Linq;
using Game.ScriptableObjects;

namespace UX.UI.FreePlayTest
{
    public class RegionalManagerUI : Singleton<RegionalManagerUI>
    {
        public ArmyDesignRelic enemyRelic, allyRelic;
        public RegionalSearcher searcher;
        public ChosenRegionalIcon chosenRegional;
        public FreePlayArmyBoard board;
        public void Load()
        {
            enemyRelic.Load(AssetManager.Ins.RelicData[Config.relicBlackConfig.Type]);
            allyRelic.Load(AssetManager.Ins.RelicData[Config.relicWhiteConfig.Type]);
            chosenRegional.Load(RegionalEffectType.None);
            searcher.Load();
            board.Load(ArmyDesign.Ins.size);
            board.LoadSave(ArmyDesign.Ins.board.Troops.ToArray());
        }

        public void ToGameScene()
        {
            Config.boardSize = ArmyDesign.Ins.army.BoardSize;
            Config.PieceConfigWhite.Clear();
            foreach (Troop troop in ArmyDesign.Ins.board.Troops)
            {
                var augNameLst = troop.equippedAugmentation.Values.ToList();
                List<AugmentationInfo> infos = new();
                foreach (var name in augNameLst)
                {
                    infos.Add(AssetManager.Ins.AugmentationData[name]);
                }
                PieceConfig pieceConfig = new PieceConfig(troop.PieceType, false,
                    (ushort)(troop.Rank * Config.boardSize + troop.File), null);
                Debug.Log($"{BoardUtils.IndexOf(troop.Rank, troop.File)}, {troop.Rank}, {troop.File}");
                Config.PieceConfigWhite.Add(pieceConfig);

            }
            
            Debug.Log($"BoardSize: {ArmyDesign.Ins.army.BoardSize}");
            SceneLoader.LoadSceneWithLoadingScreen(1);

        }
        public void ToDesignArmyPanel()
        {
            UIManager.Ins.Load(CanvasID.FreePlayDesignArmy);
        }
    }
}