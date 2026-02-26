using System;
using System.Collections;
using System.Collections.Generic;
using Game.Managers;
using Game.Save.Army;
using UnityEngine;
using UnityEngine.UI;

namespace UX.UI.FreePlayTest.DesignArmyScene
{
    public class FreePlayArmyBoard : MonoBehaviour
    {
        [SerializeField] private RectTransform mainTransform;
        [SerializeField] private GridLayoutGroup grid;
        [SerializeField] private FPSquare square;
        public FreePlayArmyTroop troopDisplay;
        protected BitArray allowed;
        protected List<FPSquare> childSquares;
        public List<Troop> EnemyTroops = new();
        public Action<Troop> OnAddTroop, OnRemoveTroop;
        protected int size;
        public List<Troop> Troops = new();

        // public override void Load(int boardSize)
        // {
        //     base.Load(boardSize);
        // Black pieces are preloaded from Config
        // EnemyTroops = new List<Troop>();
        // foreach (var config in Config.PieceConfigBlack)
        // {
        //     var troop = new Troop(config.Type, config.Index / Config.boardSize, config.Index % Config.boardSize);
        //     EnemyTroops.Add(troop);
        //     var piece = Instantiate(troopDisplay, childSquares[troop.Rank * size + troop.File].transform).GetComponent<FreePlayArmyTroop>();
        //     piece.Load(AssetManager.Ins.PieceData[troop.PieceType]);
        //     piece.Set(troop.Rank, troop.File);
        //     piece.Removable = false;
        //     piece.Placed = true;
        // }
        // }
        public (Troop troop, bool side) GetTroopByCoordinate(int rank, int file)
        {
            foreach (var tr in Troops)
                if (tr.Rank == rank && tr.File == file)
                    return (tr, false);

            foreach (var tr in EnemyTroops)
                if (tr.Rank == rank && tr.File == file)
                    return (tr, true);

            Debug.LogError($"GetTroopByCoordinate: False coordinate {rank} {file}");
            return (new Troop("piece_velkaris", -1, -1), false);
        }

        public void LoadSave(Troop[] troops, Troop[] enemyTroops, bool disableInteraction = false)
        {
            foreach (var troop in troops)
            {
                Add(troop.Rank, troop.File, troop.PieceType);
                var piece = Instantiate(troopDisplay, childSquares[troop.Rank * size + troop.File].transform);
                piece.Load(AssetManager.Ins.PieceData[troop.PieceType]);
                piece.Set(troop.Rank, troop.File);
                piece.Placed = true;
                piece.enabled = !disableInteraction;
            }

            foreach (var troop in enemyTroops)
            {
                Add(troop.Rank, troop.File, troop.PieceType);
                var piece = Instantiate(troopDisplay, childSquares[troop.Rank * size + troop.File].transform);
                piece.Load(AssetManager.Ins.PieceData[troop.PieceType]);
                piece.Set(troop.Rank, troop.File);
                piece.Placed = true;
                piece.enabled = !disableInteraction;
            }
        }

        public virtual void Load(int boardSize)
        {
            Troops = new List<Troop>();
            size = boardSize;
            if (childSquares != null)
            {
                foreach (var sq in childSquares) Destroy(sq.gameObject);
                childSquares.Clear();
            }
            else
            {
                childSquares = new List<FPSquare>();
            }

            allowed = new BitArray(boardSize * boardSize);

            var gridSize = mainTransform.rect.width / boardSize;
            grid.cellSize = new Vector3(gridSize, gridSize);

            for (var i = 0; i < boardSize; i++)
            for (var j = 0; j < boardSize; j++)
                if ((i + j) % 2 == 0)
                {
                    var ws = Instantiate(square.gameObject, transform).GetComponent<FPSquare>();
                    ws.name = $"White({i},{j})";
                    ws.SetSquare(i, j, gridSize, false);
                    childSquares.Add(ws);
                }
                else
                {
                    var bs = Instantiate(square.gameObject, transform).GetComponent<FPSquare>();
                    bs.name = $"Black({i},{j}";
                    bs.SetSquare(i, j, gridSize, true);
                    childSquares.Add(bs);
                }
        }

        public bool IsAllowed(int rank, int file)
        {
            return allowed[rank * size + file];
        }

        public void SetAllowed(bool isContruct)
        {
            for (var i = 0; i < size; i++)
            for (var j = 0; j < size; j++)
            {
                var idx = i * size + j;
                var sq = childSquares[idx];

                // Không được phép đặt 2 quân chồng lên nhau
                if (sq.transform.childCount > 0)
                    sq.MarkAsNotAllowed();
                else if (!isContruct && i >= 2 && i < size - 2)
                    // Nếu không phải construct, chỉ cho phép 2 hàng trên (0,1) và 2 hàng dưới (size-2, size-1)
                    sq.MarkAsNotAllowed();
                else
                    allowed[idx] = true;
            }
        }

        public void UnSet()
        {
            allowed.SetAll(false);
            for (var i = 0; i < size; i++)
            for (var j = 0; j < size; j++)
                childSquares[i * size + j].UnMark();
        }

        public void Add(int rank, int file, string type)
        {
            if (IsInEnemySite(rank))
            {
                var newTroop = new Troop(type, rank, file);
                EnemyTroops.Add(newTroop);
                OnAddTroop?.Invoke(newTroop);
            }
            else
            {
                var newTroop = new Troop(type, rank, file);
                Troops.Add(newTroop);
                OnAddTroop?.Invoke(newTroop);
            }
        }

        public void Move(int oldR, int oldF, int newR, int newF)
        {
            if (IsInEnemySite(oldR))
            {
                var old = EnemyTroops.FindIndex(t => t.Rank == oldR && t.File == oldF);
                EnemyTroops[old] = new Troop(EnemyTroops[old].PieceType, newR, newF);
            }
            else
            {
                var old = Troops.FindIndex(t => t.Rank == oldR && t.File == oldF);
                Troops[old] = new Troop(Troops[old].PieceType, newR, newF);
            }
        }

        public void Remove(int r, int f)
        {
            if (IsInEnemySite(r))
            {
                var removedIndex = EnemyTroops.FindIndex(t => t.Rank == r && t.File == f);
                if (removedIndex != -1)
                {
                    var removedTroop = EnemyTroops[removedIndex];
                    EnemyTroops.RemoveAt(removedIndex);
                    OnRemoveTroop?.Invoke(removedTroop);
                }
            }
            else
            {
                var removedIndex = Troops.FindIndex(t => t.Rank == r && t.File == f);
                if (removedIndex != -1)
                {
                    var removedTroop = Troops[removedIndex];
                    Troops.RemoveAt(removedIndex);
                    OnRemoveTroop?.Invoke(removedTroop);
                }
            }
        }

        public bool IsInEnemySite(int rank)
        {
            return rank <= FPArmyDesign.Ins.size / 2 - 1;
        }
    }
}