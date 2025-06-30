using Board.Action;
using Board.Interaction;
using Board.Tile;
using Core;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using Color = Core.Color;

namespace Board
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Board : MonoBehaviour
    {
        private const int MaxFile = 12;
        private const int MaxRank = 12; 
        
        [SerializeField] private Piece.PieceManager pieceManager;
        [SerializeField] private TileManager tileManager;
        private GameState gameState;

        private void ManagersSetup()
        {
            InteractionManager.Init(MaxRank, MaxFile, tileManager, pieceManager, gameState);
        }

        private void InstantiateGameState()
        {
            gameState = new GameState(MaxRank, MaxFile, Config.pieceConfig, Config.boardActive, Color.Black, Color.Black);
        }

        private void InstantiateBoard(byte[] active)
        {
            tileManager.Spawn(MaxRank, MaxFile, active);
        }

        private void InstantiatePieces(PieceData[] config)
        {
            pieceManager.Spawn(MaxRank, MaxFile, config);
        }
    
        private void Awake()
        {
            InstantiateGameState();
            ManagersSetup();
            InstantiateBoard(gameState.Position.active_board);
            InstantiatePieces(gameState.Position.main_board);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                gameState.OurSide = gameState.OurSide == Color.White ? Color.Black : Color.White;
                ActionManager.Execute(gameState, new SwitchSide());
                Debug.Log(gameState.Position.side_to_move);
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                if (InteractionManager.selectingPiece == -1) return;
                var des = InteractionManager.ActionToTake.Find(action => action.Move.flag == MoveFlag.VelkarisKill);
                if (des != null)
                {
                    ActionManager.Execute(gameState, des);
                }
            }
            
        }
    }
}
