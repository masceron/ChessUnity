using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal.Pending;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic;
using PrimeTween;
using UnityEngine;
using UnityEngine.InputSystem;
using static Game.Common.BoardUtils;

namespace UX.UI.Ingame
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class BoardViewer : Singleton<BoardViewer>
    {
        [SerializeField] private PieceInfoMenu pieceInfoMenu;
        [SerializeField] private PieceActions pieceActions;
        [SerializeField] private GameActions gameActions;
        [SerializeField] private EffectBar effectBar;
        
        private Transform mainCameraCenter;
        public static PieceLogic Hovering;
        public static int HoveringPos;
        
        public static int Selecting = -1;
        public static int SelectingFunction;
        private static readonly List<Action> MoveList = new();
        public static readonly List<Action> ListOf = new();
        
        private void Start()
        {
            mainCameraCenter = GameObject.Find("CameraTarget").GetComponent<Transform>();
            pieceActions.Load(ListOf, MoveList);
            UpdateRelic();
        }

        private void OnEnable()
        {
            MatchManager.Ins.InputProcessor = this;
        }

        public void UpdateRelic()
        {
            gameActions.UpdateRelic();
        }

        private void PanTo(int pos1, int pos2)
        {
            var direction = mainCameraCenter.position;
            direction.x = pos1;
            direction.z = pos2;
            
            Tween.Position(mainCameraCenter, direction, 0.3f);
        }

        private void SetPieceHover(int pos)
        {
            HoveringPos = pos;

            if (Selecting != -1) return;
            
            if (pos == -1)
            {
                Hovering = null;
                effectBar.Disable();
                pieceInfoMenu.Disable();
                return;
            }
            
            var piece = PieceOn(pos);

            if (piece == null) return;

            Hovering = piece;
            
            pieceInfoMenu.SetUpPieceInfo(piece);
            effectBar.SetUpStatusEffects(piece);
        }

        public void QuitMenu(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            UIManager.Ins.Load(CanvasID.QuitToMainMenu);
        }

        public void Unmark()
        {
            Selecting = -1;
            TileManager.Ins.UnmarkAll();
            pieceActions.DisablePieceInteractions();
            foreach (var action in ListOf)
            {
                if (action is System.IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
            ListOf.Clear();
            SetPieceHover(-1);
        }

        public void ExecuteAction(Action action)
        {
            Unmark();
            if (ActionManager.DoManualAction(action))
            {
                EndTurn();
            }
        }

        public void MarkPiece(int pos)
        {
            HoveringPos = pos;

            if (Selecting != -1)
            {
                if (SelectingFunction == 0) return;
                
                if (SelectingFunction == 4)
                {
                    var action = ListOf.Find(a => a.Maker == pos);
                    switch (action)
                    {
                        case null:
                            return;
                        case IPendingAble pending:
                            pending.CompleteAction();
                            return;
                    }
                    ActionManager.DoManualAction(action);
                } 
                else
                {
                    var action = ListOf.Find(a => a.Target == pos);
                    switch (action)
                    {
                        case null:
                            return;
                        case IPendingAble pending:
                            pending.CompleteAction();
                            return;
                        // Cách xử lý 2 target cũ
                        /*case IDoubleSelectionSkill doubleSkill:
                        if (doubleSkill.IsBothSelected()) { break; }
                        ((IPieceWithDoubleSelectionSkill)PieceOn(action.Maker)).firstSelection = action.Target;
                        ShowMoveList(action.Maker);
                        pieceActions.MarkSkill();
                        return; */
                    }

                    ExecuteAction(action);
                }
            }
            else
            {
                var piece = PieceOn(pos);
                if (piece == null) return;
                
                SetPieceHover(pos);
                TileManager.Ins.Select(pos);
                Selecting = pos;
                pieceActions.LoadPieceActionInfo();
                PanTo(RankOf(pos), FileOf(pos));

                if (piece.Color != MatchManager.Ins.GameState.SideToMove) return;
                
                pieceActions.EnablePieceInteractions();
                MoveList.Clear();
                PieceOn(Selecting).MoveList(MoveList);
            }
        }

        private void EndTurn()
        {
            if (SideToMove() != OurSide())
            {
                gameActions.DisableGameInteractions();
            }
            else
            {
                gameActions.EnableGameInteractions();
            }
        }

        public void Hover(int pos)
        {
            SetPieceHover(pos);
        }

        public void MarkMove()
        {
            pieceActions.ClickMove();
        }
    }
}