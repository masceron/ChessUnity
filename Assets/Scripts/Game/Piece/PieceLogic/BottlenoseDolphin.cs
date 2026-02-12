using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Effects.Condition;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using Game.Common;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class BottlenoseDolphin : Commons.PieceLogic, IPieceWithSkill
    {
        public BottlenoseDolphin(PieceConfig cfg) : base(cfg, KingMoves.Quiets, KingMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new QuickReflex(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new BottlenoseDolphinPassive(this)));
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;
                if (isPlayer)
                {
                    var listPieces = SkillRangeHelper.GetActiveEnemyPieceGlobal(Pos);
                    foreach (var piece in listPieces)
                    {
                        list.Add(new BottlenoseDolphinActive(Pos, PieceOn(piece).Pos));
                    }
                }
                else
                {
                    //query for AI in here
                }

            };
        }
        
        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}