using Board.Interaction;
using Board.Tile;
using Core;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Board
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Board : MonoBehaviour
    {
        private const int MaxFile = 12;
        private const int MaxRank = 12; 
        private const int TypeofPieces = 5;
        
        [SerializeField] private Piece.PieceManager pieceManager;
        [SerializeField] private TileManager tileManager;
        private GameState gameState;

        private void ManagersSetup()
        {
            InteractionManager.Init(MaxRank, MaxFile, pieceManager, gameState);
        }

        private void InstantiateGameState()
        {
            gameState = new GameState(MaxRank, MaxFile, TypeofPieces);
        }

        private void InstantiateBoard(bool[] active)
        {
            tileManager.Spawn(MaxRank, MaxFile, active);
        }

        private void InstantiatePieces(PieceType[] config)
        {
            pieceManager.Spawn(MaxRank, MaxFile, config);
        }
    
        private void Awake()
        {
            InstantiateGameState();
            ManagersSetup();
            InstantiateBoard(Config.boardActive);
            InstantiatePieces(Config.boardConfig);
        }
    }
}
