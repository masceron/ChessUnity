using Game.Action;
using Game.Action.Internal;
using Game.Effects.SpecialAbility;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;

namespace Game.Piece.PieceLogic
{ 
    public class ChainPickerel : Commons.PieceLogic, IPieceWithSkill
    {
        private const int Radius = 2;
        public ChainPickerel(PieceConfig cfg) : base(cfg, SmallChargingMoves.Quiets, None.Captures)
        {
            SetStat(SkillStat.Radius, Radius);
            ActionManager.EnqueueAction(new ApplyEffect(new ChainPickerelPassive(this, GetStat(SkillStat.Radius))));

        }

        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}