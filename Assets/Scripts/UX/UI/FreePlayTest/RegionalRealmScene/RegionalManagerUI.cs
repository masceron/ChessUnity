using System.Collections.Generic;
using System.Linq;
using Game.Common;
using Game.Effects.RegionalEffect;
using Game.Managers;
using Game.Piece;
using Game.ScriptableObjects;
using UnityEngine;
using UX.UI.Army.DesignArmy;
using UX.UI.FreePlayTest.DesignArmyScene;
using UX.UI.Loader;

namespace UX.UI.FreePlayTest.RegionalRealmScene
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
            foreach (var troop in ArmyDesign.Ins.board.Troops)
            {
                var augNameLst = troop.equippedAugmentation.Values.ToList();
                List<AugmentationInfo> infos = new();
                foreach (var name in augNameLst)
                {
                    infos.Add(AssetManager.Ins.AugmentationData[name]);
                }
                var pieceConfig = new PieceConfig(troop.PieceType, false,
                    (ushort)(troop.Rank * Config.boardSize + troop.File));
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