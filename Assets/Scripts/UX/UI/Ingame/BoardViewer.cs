using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Data.UI.UIObject3D.Scripts;
using DG.Tweening;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal.Pending;
using Game.Action.Quiets;
using Game.Action.Skills;
using Game.Data.Pieces;
using Game.Managers;
using Game.Piece.PieceLogic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Color = UnityEngine.Color;
using static Game.Common.BoardUtils;

namespace UX.UI.Ingame
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class BoardViewer : MonoBehaviour
    {
        [Header("Function Groups")]
        [SerializeField] private CanvasGroup pieceAction;
        [SerializeField] private CanvasGroup gameAction;
     
        [Header("Piece Actions")]
        [SerializeField] private Toggle move;
        [SerializeField] private Toggle capture;
        [SerializeField] private Toggle skill;
        [SerializeField] private TMP_Text skillCoolDown;
        
        [Header("Game Actions")]
        [SerializeField] private Button special;
        [SerializeField] private Button relic;
        [SerializeField] private Button skip;

        [Header("Piece Info Popup")]
        [SerializeField] private GameObject pieceInfo;
        [SerializeField] private UIObject3D pieceImage;
        [SerializeField] private GameObject pieceStatusEffect;
        [SerializeField] private RawImage pieceDemonstration;

        [NonSerialized] private int SelectingFunction;

        [Header("Captures")] 
        [SerializeField] private RectTransform allyCaptured;
        [SerializeField] private RectTransform enemyCaptured;
        
        private Transform mainCameraCenter;

        [Header("Miscellaneous UIs")]
        [SerializeField] private GameObject effectUI;
        [SerializeField] private GameObject capturedUI;
        [SerializeField] public GameObject chrysosShopUI;
        [SerializeField] public GameObject thalassosResurrector;
        
        private static ColorBlock _normalColors;
        private static ColorBlock _activeColors;

        private bool seeingCapture;
        private PieceLogic hovering;
        
        private void Start()
        {
            mainCameraCenter = GameObject.Find("CameraTarget").GetComponent<Transform>();
            _normalColors = move.colors;

            _activeColors = _normalColors;
            _activeColors.normalColor = Color.white;
            _activeColors.selectedColor = Color.white;
            _activeColors.highlightedColor = Color.white;
            _activeColors.pressedColor = Color.white;
            
            move.onValueChanged.AddListener(delegate
            {
                SetToggle(move);
            });
            
            capture.onValueChanged.AddListener(delegate
            {
                SetToggle(capture);
            });
            
            skill.onValueChanged.AddListener(delegate
            {
                SetToggle(skill);
            });
            
            DisablePieceInteractions();
            pieceInfo.SetActive(false);
            
            var gameState = MatchManager.Ins.GameState;
            gameState.WhiteCaptured.CollectionChanged += ReloadCaptureList;
            gameState.BlackCaptured.CollectionChanged += ReloadCaptureList;
            _moveList = new List<Game.Action.Action>();
        }

        private readonly List<GameObject> allyCapturedList = new();
        private readonly List<GameObject> enemyCapturedList = new();

        private void ReloadCaptureList(object o, NotifyCollectionChangedEventArgs e)
        {
            var color = OurSide();
            var collection = o == WhiteCaptured() ? 
                !color ? allyCapturedList : enemyCapturedList :
                color ? allyCapturedList : enemyCapturedList;

            var obj = collection == allyCapturedList ? allyCaptured : enemyCaptured;

            if (e.OldItems != null)
            {
                var removed = collection[e.OldStartingIndex];
                collection.Remove(removed);
                Destroy(removed);
            }
            else
            {
                var newObj = Instantiate(capturedUI, obj);
                newObj.GetComponent<CapturedUI>().Load((PieceConfig)e.NewItems[0]);
                collection.Add(newObj);
            }
        }

        private void OnEnable()
        {
            MatchManager.Ins.InputProcessor = this;
        }

        private void PanTo(int pos1, int pos2)
        {
            var direction = mainCameraCenter.position;
            direction.x = pos1;
            direction.z = pos2;

            mainCameraCenter.DOMove(direction, 0.3f);
        }

        private static void SetToggle(Toggle toggle)
        {
            toggle.colors = !toggle.isOn ? _normalColors : _activeColors;
        }

        private static void Disable(Toggle toggle)
        {
            toggle.colors = _normalColors;
            toggle.isOn = false;
        }

        private void DisablePieceInteractions()
        {
            pieceAction.interactable = false;
            Disable(move);
            Disable(capture);
            Disable(skill);
            skillCoolDown.text = "";
            
            SelectingFunction = 0;
        }

        private void EnablePieceInteractions()
        {
            pieceAction.interactable = true;
            skill.interactable = PieceOn(Selecting).SkillCooldown == 0;
            if (skill.interactable)
            {
                LoadSkillInfo();
            }
        }

        private void LoadSkillInfo()
        {
            var info = AssetManager.Ins.PieceData[PieceOn(Selecting).Type];
            
        }

        private void LoadPieceActionInfo()
        {
            var cooldown = PieceOn(Selecting).SkillCooldown;
            if (cooldown != 0)
            {
                skillCoolDown.text = cooldown > 0 ? cooldown.ToString() : "";
            }
            else
            {
                skillCoolDown.text = "";
            }
        }

        private void DisableGameInteractions()
        {
            gameAction.interactable = false;
        }

        private void EnableGameInteractions()
        {
            gameAction.interactable = true;
        }

        public void PressMove(InputAction.CallbackContext context)
        {
            if (!context.performed || !pieceAction.interactable) return;
            move.isOn = !move.isOn;
        }

        public void PressCapture(InputAction.CallbackContext context)
        {
            if (!context.performed || !pieceAction.interactable) return;
            capture.isOn = !capture.isOn;
        }

        public void PressSkill(InputAction.CallbackContext context)
        {
            if (!context.performed || !pieceAction.interactable) return;
            skill.isOn = !skill.isOn;
        }

        public void ClickMove()
        {
            if (SelectingFunction == 1)
            {
                TileManager.Ins.UnmarkAll();
                SelectingFunction = 0;
                return;
            }
            
            SelectingFunction = 1;
            if (MarkMove()) return;
            SelectingFunction = 0;
            move.isOn = false;
        }

        public void ClickCapture()
        {
            if (SelectingFunction == 2)
            {
                TileManager.Ins.UnmarkAll();
                SelectingFunction = 0;
                return;
            }
            
            SelectingFunction = 2;
            if (MarkCapture()) return;
            SelectingFunction = 0;
            capture.isOn = false;
        }

        public void ClickSkill()
        {
            if (SelectingFunction == 3)
            {
                TileManager.Ins.UnmarkAll();
                SelectingFunction = 0;
                return;
            }
            
            SelectingFunction = 3;
            if (MarkSkill()) return;
            SelectingFunction = 0;
            skill.isOn = false;
        }

        public void PressEndTurn(InputAction.CallbackContext context)
        {
            if (!context.performed || !skip.interactable) return;
            EndTurn();
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
                hovering = null;
                pieceStatusEffect.SetActive(false);
                pieceInfo.SetActive(false);
                return;
            }
            
            var piece = PieceOn(pos);

            if (piece == null) return;

            hovering = piece;
            SetUpPieceInfo(piece);
            SetUpStatusEffects(piece);
        }

        private void SetUpStatusEffects(PieceLogic piece)
        {
            pieceStatusEffect.SetActive(true);
            var already = pieceStatusEffect.transform.childCount;
            var needed = piece.Effects.Count;
            if (already < needed)
            {
                for (var i = 1; i <= needed - already; i++)
                {
                    Instantiate(effectUI, pieceStatusEffect.transform, true);
                }
            }
            else if (already > needed)
            {
                var index = already - 1;
                for (var i = 1; i <= already - needed; i++)
                {
                    Destroy(pieceStatusEffect.transform.GetChild(index).gameObject);
                    index--;
                }
            }

            for (var i = 0; i < needed; i++)
            {
                pieceStatusEffect.transform.GetChild(i).GetComponent<EffectUI>().Set(piece.Effects[i]);
            }
        }
        
        private void SetUpPieceInfo(PieceLogic piece)
        {
            pieceInfo.SetActive(true);
            var pieceInformation = AssetManager.Ins.PieceData[piece.Type];
            LoadPieceModel(pieceInformation);
            LoadPieceDemonstrations(pieceInformation);
        }
        
        private void LoadPieceModel(PieceObject info)
        {
            pieceImage.ObjectPrefab = info.prefab.transform;
        }
        
        private void LoadPieceDemonstrations(PieceObject info)
        {
            if (!seeingCapture)
            {
                if (info.movePattern)
                {
                    pieceDemonstration.texture = info.movePattern;
                    pieceDemonstration.color = Color.white;
                    return;
                }
            }
            else
            {
                if (info.capturePattern)
                {
                    pieceDemonstration.texture = info.capturePattern;
                    pieceDemonstration.color = Color.white;
                    return;
                }
            }
            
            pieceDemonstration.texture = null;
            pieceDemonstration.color = new Color(0, 0, 0, 0);
        }

        public void ToggleDemonstrations(InputAction.CallbackContext context)
        {
            if (!context.performed || !pieceInfo.activeSelf) return;
            
            seeingCapture = !seeingCapture;
            LoadPieceDemonstrations(AssetManager.Ins.PieceData[hovering.Type]);
        }

        public void QuitMenu(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            UIManager.Ins.Load(CanvasID.QuitToMainMenu);
        }

        private static int Selecting = -1;
        private static List<Game.Action.Action> _moveList;
        private static readonly List<Game.Action.Action> _listOf = new();
        
        private static bool MarkMove()
        {
            TileManager.Ins.UnmarkAll();
            _listOf.Clear();
            
            foreach (var _move in _moveList.OfType<IQuiets>())
            {
                _listOf.Add((Game.Action.Action)_move);
                TileManager.Ins.MarkAsMoveable(((Game.Action.Action)_move).Target);
            }
            
            return _listOf.Count > 0;
        }

        private bool MarkCapture()
        {
            TileManager.Ins.UnmarkAll();
            _listOf.Clear();
            
            foreach (var _move in _moveList.OfType<ICaptures>())
            {
                _listOf.Add((Game.Action.Action)_move);
                TileManager.Ins.MarkAsMoveable(((Game.Action.Action)_move).Target);
            }

            return _listOf.Count > 0;
        }

        private bool MarkSkill()
        {
            TileManager.Ins.UnmarkAll();
            _listOf.Clear();
            
            foreach (var _move in _moveList.OfType<ISkills>())
            {
                _listOf.Add((Game.Action.Action)_move);
                TileManager.Ins.MarkAsMoveable(((Game.Action.Action)_move).Target);
            }
            
            return _listOf.Count > 0;
        }


        public void Unmark()
        {
            Selecting = -1;
            TileManager.Ins.UnmarkAll();
            DisablePieceInteractions();
            _listOf.Clear();
            SetPieceHover(-1);
        }

        public void ExecuteAction(Game.Action.Action action)
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
                
                var action = _listOf.Find(a => a.Target == pos);
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
            else
            {
                var piece = PieceOn(pos);
                if (piece == null) return;
                
                SetPieceHover(pos);
                TileManager.Ins.Select(pos);
                Selecting = pos;
                LoadPieceActionInfo();
                PanTo(RankOf(pos), FileOf(pos));

                if (piece.Color != MatchManager.Ins.GameState.SideToMove) return;
                
                EnablePieceInteractions();
                _moveList.Clear();
                PieceOn(Selecting).MoveList(_moveList);
            }
        }

        private void NewTurn()
        {
            ActionManager.EnqueueAction(new EndTurn());
            
            if (SideToMove() != OurSide())
            {
                DisableGameInteractions();
            }
            else
            {
                EnableGameInteractions();
            }
        }

        public void Hover(int pos)
        {
            SetPieceHover(pos);
        }
    }
}
