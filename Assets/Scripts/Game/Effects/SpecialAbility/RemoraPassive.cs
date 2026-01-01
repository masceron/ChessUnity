using System.Collections.Generic;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Common;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Effects.SpecialAbility
{
    public class RemoraPassive: Effect, IOnMoveGenEffect, IAfterPieceActionEffect
    {
        public RemoraPassive(PieceLogic piece) : base(-1, 1, piece, "effect_remora_passive")
        {
        }

        public void OnCallMoveGen(PieceLogic caller, List<Action.Action> actions)
        {
            if (caller != Piece) return;
            for (var i = 0; i < actions.Count; i++)
            {
                if (actions[i] is NormalMove)
                {
                    actions[i] = new RemoraMove(Piece.Pos, actions[i].Target);
                }
                else if (actions[i] is NormalCapture)
                {
                    actions[i] = new RemoraMark(Piece.Pos, actions[i].Target);
                }
            }
        }

        public void OnCallAfterPieceAction(Action.Action action)
        {
            foreach (var (rank, file) in MoveEnumerators.Around(RankOf(Piece.Pos), FileOf(Piece.Pos), 1))
            {
                var idx = IndexOf(rank, file);
                var pOn = PieceOn(idx);
                if (pOn == null) continue;
                
                if (pOn.Color == Piece.Color)
                {
                    ActionManager.EnqueueAction(new Purify(Piece.Pos, idx));
                }
                else
                {
                    ActionManager.EnqueueAction(new Nullify(Piece.Pos, idx));
                }
            }
        }
    }
}