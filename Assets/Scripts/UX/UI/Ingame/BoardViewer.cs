using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal.Pending;
using Game.Managers;
using Game.Piece.PieceLogic;
using Game.Relics;
using PrimeTween;
using UnityEngine;
using UnityEngine.InputSystem;
using static Game.Common.BoardUtils;

namespace UX.UI.Ingame
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class BoardViewer : MonoBehaviour
    {
        [SerializeField] private PieceInfoMenu pieceInfoMenu;
        [SerializeField] private PieceActions pieceActions;
        [SerializeField] private GameActions gameActions;
        [SerializeField] private EffectBar effectBar;
        
        private Transform mainCameraCenter;
        public static PieceLogic Hovering;
        
        public static int Selecting = -1;
        public static int SelectingFunction;
        private static readonly List<Action> MoveList = new();
        public static readonly List<Action> ListOf = new();
        
        private void Start()
        {
            mainCameraCenter = GameObject.Find("CameraTarget").GetComponent<Transform>();
            pieceActions.Load(ListOf, MoveList);
            gameActions.Load(EndTurn);
            UpdateRelic();
        }

        private void OnEnable()
        {
            MatchManager.Ins.InputProcessor = this;
        }

        public void LoadRelic(RelicConfig whiteRelic, RelicConfig blackRelic)
        {
            gameActions.LoadRelic(whiteRelic, blackRelic);
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
        
        public void EndTurn()
        {
            Unmark();
            NewTurn();
        }

        private void SetPieceHover(int pos)
        {
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
            ActionManager.EnqueueAction(action);
            Unmark();
            NewTurn();
        }

        public void MarkPiece(int pos)
        {
            if (Selecting != -1)
            {
                if (SelectingFunction == 0) return;
                
                if (SelectingFunction == 4)
                {
                    Hovering = PieceOn(pos);
                    var action = ListOf.Find(a => a.Maker == pos);
                    switch (action)
                    {
                        case null:
                            return;
                        case IPendingAble pending:
                            pending.CompleteAction();
                            return;
                    }
                    ActionManager.EnqueueAction(action);
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

        private void NewTurn()
        {
            ActionManager.EnqueueAction(new EndTurn());
            
            if (SideToMove() != OurSide())
            {
                gameActions.DisableGameInteractions();
            }
            else
            {
                gameActions.EnableGameInteractions();
                gameActions.PassTurn();
            }
        }

        public void Hover(int pos)
        {
            SetPieceHover(pos);
        }
    }
}
