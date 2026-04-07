using System.Collections.Generic;
using Game.Action.Internal.Pending.Relic;
using Game.Action.Relics;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Relics.Commons;
using UnityEngine;
using UX.UI.Ingame;

namespace Game.Relics
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class EyeOfMimic : RelicLogic
    {
        public EyeOfMimic(RelicConfig config) : base(config)
        {
            Type = config.Type;
            Color = config.Color;
            TimeCooldown = config.TimeCooldown; // Cooldown in turns
            CurrentCooldown = 0;
        }

        public override void Activate()
        {
            if (CurrentCooldown == 0)
            {
                foreach (var piece in BoardUtils.PieceBoard())
                {
                    if (piece == null) continue;

                    TileManager.Ins.MarkAsMoveable(piece.Pos);
                    //Làm lại
                    //var pending = new EyeOfMimicPending(this, piece.Pos);
                    //BoardViewer.ListOf.Add(pending);
                }

                BoardViewer.Selecting = -2;
                BoardViewer.SelectingFunction = 4;
            }
            else
            {
                Debug.Log("Eye of Mimic is on cooldown for " + CurrentCooldown + " more turns.");
            }
        }

        public override void ActiveForAI()
        {
            var ourPieces = new List<PieceLogic>();
            var enemyPieces = new List<PieceLogic>();

            PieceLogic ourPiece = null;
            PieceLogic enemyPiece = null;

            var minMoveset = int.MaxValue;
            var maxMoveset = int.MinValue;

            foreach (var piece in BoardUtils.PieceBoard())
            {
                if (piece == null) continue;
                if (piece.Color == BoardUtils.OurSide())
                {
                    var pieceMoveset = piece.GetQuitesValue();
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
                    var pieceMoveset = piece.GetQuitesValue();
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

            switch (ourPieces.Count)
            {
                case 0:
                    break;
                case 1:
                    ourPiece = ourPieces[0];
                    break;
                default:
                    ourPiece = ourPieces[Random.Range(0, ourPieces.Count)];
                    break;
            }

            switch (enemyPieces.Count)
            {
                case 0:
                    break;
                case 1:
                    enemyPiece = enemyPieces[0];
                    break;
                default:
                    enemyPiece = enemyPieces[Random.Range(0, enemyPieces.Count)];
                    break;
            }

            if (ourPiece == null || enemyPiece == null) return;
            var excute = new EyeOfMimicExecute(ourPiece, enemyPiece);
            BoardViewer.Ins.ExecuteAction(excute);

            // var ourPending = new EyeOfMimicPending(this, ourPiece.Pos);
            // BoardViewer.Ins.ExecuteAction(await ourPending.WaitForCompletion());
            // var enemyPending = new EyeOfMimicPending(this, enemyPiece.Pos);
            // BoardViewer.Ins.ExecuteAction(await enemyPending.WaitForCompletion());
        }
    }
}