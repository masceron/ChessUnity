using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Data.Pieces;
using Game.Effects.Traits;
using Game.Movesets;

namespace Game.Piece.PieceLogic.Elites
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ElectricEel : PieceLogic, IPieceWithSkill
    {
        public ElectricEel(PieceConfig cfg) : base(cfg, ElectricEelMoves.Quiets, ElectricEelMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new ElectricEelVengeful(this)));
            
            Skills = list =>
            {
                if (SkillCooldown == 0)
                {
                    list.Add(new ElectricEelActive(Pos));
                }
            };
        }

        

        protected override void CustomBehaviors(List<Action.Action> list)
        {
            Skills(list);
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}