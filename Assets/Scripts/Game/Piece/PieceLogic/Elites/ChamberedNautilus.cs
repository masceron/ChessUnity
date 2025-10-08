using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Effects.Buffs;
using Game.Effects.Condition;
using Game.Movesets;

namespace Game.Piece.PieceLogic.Elites
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ChamberedNautilus : PieceLogic, IPieceWithSkill
    {
        private sbyte timeToCooldown;

        public ChamberedNautilus(PieceConfig cfg) : base(cfg, BishopMoves.Quiets, BarracudaMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new ChamberedNautilusHunger(this)));
            
            Skills = list =>
            {
                if (SkillCooldown == 0) list.Add(new ChamberedNautilusActive(Pos));
            };
        }

        sbyte IPieceWithSkill.TimeToCooldown
        {
            get => timeToCooldown;
            set => timeToCooldown = value;
        }

        public SkillsDelegate Skills { get; }
    }
}