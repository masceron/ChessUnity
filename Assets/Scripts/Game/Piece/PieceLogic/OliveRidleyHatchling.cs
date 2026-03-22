using Game.Action;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Effects.SpecialAbility;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;

namespace Game.Piece.PieceLogic
{
    public class OliveRidleyHatchling : Commons.PieceLogic, IPieceWithSkill
    {
        private const int Counter = 10;
        public OliveRidleyHatchling(PieceConfig cfg) : base(cfg, VersatileDefenderMove.Quiets, None.Captures)
        {
            SetStat(SkillStat.Counter, Counter);
            ActionManager.EnqueueAction(new ApplyEffect(new Shield(this, -1)));
            ActionManager.EnqueueAction(new ApplyEffect(new OliveRidleyHatchlingPassive(this, GetStat(SkillStat.Counter))));

        }

        public void SetSkillHandler(SkillsDelegate skillHandler)
        {
            Skills = skillHandler;
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}