using Game.Action;
using Game.Action.Internal;
using Game.Action.Internal.Pending.Piece;
using Game.Action.Skills;
using Game.Common;
using Game.Effects.Buffs;
using Game.Managers;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;

namespace Game.Piece.PieceLogic
{
    public class RibbonEel : Commons.PieceLogic, IPieceWithSkill
    {
        private sbyte timeToCooldown;

        public RibbonEel(PieceConfig cfg) : base(cfg, AmbushPredatorMoves.Quiets, KingMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Camouflage(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new TrueBite(this)));

            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;
                if (isPlayer)
                {
                    foreach (var markedPiece in BoardUtils.FindPiecesWithEffectName(Color, "effect_marked"))
                    {
                        if (MatchManager.Ins.GameState.IsDay)
                        {
                            // For Diurnal
                            list.Add(new RibbonEelPendingForChooseTarget(markedPiece.Pos, Pos));
                        }
                    }
                }
                else
                {

                }
            };
        }

        sbyte IPieceWithSkill.TimeToCooldown
        {
            get => timeToCooldown;
            set => timeToCooldown = value;
        }

        public SkillsDelegate Skills { get; }
    }
}