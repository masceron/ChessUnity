using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Effects.Buffs;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Swordfish: Commons.PieceLogic, IPieceWithSkill
    {
        public Swordfish(PieceConfig cfg) : base(cfg, QueenMoves.Quiets, QueenMoves.Captures)
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Piercing(-1, this)));
            ActionManager.EnqueueAction(new ApplyEffect(new SwordfishAttack(this)));
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown > 0) return;
                if (isPlayer)
                {
                    list.Add(new SwordFishActive(Pos));
                }
                else
                {
                    //query for AI in here
                }
            };
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}