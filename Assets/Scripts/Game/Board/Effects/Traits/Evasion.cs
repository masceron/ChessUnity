using Game.Board.Action;
using Game.Board.General;
using Game.Board.Piece.PieceLogic;
using static Game.Common.BoardUtils;

namespace Game.Board.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Evasion: Effect
    {
        private readonly int probability;

        public Evasion(sbyte duration, int probability, PieceLogic piece) : base(duration, 1, piece, EffectName.Evasion)
        {
            this.probability = probability;
        }

        public override void OnCall(Action.Action action)
        {
            if (action == null || action.To != Piece.Pos || action.Result == ActionResult.Failed) return;

            if (Distance(action.From, action.To) < 3) return;
            if (!MatchManager.Roll(probability)) return;
            
            action.Result = ActionResult.Failed;
        }

        public override string Description()
        {
            return string.Format(AssetManager.Ins.EffectData[EffectName].description, probability);
        }
    }
}