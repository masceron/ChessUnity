using System.Collections.Generic;
using Game.Action.Internal.Pending;
using Game.Action.Internal.Pending.Relic;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Relics.Commons;
using UnityEngine;
using UX.UI.Ingame;

namespace Game.Relics
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class EyeOfMimic : RelicLogic
    {
        public EyeOfMimic(RelicConfig config) : base(config)
        {
            type = config.Type;
            Color = config.Color;
            TimeCooldown = config.TimeCooldown; // Cooldown in turns
            currentCooldown = 0;
        }
        public override void Activate()
        {
            if (currentCooldown == 0)
            {
                
                foreach (var piece in MatchManager.Ins.GameState.PieceBoard)
                {
                    if (piece == null) continue;

                    TileManager.Ins.MarkAsMoveable(piece.Pos);
                    var pending = new EyeOfMimicPending(this, piece.Pos, piece.Color);
                    BoardViewer.ListOf.Add(pending);
                }

                BoardViewer.Selecting = -2;
                BoardViewer.SelectingFunction = 4;
            }
            else
            {
                Debug.Log("Eye of Mimic is on cooldown for " + currentCooldown + " more turns.");
            }
        }

        public override void ActiveForAI()
        {
            List<PieceLogic> ourPieces = new List<PieceLogic>();
            List<PieceLogic> enemyPieces = new List<PieceLogic>();
            
            PieceLogic ourPiece = null;
            PieceLogic enemyPiece = null;
            
            int minMoveset = int.MaxValue;
            int maxMoveset = int.MinValue;
            
            foreach (var piece in MatchManager.Ins.GameState.PieceBoard)
            {
                if (piece == null) continue;
                if (piece.Color == MatchManager.Ins.GameState.OurSide)
                {
                    int pieceMoveset = piece.GetQuitesValue();
                    if (pieceMoveset < minMoveset)
                    {
                        minMoveset = pieceMoveset;
                        ourPieces.Clear();
                        ourPieces.Add(piece);
                    }
                    else if (pieceMoveset == minMoveset)
                    {
                        ourPieces.Add(piece);
                    }
                }
                else
                {
                    int pieceMoveset = piece.GetQuitesValue();
                    if (pieceMoveset > maxMoveset)
                    {
                        maxMoveset = pieceMoveset;
                        enemyPieces.Clear();
                        enemyPieces.Add(piece);
                    }
                    else if (pieceMoveset == maxMoveset)
                    {
                        enemyPieces.Add(piece);
                    }
                }
            }

            if (ourPieces.Count == 0)
            {
                //
            }
            else if (ourPieces.Count == 1)
            {
                ourPiece = ourPieces[0];
            }
            else
            {
                ourPiece = ourPieces[Random.Range(0, ourPieces.Count)];
            }

            if (enemyPieces.Count == 0)
            {
                //
            }
            else if (enemyPieces.Count == 1)
            {
                enemyPiece = enemyPieces[0];
            }
            else
            {
                enemyPiece = enemyPieces[Random.Range(0, enemyPieces.Count)];
            }

            if (ourPiece != null && enemyPiece != null)
            {
                var ourPending = new EyeOfMimicPending(this, ourPiece.Pos, ourPiece.Color);
                if (ourPending is IPendingAble op)
                {
                    op.CompleteAction();
                }
                else
                {
                    BoardViewer.Ins.ExecuteAction(ourPending);
                }
                
                var enemyPending = new EyeOfMimicPending(this, enemyPiece.Pos, enemyPiece.Color);
                if (ourPending is IPendingAble ep)
                {
                    ep.CompleteAction();
                }
                else
                {
                    BoardViewer.Ins.ExecuteAction(enemyPending);
                }
            }
        }
    }
}
