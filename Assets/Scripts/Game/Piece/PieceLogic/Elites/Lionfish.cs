using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Data.Pieces;
using Game.Effects.Traits;
using Game.Moves;

namespace Game.Piece.PieceLogic.Elites
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Lionfish: PieceLogic, IPieceWithSkill
    {
        public Lionfish(PieceConfig cfg) : base(cfg, BishopMoves.Quiets, KingMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new LionfishVengeful(this)));

            Skills = list =>
            {
                if (SkillCooldown == 0) list.Add(new LionfishActive(Pos));
            };
        }

        
        protected override void MoveToMake(List<Action.Action> list)
        {
            Quiets(list, Pos);
            Captures(list, Pos);
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}