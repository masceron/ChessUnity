using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Common;
using Game.Effects.SpecialAbility;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    public class DwarfCuttlefish : Commons.PieceLogic, IPieceWithSkill
    {
        private int timeToCooldown;
        private const int EvasionChance = 10;
        private const string Stunned = "effect_stunned";
        
        public DwarfCuttlefish(PieceConfig cfg) : base(cfg, SquidMoves.Quiets, SquidMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new QuickReflex(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Evasion(-1, EvasionChance, this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new DwarfCuttlefishPassive(this)));
            
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;
                if (isPlayer)
                {
                    var (rank, file) = RankFileOf(Pos);
                    switch (Color)
                    {
                        case false:
                            foreach (var (rankOff, fileOff) in MoveEnumerators.Up(rank, file, 1))
                            {
                                var targetPos = IndexOf(rankOff, fileOff);
                                if (!IsActive(targetPos)) continue;
                                var targetPiece = PieceOn(targetPos);
                                if (targetPiece == null || targetPiece.Color == Color
                                                        || targetPiece.Effects.All(e => e.EffectName == Stunned)) continue;
                                list.Add(new DwarfCuttlefishActive(this, targetPiece));
                            }
                            break;
                        case true:
                            foreach (var (rankOff, fileOff) in MoveEnumerators.Down(rank, file, 1))
                            {
                                var targetPos = IndexOf(rankOff, fileOff);
                                if (!IsActive(targetPos)) continue;
                                var targetPiece = PieceOn(targetPos);
                                if (targetPiece == null || targetPiece.Color == Color
                                                        || targetPiece.Effects.All(e => e.EffectName == Stunned)) continue;
                                list.Add(new DwarfCuttlefishActive(this, targetPiece));
                            }
                            break;
                    }
                }
                else
                {
                    //query for AI in here
                    if (excludeEmptyTile)
                    {
                        
                    }
                    else
                    {
                        
                    }
                }
            };
        }

        int IPieceWithSkill.TimeToCooldown
        {
            get => timeToCooldown;
            set => timeToCooldown = value;
        }

        public SkillsDelegate Skills { get; }
    }
}