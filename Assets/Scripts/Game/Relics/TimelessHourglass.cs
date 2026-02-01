using System.Collections.Generic;
using Game.Action.Internal.Pending;
using Game.Action.Internal.Pending.Relic;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Relics.Commons;
using UX.UI.Ingame;
using ZLinq;

namespace Game.Relics
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class TimelessHourglass : RelicLogic
    {
        public TimelessHourglass(RelicConfig config) : base(config)
        {
            type = config.Type;
            Color = config.Color;
            TimeCooldown = config.TimeCooldown;
            CurrentCooldown = 0;
        }
        public override void Activate()
        {
            if (CurrentCooldown == 0)
            {
                
                foreach (var piece in MatchManager.Ins.GameState.PieceBoard)
                {
                    if (piece == null) continue;

                    TileManager.Ins.MarkAsMoveable(piece.Pos);
                    var pending = new TimelessHourglassPending(this, piece.Pos);
                    BoardViewer.ListOf.Add(pending);
                }

                BoardViewer.Selecting = -2;
                BoardViewer.SelectingFunction = 4;
            }
        }

        public override void ActiveForAI()
        {
            // Gather all pieces
            var pieces = MatchManager.Ins.GameState.PieceBoard
                .Where(p => p != null)
                .ToList();

            if (pieces.Count == 0) return;

            bool relicColor = Color;

            // Candidates: allies with cooldown >= 2, enemies with cooldown == 1
            var candidates = new List<PieceLogic>();
            foreach (var piece in pieces)
            {
                if (piece.Color == relicColor)
                {
                    if (piece.SkillCooldown >= 2) candidates.Add(piece);
                }
                else
                {
                    if (piece.SkillCooldown == 1) candidates.Add(piece);
                }
            }

            if (candidates.Count == 0) return;
            
            var bestScore = candidates.Max((p) => p.GetValueForAI());
            var top = candidates.Where(p => p.GetValueForAI() == bestScore).ToList();
            var chosen = top.Count == 1 ? top[0] : top[UnityEngine.Random.Range(0, top.Count)];

            var pending = new TimelessHourglassPending(this, chosen.Pos);
            if (pending is PendingAction p)
            {
                p.CompleteAction();
            }
        }
    
    }
}