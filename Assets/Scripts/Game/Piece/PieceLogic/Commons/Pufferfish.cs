using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Data.Pieces;
using Game.Effects.Traits;
using Game.Moves;

namespace Game.Piece.PieceLogic.Commons
{
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

        protected override void MoveToMake(List<Action.Action> list)
        {
            Quiets(list, Pos);
            Captures(list, Pos);
            Skills(list);
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}