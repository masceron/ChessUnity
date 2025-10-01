using System.Collections.Generic;
using System.Linq;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Effects;
using Game.Effects.Buffs;
using Game.Effects.Traits;
using Game.Movesets;
using SnappingStrike = Game.Action.Captures.SnappingStrike;

namespace Game.Piece.PieceLogic.Champions
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class PhantomJelly: PieceLogic, IPieceWithSkill
    {
        public PhantomJelly(PieceConfig cfg) : base(cfg, QueenMoves.Quiets, QueenMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Evasion(-1, 50, this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Camouflage(this)));
            Skills = list =>
            {
                if (SkillCooldown == 0) list.Add(new PhantomJellyActive(Pos));
            };
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}