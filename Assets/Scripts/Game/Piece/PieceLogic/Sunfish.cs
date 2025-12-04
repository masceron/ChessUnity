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
    public class Sunfish : Commons.PieceLogic, IPieceWithSkill
    {
        public Sunfish(PieceConfig cfg) : base(cfg, VersatileDefenderMove.Quiets, VersatileDefenderMove.Captures)
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Shield(this)));
            ActionManager.EnqueueAction(new ApplyEffect(new SunfishPassive(this)));

            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown > 0) return;
                if (isPlayer)
                {
                    if (SkillCooldown == 0)
                    {
                        list.Add(new SunfishActive(Pos, 4));
                    }

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
