

using Game.Common;
using TMPro;
using System.Collections.Generic;
using UX.UI.Loader;
using Game.Piece;
using UX.UI.Army.DesignArmy;
using Game.Relics;
using Game.Save.Army;
using Game.Save.Relics;
using UnityEngine;
using UnityEngine.InputSystem;
using Game.Managers;
using LineupConfig = Game.Save.Stage.LineupConfig;
using Game.ScriptableObjects.Collections;

namespace UX.UI.FreePlayTest
{
    public class FreePlayArmyBoard : ArmyDesignBoard
    {

        public ArmyDesignTroop troopDisplay;
        public List<Troop> EnemyTroops;

        public override void Load(int boardSize)
        {
            base.Load(boardSize);
            // Black pieces are preloaded from Config
            EnemyTroops = new List<Troop>();
            foreach (PieceConfig config in Config.PieceConfigBlack)
            {
                Troop troop = new Troop(config.Type, config.Index / Config.boardSize, config.Index % Config.boardSize);
                EnemyTroops.Add(troop);
                var piece = Instantiate(troopDisplay, childSquares[troop.Rank * size + troop.File].transform).GetComponent<FreePlayArmyTroop>();
                // Debug.Log($"ArmySearcher: {ArmySearcher.Ins}");
                // Debug.Log($"piece : {piece}");
                // Debug.Log()
                piece.Load(AssetManager.Ins.PieceData[troop.PieceType]);
                piece.Set(troop.Rank, troop.File);
                piece.Removable = false;
                piece.Placed = true;
            }
        }
        public (Troop troop, bool side) GetTroopByCoordinate(int rank, int file)
        {
            foreach (Troop tr in Troops)
            {
                if (tr.Rank == rank && tr.File == file)
                {
                    return (tr, false);
                }
            }
            foreach (Troop tr in EnemyTroops)
            {
                if (tr.Rank == rank && tr.File == file)
                {
                    return (tr, true);
                }
            }
            Debug.LogError($"GetTroopByCoordinate: False coordinate {rank} {file}");
            return (new Troop(PieceType.Velkaris, -1, -1), false);
        }
        public override void LoadSave(Troop[] troops)
        {
            foreach (var troop in troops)
            {
                Add(troop.Rank, troop.File, troop.PieceType);
                var piece = Instantiate(ArmyDesign.Ins.troopDisplay, childSquares[troop.Rank * size + troop.File].transform).GetComponent<FreePlayArmyTroop>();

                piece.Load(AssetManager.Ins.PieceData[troop.PieceType]);
                piece.Set(troop.Rank, troop.File);
                piece.Placed = true;
                piece.Removable = true;
            }

        }  
    }
}