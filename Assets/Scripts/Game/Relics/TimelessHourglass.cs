using System.Collections.Generic;
using Game.Action.Internal.Pending.Relic;
using Game.Action.Relics;
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

        public override async void ActiveForAI()
        {
            // Gather all pieces
            var pieces = MatchManager.Ins.GameState.PieceBoard
                .Where(pieceLogic => pieceLogic != null)
                .ToList();

            if (pieces.Count == 0) return;

            var relicColor = Color;

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
            
            var bestScore = candidates.Max(pieceLogic => pieceLogic.GetValueForAI());
            var top = candidates.Where(pieceLogic => pieceLogic.GetValueForAI() == bestScore).ToList();
            var chosen = top.Count == 1 ? top[0] : top[UnityEngine.Random.Range(0, top.Count)];

            var excute = new TimelessHourglassExcute(CommanderPiece.Pos, Color ,chosen.Pos);
            BoardViewer.Ins.ExecuteAction(excute);

            // var pending = new TimelessHourglassPending(this, chosen.Pos);
            // if (pending is PendingAction p)
            // {
            //     BoardViewer.Ins.ExecuteAction(await p.WaitForCompletion());
            // }
        }
    
    }
}