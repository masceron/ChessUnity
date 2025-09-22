using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Relics;

namespace Game.Piece.PieceLogic.Commanders
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Velkaris : PieceLogic, IPieceWithSkill, IRelicCarriable
    {
        public PieceLogic Marked;

        public Velkaris(PieceConfig cfg, RelicLogic carriedRelic = null) : base(cfg, RookMoves.Quiets, RookMoves.Captures)
        {
            Marked = null;
            SkillCooldown = -1;
            ActionManager.ExecuteImmediately(new ApplyEffect(new VelkarisMarker(this)));
            
            Skills = list =>
            {
                if (SkillCooldown == 0 && Marked != null)
                {
                    list.Add(new VelkarisKill(Pos, Pos, Marked.Pos));
                }
            };

            CarriedRelic = carriedRelic;
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
        public RelicLogic CarriedRelic { get; set; }
    }
}