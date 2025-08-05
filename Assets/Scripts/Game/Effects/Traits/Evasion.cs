using Game.Action;
using Game.Managers;
using Game.Piece.PieceLogic;
using static Game.Common.BoardUtils;

namespace Game.Effects.Traits
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
            if (action == null || action.Target != Piece.Pos || action.Result == ActionResult.Failed) return;

            if (Distance(action.Maker, action.Target) < 3) return;
            if (!MatchManager.Roll(probability)) return;
            
            action.Result = ActionResult.Failed;
        }

        public override string Description()
        {
            return string.Format(AssetManager.Ins.EffectData[EffectName].description, probability);
        }
    }
}