using System.Collections;
using System.Collections.Generic;
using Game.Piece;
using Game.Save.Army;
using UnityEngine;
using UnityEngine.UI;

namespace UX.UI.Army.DesignArmy
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ArmyDesignBoard: MonoBehaviour
    {
        [SerializeField] private RectTransform mainTransform;
        [SerializeField] private GridLayoutGroup grid;
        [SerializeField] private ArmyDesignSquare square;
        private int size;
        private BitArray allowed;

        private List<ArmyDesignSquare> childSquares;
        public List<Troop> troops;
        public void Load(int boardSize)
        {
            troops = new List<Troop>();
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
            var rankStart = size / 2;
            
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

        public void LoadSave(Troop[] _troops)
        {
            var searcher = FindAnyObjectByType<ArmySearcher>();
            foreach (var troop in _troops)
            {
                troops.Add(troop);
                var piece = Instantiate(searcher.troopDisplay, childSquares[troop.Rank * size + troop.File].transform).GetComponent<ArmyDesignTroop>();
                
                piece.Load(searcher.data[troop.Type]);
                piece.Set(troop.Rank, troop.File);
                piece.set = true;
            }
        }

        public void Add(int rank, int file, PieceType type)
        {
            troops.Add(new Troop(type, rank, file));
        }

        public void Move(int oldR, int oldF, int newR, int newF)
        {
            var old = troops.FindIndex(t => t.Rank == oldR && t.File == oldF);
            troops[old] = new Troop(troops[old].Type, newR, newF);
        }

        public void Remove(int r, int f)
        {
            troops.RemoveAt(troops.FindIndex(t => t.Rank == r && t.File == f));
        }
    }
}