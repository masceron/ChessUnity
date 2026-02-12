using System.Collections.Generic;
using Game.Action.Internal.Pending.Relic;
using Game.Action.Relics;
using Game.Common;
using Game.Managers;
using Game.Piece;
using Game.Piece.PieceLogic.Commons;
using Game.Relics.Commons;
using UX.UI.Ingame;
using ZLinq;

namespace Game.Relics
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SirensHarpoon : RelicLogic
    {
        public SirensHarpoon(RelicConfig config) : base(config)
        {
            type = config.Type;
            Color = config.Color;
            TimeCooldown = 2;
            CurrentCooldown = 0;
        }
        public override void Activate()
        {
            if (CurrentCooldown == 0)
            {
                foreach (var piece in MatchManager.Ins.GameState.PieceBoard)
                {
                    if (piece == null) continue;


                    if (piece.Color != Color && piece.PieceRank <= PieceRank.Common)
                    {
                        TileManager.Ins.MarkAsMoveable(piece.Pos);
                        var pending = new SirensHarpoonPending(this, piece.Pos);
                        BoardViewer.ListOf.Add(pending);
                    }
                    BoardViewer.Selecting = -2;
                    BoardViewer.SelectingFunction = 4;
                }
            }
        }

        public async override void ActiveForAI()
        {
            var listPieces = new List<PieceLogic>();

            for (var i = 0; i < BoardUtils.BoardSize; ++i)
            {
                var piece = BoardUtils.PieceOn(i);
                if (piece == null || piece.Color == Color) continue;
                if (piece.PieceRank > PieceRank.Common) continue;

                if (piece.Effects.Any(e => (
                        e.EffectName == "effect_extremophile" || e.EffectName == "effect_sanity"))
                   ) continue;

                listPieces.Add(piece);
            }

            if (listPieces.Count == 0) return;
            
            listPieces.Sort((a, b) => 
                b.GetValueForAI().CompareTo(a.GetValueForAI())
            );
            
            var topValue = listPieces[0].GetValueForAI();
            var topGroup = listPieces.Where(pieceLogic => pieceLogic.GetValueForAI() == topValue).ToList();
            var idx = UnityEngine.Random.Range(0, topGroup.Count);
            
            var excute = new SirenHarpoonExcute(CommanderPiece.Pos ,topGroup[idx].Pos);
            BoardViewer.Ins.ExecuteAction(excute);

            // var pending = new SirensHarpoonPending(this, topGroup[idx].Pos);
            // if (pending is PendingAction p)
            // {
            //     BoardViewer.Ins.ExecuteAction(await p.WaitForCompletion());
            // }
        }
    }
}