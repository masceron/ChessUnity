using System.Collections.Generic;
using Game.Managers;
using Game.Save.Army;
using UnityEngine;
using UX.UI.Army.DesignArmy;

namespace UX.UI.FreePlayTest.DesignArmyScene
{
    public class FreePlayArmyBoard : ArmyDesignBoard
    {

        public FreePlayArmyTroop troopDisplay;
        public List<Troop> EnemyTroops;

        public override void Load(int boardSize)
        {
            base.Load(boardSize);
            // Black pieces are preloaded from Config
            EnemyTroops = new List<Troop>();
            foreach (var config in Config.PieceConfigBlack)
            {
                var troop = new Troop(config.Type, config.Index / Config.boardSize, config.Index % Config.boardSize);
                EnemyTroops.Add(troop);
                var piece = Instantiate(troopDisplay, childSquares[troop.Rank * size + troop.File].transform).GetComponent<FreePlayArmyTroop>();
                piece.Load(AssetManager.Ins.PieceData[troop.PieceType]);
                piece.Set(troop.Rank, troop.File);
                piece.Removable = false;
                piece.Placed = true;
            }
        }
        public (Troop troop, bool side) GetTroopByCoordinate(int rank, int file)
        {
            foreach (var tr in Troops)
            {
                if (tr.Rank == rank && tr.File == file)
                {
                    return (tr, false);
                }
            }
            foreach (var tr in EnemyTroops)
            {
                if (tr.Rank == rank && tr.File == file)
                {
                    return (tr, true);
                }
            }
            Debug.LogError($"GetTroopByCoordinate: False coordinate {rank} {file}");
            return (new Troop("piece_velkaris", -1, -1), false);
        }
        public override void LoadSave(Troop[] troops)
        {
            foreach (var troop in troops)
            {
                Add(troop.Rank, troop.File, troop.PieceType);
                var piece = Instantiate(troopDisplay, childSquares[troop.Rank * size + troop.File].transform);
                Debug.Log($"PieceType : {troop.PieceType}"); //Living Coral thay vi BobtailSquid
                piece.Load(AssetManager.Ins.PieceData[troop.PieceType]);
                piece.Set(troop.Rank, troop.File);
                piece.Placed = true;
                piece.Removable = true;
            }

        }  
    }
}