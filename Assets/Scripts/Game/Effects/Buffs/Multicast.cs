using Game.Action.Skills;
using Game.Common;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using static Game.Common.BoardUtils;

namespace Game.Effects.Buffs
{
    public class Multicast : Effect, IOnApplyTrigger, IOnRemoveTrigger
    {
        private readonly int Number;
        public Multicast(PieceLogic piece, int number, int duration) : base(duration, -1, piece, "effect_multicast")
        {
            Number = number;
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() - 60;
        }

        void IOnApplyTrigger.OnApply()
        {
            if (Piece.GetStat(SkillStat.Target) != 0)
            {
                Piece.SetStat(SkillStat.Target, Piece.GetStat(SkillStat.Target) + Number);
            }

            if (Piece.GetStat(SkillStat.Unit) != 0)
            {
                Piece.SetStat(SkillStat.Unit, Piece.GetStat(SkillStat.Unit) + Number);
            }
        }

        void IOnRemoveTrigger.OnRemove()
        {
            if (Piece.GetStat(SkillStat.Target) != 0)
            {
                Piece.SetStat(SkillStat.Target, Piece.GetStat(SkillStat.Target) - Number);
            }

            if (Piece.GetStat(SkillStat.Unit) != 0)
            {
                Piece.SetStat(SkillStat.Unit, Piece.GetStat(SkillStat.Unit) - Number);
            }
        }
    }
}