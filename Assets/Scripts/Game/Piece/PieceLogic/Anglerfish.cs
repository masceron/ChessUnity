using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Common;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Anglerfish : Commons.PieceLogic, IPieceWithSkill
    {
        public Anglerfish(PieceConfig cfg) : base(cfg, KingMoves.Quiets, PawnPushMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Consume(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Extremophile(this)));

            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;
                if (isPlayer)
                {
                    // if (!Color)
                    // {
                        // foreach (var (rank, file) in MoveEnumerators.Up(RankOf(Pos), FileOf(Pos), 3))
                        // {
                        //     var idx = IndexOf(rank, file);
                        //     var pOn = PieceOn(idx);
                        //     if (pOn != null && pOn.Color != Color)
                        //     {
                        //         list.Add(new AnglerfishTaunt(Pos, idx));
                        //     }
                        // }
                        var targets = SkillRangeHelper.GetActiveEnemyPieceInDirectionUp(Pos, 3);
                        foreach (var target in targets)
                        {
                            list.Add(new AnglerfishTaunt(Pos, target));
                        }
                        
                    // }
                    // else
                    // {
                    //     foreach (var (rank, file) in MoveEnumerators.Down(RankOf(Pos), FileOf(Pos), 3))
                    //     {
                    //         var idx = IndexOf(rank, file);
                    //         var pOn = PieceOn(idx);
                    //         if (pOn != null && pOn.Color != Color)
                    //         {
                    //             list.Add(new AnglerfishTaunt(Pos, idx));
                    //         }
                    //     }
                    // }
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