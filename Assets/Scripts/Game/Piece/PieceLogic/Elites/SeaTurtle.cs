using Game.Action.Skills;
using Game.Movesets;
using Game.Effects.Buffs;
using Game.Action.Internal;
using Game.Action;

namespace Game.Piece.PieceLogic.Elites
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SeaTurtle : PieceLogic, IPieceWithSkill
    {
        public SeaTurtle(PieceConfig cfg) : base(cfg, SeaTurtleMoves.Quiets, SeaTurtleMoves.Captures)
        { 
            ActionManager.EnqueueAction(new ApplyEffect(new Camouflage(this)));
            Skills = list =>
            {
                if (SkillCooldown == 0) {
                    list.Add(new SeaTurtleActive(Pos));
                }
            };
            
        }
        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}