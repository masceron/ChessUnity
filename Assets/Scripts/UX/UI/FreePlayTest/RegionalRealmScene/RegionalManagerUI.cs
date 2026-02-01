using Game.Common;
using Game.Managers;
using Game.Piece;
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
            board.Load(FPArmyDesign.Ins.size);
            board.LoadSave(FPArmyDesign.Ins.board.Troops.ToArray(), FPArmyDesign.Ins.board.EnemyTroops.ToArray());
        }

        public void ToGameScene()
        {
            Config.boardSize = FPArmyDesign.Ins.army.BoardSize;

            Config.PieceConfigWhite.Clear();
            foreach (var troop in FPArmyDesign.Ins.board.Troops)
            {
                var augNameLst = troop.equippedAugmentation.Values.ToList();
                // List<AugmentationInfo> infos = new();
                // foreach (var name in augNameLst)
                // {
                //     AugmentationInfo info = AssetManager.Ins.AugmentationData[name];
                //     infos.Add(info);
                // }
                var pieceConfig = new PieceConfig(troop.PieceType, false,
                    (ushort)(troop.Rank * Config.boardSize + troop.File), augNameLst);
                Debug.Log($"{BoardUtils.IndexOf(troop.Rank, troop.File)}, {troop.Rank}, {troop.File}");
                Config.PieceConfigWhite.Add(pieceConfig);
            }

            Config.PieceConfigBlack.Clear();
            foreach (var troop in FPArmyDesign.Ins.board.EnemyTroops)
            {
                var augNameLst = troop.equippedAugmentation.Values.ToList();
                // List<AugmentationInfo> infos = new();
                // foreach (var name in augNameLst)
                // {
                //     AugmentationInfo info = AssetManager.Ins.AugmentationData[name];
                //     infos.Add(info);
                // }
                var pieceConfig = new PieceConfig(troop.PieceType, true,
                    (ushort)(troop.Rank * Config.boardSize + troop.File), augNameLst);
                Debug.Log($"{BoardUtils.IndexOf(troop.Rank, troop.File)}, {troop.Rank}, {troop.File}");
                Config.PieceConfigBlack.Add(pieceConfig);
            }

            Debug.Log($"BoardSize: {FPArmyDesign.Ins.army.BoardSize}");
            SceneLoader.LoadFreePlay(LoadCanvasByFunc.chosenGameMode);
        }
        public void ToDesignArmyPanel()
        {
            UIManager.Ins.Load(CanvasID.FreePlayDesignArmy);
        }
    }
}