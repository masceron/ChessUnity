using Game.Action.Internal;
using Game.AI;
using Game.Effects.Debuffs;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using UX.UI.Ingame;
using System.Linq;
using UnityEngine;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class StingrayDash: Action, ISkills, IAIAction
    {
        public StingrayDash(int maker, int to) : base(maker, true)
        {
            Target = (ushort)to;
        }
        
        protected override void Animate()
        {
            PieceManager.Ins.Move(Maker, Target);
        }

        protected override void ModifyGameState()
        {
            var (rankFrom, fileFrom) = RankFileOf(Maker);
            var (rankTo, fileTo) = RankFileOf(Target);
            var board = PieceBoard();
            var caller = board[Maker];

            var rankDir = rankTo == rankFrom ? 0 : rankTo > rankFrom ? 1 : -1;
            var fileDir = fileTo == fileFrom ? 0 : fileTo > fileFrom ? 1 : -1;

            while (rankFrom != rankTo || fileFrom != fileTo)
            {
                rankFrom += rankDir;
                fileFrom += fileDir;

                var p = board[IndexOf(rankFrom, fileFrom)];
                if (p == null || p.Color == caller.Color) continue;
                
                ActionManager.EnqueueAction(new ApplyEffect(new Slow(1, 1, p)));
                ActionManager.EnqueueAction(new ApplyEffect(new Poison(1, p)));
            }
            
            MatchManager.Ins.GameState.Move(Maker, Target);
            Maker = Target;
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
        
        public void CompleteActionForAI()
        {
            var makerPiece = PieceOn(Maker);
            if (makerPiece == null) return;

            var (r0, f0) = RankFileOf(Maker);
            var enemies = new System.Collections.Generic.List<PieceLogic>();

            for (int dr = -1; dr <= 1; dr++)
            {
                for (int df = -1; df <= 1; df++)
                {
                    if (dr == 0 && df == 0) continue;
                    int rr = r0 + dr;
                    int ff = f0 + df;
                    if (!VerifyBounds(rr) || !VerifyBounds(ff)) continue;
                    int idx = IndexOf(rr, ff);
                    if (!IsActive(idx)) continue;
                    var p = PieceOn(idx);
                    if (p == null || p.Color == makerPiece.Color) continue;
                    // Exclude Extremophiles
                    if (p.Effects != null && p.Effects.Any(e => e.EffectName == "effect_extremophile")) continue;
                    enemies.Add(p);
                }
            }

            if (enemies.Count == 0) return;

            int bestScore = enemies.Max(p => p.GetValueForAI());
            var top = enemies.Where(p => p.GetValueForAI() == bestScore).ToList();
            var chosen = top.Count == 1 ? top[0] : top[Random.Range(0, top.Count)];

            Target = (ushort)chosen.Pos;
            BoardViewer.Ins.ExecuteAction(this);
        }
    
    }
}