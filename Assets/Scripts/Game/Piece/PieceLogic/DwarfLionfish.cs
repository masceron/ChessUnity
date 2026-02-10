using Game.Action.Skills;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using Game.Action;
using Game.Action.Internal;
using Game.Effects.Traits;

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
                    var (rank, file) = RankFileOf(Pos);
                    var listA = GetPiecesInRadius(rank, file, 1, p => 
                        p != null && p.Color != Color && 
                        (p.Effects == null || !p.Effects.Any(e => e.EffectName == "effect_extremophile")));
                    
                    if (listA.Count >= 1)
                    {
                        if (!excludeEmptyTile)
                        {
                            list.Add(new DwarfLionfishActive(Pos));
                        }
                        else
                        {
                            list.Add(new DwarfLionfishActive(Pos));
                        }
                    }
                }
            };
            
        }
        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}