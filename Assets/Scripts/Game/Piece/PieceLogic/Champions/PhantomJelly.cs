using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Effects.Buffs;
using Game.Effects.Debuffs;
using Game.Effects.Traits;
using Game.Movesets;

namespace Game.Piece.PieceLogic.Champions
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class PhantomJelly: PieceLogic, IPieceWithSkill
    {
        public PhantomJelly(PieceConfig cfg) : base(cfg, KingMoves.Quiets, PhantomJellyMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Evasion(-1, 50, this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Camouflage(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Controlled(2, this)));
            Skills = list =>
            {
                if (SkillCooldown == 0) list.Add(new PhantomJellyActive(Pos));
            };
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}