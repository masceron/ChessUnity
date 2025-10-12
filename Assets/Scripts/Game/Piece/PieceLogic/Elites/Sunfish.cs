using Game.Action.Skills;
using Game.Movesets;
using static Game.Common.BoardUtils;
using Game.Effects.Buffs;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Action;


namespace Game.Piece.PieceLogic.Elites
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Sunfish: PieceLogic, IPieceWithSkill
    {
        public Sunfish(PieceConfig cfg) : base(cfg, VersatileDefenderMove.Quiets, VersatileDefenderMove.Captures)
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Shield(this)));
            ActionManager.EnqueueAction(new ApplyEffect(new SunfishPassive(this)));

            Skills = list =>
            {
                if (SkillCooldown == 0) 
                {
                    list.Add(new SunfishActive(Pos, 4));
                }
            };
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}
