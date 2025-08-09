using System.Collections.Generic;
using Game.Common;
using UnityEngine;
using UnityEngine.UI;
using Color = Game.Managers.Color;

namespace UX.UI.Army.DesignArmy
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ArmyDesignBoard: MonoBehaviour
    {
        [SerializeField] private RectTransform mainTransform;
        [SerializeField] private GridLayoutGroup grid;
        [SerializeField] private UDictionary<Color, GameObject> squares;

        private List<ArmyDesignSquare> childSquares;
        public void Load(int boardSize)
        {
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
            
            var size = mainTransform.rect.width / boardSize;
            grid.cellSize = new Vector3(size, size);
            
            for (var i = 0; i < boardSize; i++)
            {
                for (var j = 0; j < boardSize; j++)
                {
                    if ((i + j) % 2 == 0)
                    {
                        var ws = Instantiate(squares[Color.White], transform).GetComponent<ArmyDesignSquare>();
                        ws.name = $"White({i},{j})";
                        ws.SetSquare(i, j, size);
                        childSquares.Add(ws);
                    }
                    else
                    {
                        var bs = Instantiate(squares[Color.Black], transform).GetComponent<ArmyDesignSquare>();
                        bs.name = $"Black({i},{j}";
                        bs.SetSquare(i, j, size);
                        childSquares.Add(bs);
                    }
                }
            }
        }
    }
}