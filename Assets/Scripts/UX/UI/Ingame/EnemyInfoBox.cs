using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Relics.Commons;
using UnityEngine;
using UnityEngine.UIElements;
using UX.UI.Common;
using UX.UI.Common.Tooltip;

namespace UX.UI.Ingame
{
    public class EnemyInfoBox : MonoBehaviour
    {
        private BoardViewer _boardViewer;
        private VisualElement _enemyIcon;
        private VisualElement _enemySkill;
        private VisualElement _enemyRelic;
        private RadialProgress _enemySkillRadial;
        private RadialProgress _enemyRelicRadial;
        private Label _enemySkillCooldownTime;
        private Label _enemyRelicCooldownTime;
        private VisualElement _fieldEffect;
        private RelicLogic _relicLogic;

        private TooltipManipulator _skillTooltipManipulator;

        private void Awake()
        {
            _boardViewer = gameObject.GetComponent<BoardViewer>();
            _skillTooltipManipulator = null;

            var root = GetComponent<UIDocument>().rootVisualElement;

            _enemyIcon = root.Q<VisualElement>("EnemyPieceIcon");
            _enemySkillRadial = root.Q<RadialProgress>("EnemySkillRadial");
            _enemyRelicRadial = root.Q<RadialProgress>("EnemyRelicRadial");
            _fieldEffect = root.Q<VisualElement>("FieldEffect");
            _enemySkillCooldownTime = root.Q<Label>("EnemySkillCooldownTime");
            _enemyRelicCooldownTime = root.Q<Label>("EnemyRelicCooldownTime");
            _enemySkill = root.Q<VisualElement>("EnemySkill");
            _enemyRelic = root.Q<VisualElement>("EnemyRelic");

            MatchManager.Ins.OnInitComplete += state =>
            {
                SetupRelic(state.Relics);
                state.PropertyChanged += (_, args) =>
                {
                    if (args.PropertyName == nameof(GameState.Relics))
                        HandleRelicChanges();
                };
            };
        }

        private void OnEnable()
        {
            _boardViewer.OnSelectingPieceChanged += SelectingPieceChangeHandler;
        }

        private void OnDisable()
        {
            _boardViewer.OnSelectingPieceChanged -= SelectingPieceChangeHandler;
        }

        private void SelectingPieceChangeHandler(PieceLogic pieceLogic)
        {
            _enemySkill.RemoveTooltip(_skillTooltipManipulator);
            _skillTooltipManipulator = null;
            _enemySkill.SetEnabled(false);

            if (pieceLogic == null || pieceLogic.Color == BoardUtils.OurSide())
            {
                _enemySkillRadial.Progress = 1f;
                _enemySkillCooldownTime.text = "";
                _enemyIcon.style.backgroundImage = null;
                return;
            }

            var data = AssetManager.Ins.PieceData[pieceLogic.Type];
            _enemyIcon.style.backgroundImage = data.icon;

            if (pieceLogic is not IPieceWithSkill pieceWithSkill) return;

            _skillTooltipManipulator = _enemySkill.AddTooltip(
                Localizer.GetText("piece_skill", data.key + "_skill", null),
                "",
                Localizer.GetText("piece_skill_description", data.key + "_skill_description", null));

            if (pieceLogic.SkillCooldown == 0)
            {
                _enemySkill.SetEnabled(true);
                return;
            }

            var timeToCooldown = pieceWithSkill.TimeToCooldown;
            _enemySkillRadial.Progress =
                (float)(timeToCooldown - pieceLogic.SkillCooldown) / timeToCooldown;
            _enemySkillCooldownTime.text = pieceLogic.SkillCooldown.ToString();
        }

        private void SetupRelic((RelicLogic, RelicLogic) relics)
        {
            _relicLogic = !BoardUtils.OurSide() ? relics.Item2 : relics.Item1;
            var data = AssetManager.Ins.RelicData[_relicLogic.Type];
            _enemyRelic.AddTooltip(Localizer.GetText("relic_name", data.key, null), "",
                Localizer.GetText("relic_description", $"{data.key}_description", null));
            HandleRelicChanges();
        }

        private void HandleRelicChanges()
        {
            _enemyRelicRadial.Progress = 1f;
            _enemyRelicCooldownTime.text = "";
            _enemyRelic.SetEnabled(false);

            switch (_relicLogic.CurrentCooldown)
            {
                case 0:
                    _enemyRelic.SetEnabled(true);
                    return;
                case > 0:
                    _enemyRelicRadial.Progress = (float)(_relicLogic.TimeCooldown - _relicLogic.CurrentCooldown) /
                                                 _relicLogic.TimeCooldown;
                    _enemyRelicCooldownTime.text = _relicLogic.CurrentCooldown.ToString();
                    break;
            }
        }
    }
}