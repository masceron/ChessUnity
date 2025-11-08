using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Piece.PieceLogic;
using static Game.Common.BoardUtils;
using System.Collections.Generic;
using System.Linq;
using System;
using Game.Managers;
using Game.Piece;
using Game.Effects.Others;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class  BlackSwallowerVengeful: Effect
    {
        public BlackSwallowerVengeful(PieceLogic piece) : base(-1, 1, piece, EffectName.BlackSwallowerVengeful)
        {
        }

        public override void OnCallPieceAction(Action.Action action)    
        {
            if (action == null) return;
            
            if (action.Result == ActionResult.Succeed)
            {
                if (action.Target == Piece.Pos)
                {
                    List<PieceLogic> pieceAround = new List<PieceLogic>();
                    foreach (var (rank, file) in MoveEnumerators.Around(RankOf(Piece.Pos), FileOf(Piece.Pos), 1))
                    {
                        var index = IndexOf(rank, file);
                        var pOn = PieceOn(index);
                        if (pOn == null || pOn == Piece) continue;
                        pieceAround.Add(pOn);
                    }
                    if (pieceAround.Count == 0) return;
                    var random = new Random();
                    var randomPiece = pieceAround[random.Next(0, pieceAround.Count)];
                    if (randomPiece == null) return;
                    foreach (var effect in randomPiece.Effects.Where(effect => effect.Category == EffectCategory.Buff))
                    {
                        ActionManager.EnqueueAction(new RemoveEffect(effect));
                    }
                }
                else if (action.Target != Piece.Pos)
                {
                    var targetPiece = PieceOn(action.Target);
                    if (targetPiece != null && (targetPiece.PieceRank == PieceRank.Elite || targetPiece.PieceRank == PieceRank.Champion || targetPiece.PieceRank == PieceRank.Commander))
                    {
                        ActionManager.EnqueueAction(new ApplyEffect(new KillPieceAfterSwitchTurn(Piece)));
                        
                    }
                }
            }
        }
    }
}