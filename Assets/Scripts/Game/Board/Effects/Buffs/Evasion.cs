using System;
using Game.Board.Action;
using Game.Board.General;
using Game.Board.Piece.PieceLogic;
using static Game.Common.BoardUtils;

namespace Game.Board.Effects.Buffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Evasion: Effect
    {
        public Evasion(sbyte duration, sbyte strength, PieceLogic piece) : base(duration, strength, piece, EffectType.Evasion)
        {}

        public override void OnCall(Action.Action action)
        {
            if (action.To != Piece.pos || action.Success == ActionResult.Failed) return;
            
            var rankDiff = Math.Abs(RankOf(action.From) - RankOf(action.To));
            var fileDiff = Math.Abs(FileOf(action.From) - FileOf(action.To));

            if (Math.Max(rankDiff, fileDiff) < 3) return;
            if (!MatchManager.Roll(Strength)) return;
            
            action.Success = ActionResult.Failed;

        }
    }
}