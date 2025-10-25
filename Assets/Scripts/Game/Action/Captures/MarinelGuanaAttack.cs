using Game.Action.Internal;
using UnityEngine;
using UX.UI.Ingame;
using Game.Action.Internal.Pending;
using Game.Managers;
using static Game.Common.BoardUtils;
using Game.Tile;
using Game.Action.Captures;
using Game.Common;
using Game.Movesets;
using System.Collections.Generic;


namespace Game.Action.Captures
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MarinelGuanaAttack: Action, ICaptures, IPendingAble
    {
        private readonly System.Func<bool> getIsActiveSkill;
        private readonly System.Action<bool> setIsActiveSkill;
        public MarinelGuanaAttack(int maker, int to, System.Func<bool> getIsActiveSkill, System.Action<bool> setIsActiveSkill) : base(maker, true)
        {
            Maker = (ushort)maker;
            Target = (ushort)to;
            this.getIsActiveSkill = getIsActiveSkill;
            this.setIsActiveSkill = setIsActiveSkill;
        }

        protected override void ModifyGameState()
        {

        }

        public void CompleteAction()
        {
            var makerPiece = PieceOn(Maker);
            if (makerPiece == null) return;
            var color = makerPiece.Color;        
            if (getIsActiveSkill())
            {
                ActionManager.ExecuteImmediately(new NormalCapture(Maker, Target));
                TileManager.Ins.Select(Target);
                BoardViewer.ListOf.Clear();
                var (rank, file) = RankFileOf(Target);

                foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 2)) {
                    int pos = IndexOf(rankOff, fileOff);  
                    var piece = PieceOn(pos);
                    if (pos == Maker) continue;
                    if (piece != null && color != piece.Color)
                    {
                        TileManager.Ins.MarkAsMoveable(pos);
                        var killAction = new KillPieceAction(pos);
                        BoardViewer.ListOf.Add(killAction);
                    }
                }
                setIsActiveSkill(false);
            }
            else
            {
                ActionManager.ExecuteImmediately(new NormalCapture(Maker, Target));
                BoardViewer.ListOf.Clear();
                TileManager.Ins.Select(Target);
                MatchManager.Ins.InputProcessor.Unmark();
                ActionManager.ExecuteImmediately(new EndTurn());
               
            }

        }
    }
}