using System.Collections.Generic;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Piece.PieceLogic;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Demolisher: Effect, IAfterPieceActionEffect, IOnMoveGenEffect
    {
        public Demolisher(PieceLogic piece) : base(-1, 1, piece, "effect_demolisher")
        { }
        
        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is DestroyConstruct && action.Maker == Piece.Pos && PieceOn(action.Maker).Type != "piece_rusty_parrotfish" && action.Result == ResultFlag.Success)
            {
                if (Piece is RedtailParrotfish && PieceOn(action.Target) is StoneWall)
                {
                    return;
                }
                // Giết chính mình
                ActionManager.EnqueueAction(new KillPiece(Piece.Pos));
            }
        }
        public void OnCallMoveGen(PieceLogic caller, List<Action.Action> actions)
        {
            if (caller != Piece) return;
            for (var i = 0; i < actions.Count; i++)
            {
                if (actions[i] is ICaptures)
                    actions[i] = new DestroyConstruct(Piece.Pos, actions[i].Target);
            }
        }
        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 0;
        }
    }
}