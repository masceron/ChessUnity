using System.Collections;
using System.Collections.Generic;
using System;
using Game.Piece;
using Game.Save.Army;
using Game.Common;
using Game.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;
using static Game.Common.BoardUtils;
using Game.Managers;

namespace UX.UI.Army.DesignArmy
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ArmyDesignBoard: MonoBehaviour
    {
        [SerializeField] private RectTransform mainTransform;
        [SerializeField] private GridLayoutGroup grid;
        [SerializeField] private ArmyDesignSquare square;
        protected int size;
        protected BitArray allowed;
        protected List<ArmyDesignSquare> childSquares;
        public List<Troop> Troops;
        public Action<Troop> OnAddTroop, OnRemoveTroop;
        public virtual void Load(int boardSize)
        {
            Troops = new List<Troop>();
            size = boardSize;
            if (childSquares != null)
            {
                foreach (var sq in childSquares)
                {
                    Destroy(sq.gameObject);
                }
                childSquares.Clear();
            }
            else
            {
                childSquares = new List<ArmyDesignSquare>();
            }
            
            allowed = new BitArray(boardSize * boardSize);
            
            var gridSize = mainTransform.rect.width / boardSize;
            grid.cellSize = new Vector3(gridSize, gridSize);
            
            for (var i = 0; i < boardSize; i++)
            {
                for (var j = 0; j < boardSize; j++)
                {
                    if ((i + j) % 2 == 0)
                    {
                        var ws = Instantiate(square.gameObject, transform).GetComponent<ArmyDesignSquare>();
                        ws.name = $"White({i},{j})";
                        ws.SetSquare(i, j, gridSize, false);
                        childSquares.Add(ws);
                    }
                    else
                    {
                        var bs = Instantiate(square.gameObject, transform).GetComponent<ArmyDesignSquare>();
                        bs.name = $"Black({i},{j}";
                        bs.SetSquare(i, j, gridSize, true);
                        childSquares.Add(bs);
                    }
                }
            }
        }
        
        public bool IsAllowed(int rank, int file)
        {
            return allowed[rank * size + file];
        }

        public void SetAllowed()
        { 
            int rankStart = size / 2;
            
            for (var i = 0; i < rankStart; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    childSquares[i * size + j].MarkAsNotAllowed();
                }
            }
            
            for (var i = rankStart; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    if (i < size - 2)
                    {
                        childSquares[i * size + j].MarkAsNotAllowed();
                    }
                    else
                    {
                        var idx = i * size + j;
                        var sq = childSquares[idx];
                        //không được phép đặt 2 quân chồng lên nhau
                        if (sq.transform.childCount > 0) sq.MarkAsNotAllowed(); 
                        else
                        {
                            allowed[idx] = true;
                        }
                    }
                }
            }
        }

        public void UnSet()
        {
            allowed.SetAll(false);
            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    childSquares[i * size + j].UnMark();
                }
            }
        }

        public virtual void LoadSave(Troop[] troops)
        {

            foreach (var troop in troops)
            {
                Add(troop.Rank, troop.File, troop.PieceType);
                var piece = Instantiate(ArmyDesign.Ins.troopDisplay, childSquares[troop.Rank * size + troop.File].transform).GetComponent<ArmyDesignTroop>();

                piece.Load(AssetManager.Ins.PieceData[troop.PieceType]);
                piece.Set(troop.Rank, troop.File);
                piece.Placed = true;
            }
        }
        public void Add(int rank, int file, PieceType type)
        {
            Troop newTroop = new Troop(type, rank, file);
            Troops.Add(newTroop);
            OnAddTroop?.Invoke(newTroop);
        }

        public void Move(int oldR, int oldF, int newR, int newF)
        {
            var old = Troops.FindIndex(t => t.Rank == oldR && t.File == oldF);
            Troops[old] = new Troop(Troops[old].PieceType, newR, newF);
        }

        public void Remove(int r, int f)
        {
            int removedIndex = Troops.FindIndex(t => t.Rank == r && t.File == f);
            Troop removedTroop = Troops[removedIndex];
            Troops.RemoveAt(removedIndex);
            OnRemoveTroop?.Invoke(removedTroop);
        }
    }
}