using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Common;
using Game.Data.Pieces;
using Game.Effects.Traits;
using Game.Moves;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic.Elites
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Anglerfish: PieceLogic, IPieceWithSkill
    {
        public Anglerfish(PieceConfig cfg) : base(cfg, KingMoves.Quiets, PawnPushMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Consume(this)));
            
            Skills = list =>
            {
                if (SkillCooldown != 0) return;
                
                if (!Color)
                {
                    foreach (var (rank, file) in MoveEnumerators.Up(RankOf(Pos), FileOf(Pos), 3))
                    {
                        var idx = IndexOf(rank, file);
                        var pOn = PieceOn(idx);
                        if (pOn != null && pOn.Color != Color)
                        {
                            list.Add(new AnglerfishTaunt(Pos, idx));
                        }
                    }
                }
                else
                {
                    foreach (var (rank, file) in MoveEnumerators.Down(RankOf(Pos), FileOf(Pos), 3))
                    {
                        var idx = IndexOf(rank, file);
                        var pOn = PieceOn(idx);
                        if (pOn != null && pOn.Color != Color)
                        {
                            list.Add(new AnglerfishTaunt(Pos, idx));
                        }
                    }
                }
            };
        }

        protected override void MoveToMake(List<Action.Action> list)
        {
            Quiets(list, Pos);
            Captures(list, Pos);
            Skills(list);
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }

        public SkillsDelegate Skills { get; set; }
    }
}