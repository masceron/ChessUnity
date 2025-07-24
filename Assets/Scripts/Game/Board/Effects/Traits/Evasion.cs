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
        private readonly int probability;

        public Evasion(sbyte duration, int probability, PieceLogic piece) : base(duration, 1, piece, EffectType.Evasion)
        {
            this.probability = probability;
        }

        public override void OnCall(Action.Action action)
        {
            if (action == null || action.To != Piece.pos || action.Result == ActionResult.Failed) return;
            
            var rankDiff = Math.Abs(RankOf(action.From) - RankOf(action.To));
            var fileDiff = Math.Abs(FileOf(action.From) - FileOf(action.To));

            if (Math.Max(rankDiff, fileDiff) < 3) return;
            if (!MatchManager.Roll(probability)) return;
            
            action.Result = ActionResult.Failed;
        }

        public override string Description()
        {
            return string.Format(MatchManager.assetManager.EffectData[EffectName].description, probability);
        }
    }
}