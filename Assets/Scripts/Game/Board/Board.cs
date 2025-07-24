using Game.Board.Action;
using Game.Board.General;
using Game.Board.Piece;
using Game.Board.Tile;
using UnityEngine;

namespace Game.Board
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Board : MonoBehaviour
    {
        [SerializeField] private PieceManager pieceManager;
        [SerializeField] private TileManager tileManager;
        [SerializeField] public AssetManager assetManager;

        private void MatchMaker()
        {
            MatchManager.Init(tileManager, pieceManager, assetManager, new Config());
            ActionManager.Init();
        }
    
        private void Awake()
        {
            MatchMaker();
        }
    }
}
