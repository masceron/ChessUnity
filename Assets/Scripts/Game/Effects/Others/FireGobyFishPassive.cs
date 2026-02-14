using System.Collections.Generic;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using UnityEngine;

namespace Game.Effects.Others
{
    public class FireGobyFishPassive : Effect, IAfterPieceActionTrigger
    {
        public FireGobyFishPassive(PieceLogic piece) : base(-1, 1, piece, "effect_fire_goby_fish_passive")
        {
        }


        public AfterActionPriority Priority => AfterActionPriority.Debuff;

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is not ICaptures || action.Maker != Piece.Pos || action.Result != ResultFlag.Success) return;
            var targetPos = action.Target;
            var (rank, file) = BoardUtils.RankFileOf(targetPos);
            List<PieceLogic> surroundingPieces = new();
            foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 1))
            {
                var piecePos = BoardUtils.IndexOf(rankOff, fileOff);
                var piece = BoardUtils.PieceOn(piecePos);

                if (piece == null) continue;
                surroundingPieces.Add(piece);
            }

            var index = Random.Range(0, surroundingPieces.Count);
            ActionManager.EnqueueAction(new ApplyEffect(new Silenced(surroundingPieces[index]), Piece));
        }
    }
}