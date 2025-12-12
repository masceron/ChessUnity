using System.Collections.Generic;
using System.Linq;
using Game.Common;
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
        public RegionalIcon chosenRegional;
        public FreePlayArmyBoard board;
        public void Load()
        {
            enemyRelic.Load(AssetManager.Ins.RelicData[Config.relicBlackConfig.Type]);
            allyRelic.Load(AssetManager.Ins.RelicData[Config.relicWhiteConfig.Type]);
            searcher.Load();
            board.Load(FreePlayArmyDesign.Ins.size);
            board.LoadSave(FreePlayArmyDesign.Ins.board.Troops.ToArray());
        }

        public void ToGameScene()
        {
            Config.boardSize = FreePlayArmyDesign.Ins.army.BoardSize;
            Config.PieceConfigWhite.Clear();
            foreach (var troop in FreePlayArmyDesign.Ins.board.Troops)
            {
                var augNameLst = troop.equippedAugmentation.Values.ToList();
                List<AugmentationInfo> infos = new();
                foreach (var name in augNameLst)
                {
                    AugmentationInfo info = AssetManager.Ins.AugmentationData[name];
                    infos.Add(info);
                }
                var pieceConfig = new PieceConfig(troop.PieceType, false,
                    (ushort)(troop.Rank * Config.boardSize + troop.File));
                Debug.Log($"{BoardUtils.IndexOf(troop.Rank, troop.File)}, {troop.Rank}, {troop.File}");
                Config.PieceConfigWhite.Add(pieceConfig);

            }
            
            Debug.Log($"BoardSize: {FreePlayArmyDesign.Ins.army.BoardSize}");
            SceneLoader.LoadFreePlay(LoadCanvasByFunc.chosenGameMode);

        }
        public void ToDesignArmyPanel()
        {
            UIManager.Ins.Load(CanvasID.FreePlayDesignArmy);
        }
    }
}