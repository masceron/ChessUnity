using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Data.Pieces;
using Game.Effects.Traits;
using Game.Movesets;

namespace Game.Piece.PieceLogic.Commons
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Pufferfish: PieceLogic, IPieceWithSkill
    {
        public Pufferfish(PieceConfig cfg) : base(cfg, PufferfishMoves.Quiets, PufferfishMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Dominator(this)));

            Skills = list =>
            {
                list.Add(new PufferfishExplode(Pos));
            };
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}