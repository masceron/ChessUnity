using Game.Action;
using Game.Action.Skills;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Movesets;


namespace Game.Piece.PieceLogic.Elites
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class BobtailSquid: PieceLogic, IPieceWithSkill
    {
        public BobtailSquid(PieceConfig cfg) : base(cfg, QueenMoves.Quiets, QueenMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Evasion(-1, 25, this)));
            
            Skills = list =>
            {
                if (SkillCooldown == 0)
                {
                    list.Add(new BobtailSquidActive(Pos, Pos));
                }
            };
        }
        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}