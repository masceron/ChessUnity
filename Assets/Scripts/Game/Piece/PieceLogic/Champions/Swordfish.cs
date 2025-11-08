using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Effects.Buffs;
using Game.Effects.Debuffs;
using Game.Effects.Traits;
using Game.Movesets;
using UnityEditor;
using UnityEditor.Timeline.Actions;

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

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}