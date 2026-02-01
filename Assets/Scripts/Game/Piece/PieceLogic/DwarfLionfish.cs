using Game.Action.Skills;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using Game.Action;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Common;
namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class DwarfLionfish : Commons.PieceLogic, IPieceWithSkill
    {
        public DwarfLionfish(PieceConfig cfg) : base(cfg, BishopMoves.Quiets, KingMoves.Captures)
        { 
            ActionManager.ExecuteImmediately(new ApplyEffect(new DwarfLionfishPassive(this)));
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown > 0) return;
                if (isPlayer)
                {
                    list.Add(new DwarfLionfishActive(Pos));
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