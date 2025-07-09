using System.Collections;
using System.Threading;
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
            gameState = new GameState(MaxRank, MaxFile, Config.PieceConfig, Config.BoardActive, Color.Black, Color.Black);
        }

        private void InstantiateBoard(BitArray active)
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
            InstantiateBoard(gameState.ActiveBoard);
            InstantiatePieces(gameState.MainBoard);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                gameState.OurSide = gameState.OurSide == Color.White ? Color.Black : Color.White;
                ActionManager.Execute(gameState, new SwitchSide());
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                if (InteractionManager.SelectingPiece == -1) return;
                switch (gameState.MainBoard[InteractionManager.SelectingPiece].Type)
                {
                    case PieceType.Velkaris:
                    {
                        var des = InteractionManager.ActionToTake.Find(action => action.GetType() == typeof(VelkarisKill));
                        if (des != null)
                        {
                            ActionManager.Execute(gameState, des);
                        }

                        break;
                    }
                    case PieceType.GuidingSiren when InteractionManager.SelectPieceLock == null:
                    {
                        InteractionManager.UnmarkPiece(InteractionManager.SelectingPiece);
                        InteractionManager.MarkPiece(InteractionManager.SelectingPiece, typeof(SirenActive));
                        Debug.Log("Siren force.");
                        break;
                    }
                    case PieceType.GuidingSiren:
                        InteractionManager.SelectPieceLock = null;
                        InteractionManager.UnmarkPiece(InteractionManager.SelectingPiece);
                        break;
                }
            }
            
        }
    }
}
