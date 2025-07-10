using System.Collections;
using Board.Action;
using Board.Interaction;
using Board.Tile;
using Core;
using Core.General;
using Core.Piece;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using Color = Core.General.Color;

namespace Board
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Board : MonoBehaviour
    {
        [SerializeField] private Piece.PieceManager pieceManager;
        [SerializeField] private TileManager tileManager;
        private MatchManager matchManager;

        private void MatchMaker()
        {
            matchManager = new MatchManager(tileManager, pieceManager, new Config());
            InteractionManager.Init(matchManager);
            ActionManager.Init(matchManager.GameState, matchManager);
        }

        private void InstantiateBoard(BitArray active)
        {
            tileManager.Spawn(matchManager.GameState.MaxRank, matchManager.GameState.MaxFile, active);
        }

        private void InstantiatePieces(PieceLogic[] config)
        {
            pieceManager.Spawn(matchManager.GameState.MaxRank, matchManager.GameState.MaxFile, config);
        }
    
        private void Awake()
        {
            MatchMaker();
            InstantiateBoard(matchManager.GameState.ActiveBoard);
            InstantiatePieces(matchManager.GameState.MainBoard);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                matchManager.GameState.OurSide = matchManager.GameState.OurSide == Color.White ? Color.Black : Color.White;
                ActionManager.Execute(new EndTurn());
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                if (InteractionManager.SelectingPiece == -1) return;
                switch (matchManager.GameState.MainBoard[InteractionManager.SelectingPiece].Type)
                {
                    case PieceType.Velkaris:
                    {
                        var des = InteractionManager.ActionToTake.Find(action => action.GetType() == typeof(VelkarisKill));
                        if (des != null)
                        {
                            ActionManager.Execute(des);
                        }

                        break;
                    }
                    case PieceType.GuidingSiren when InteractionManager.SelectPieceLock == null:
                    {
                        InteractionManager.UnmarkPiece(InteractionManager.SelectingPiece);
                        InteractionManager.MarkPiece(InteractionManager.SelectingPiece, typeof(SirenActive));
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
