using System.Collections.Generic;
using System.Linq;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Data.Pieces;
using Game.Effects;
using Game.Effects.Buffs;
using Game.Effects.Traits;
using Game.Moves;
using SnappingStrike = Game.Action.Captures.SnappingStrike;

namespace Game.Piece.PieceLogic.Champions
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Swordfish: PieceLogic, IPieceWithSkill
    {
        public Swordfish(PieceConfig cfg) : base(cfg, QueenMoves.Quiets, QueenMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Piercing(-1, this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new SwordfishAttack(this)));
            Skills = list =>
            {
                if (SkillCooldown == 0) list.Add(new SwordFishActive(Pos));
            };
        }

        private bool snap;

        protected override void MoveToMake(List<Action.Action> list)
        {
            snap = Effects.Any(e => e.EffectName == EffectName.SnappingStrike);

            Quiets(list, Pos);
            Captures(list, Pos);
            Skills(list);

            if (!snap) return;
            
            for (var i = 0; i < list.Count; i++)
            {
                if (list[i] is NormalCapture)
                {
                    list[i] = new SnappingStrike(Pos, list[i].Target);
                }
            }
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}