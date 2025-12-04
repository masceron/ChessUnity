using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Pufferfish: Commons.PieceLogic, IPieceWithSkill
    {
        public Pufferfish(PieceConfig cfg) : base(cfg, PufferfishMoves.Quiets, PufferfishMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Dominator(this)));

            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown > 0) return;
                if (isPlayer)
                {
                    list.Add(new PufferfishExplode(Pos));
                } else
                {
                    //query for AI in here
                }
            };
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}