using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Common;
using Game.Effects.Buffs;
using Game.Effects.SpecialAbility;
using Game.Effects.States;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class PaintedGreenling : Commons.PieceLogic
    {
        private const int Number = 1;
        private const int Duration = 3;
        private const int Radius = 1;
        public PaintedGreenling(PieceConfig cfg) : base(cfg, SpinningMoves.Quiets, None.Captures)
        {
            SetStat(SkillStat.Radius, Radius);
            SetStat(SkillStat.Duration, Duration);
            SetStat(SkillStat.Number, Number);

            ActionManager.EnqueueAction(new ApplyEffect(new PaintedGreenlingPassive(this)));
        }
    }
}