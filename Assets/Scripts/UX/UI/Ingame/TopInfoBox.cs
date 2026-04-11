using System.Collections.Generic;
using System.Collections.Specialized;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using ObservableCollections;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;
using ZLinq;

namespace UX.UI.Ingame
{
    public class TopInfoBox : MonoBehaviour
    {
        private Label _allyCount;
        private Label _enemyCount;
        private ObservableDictionary<int, Entity> _entities;

        private void Awake()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;

            root.Q<Label>("RoundCounter").SetBinding("text", new DataBinding
            {
                dataSourcePath = new PropertyPath("CurrentTurn"),
                bindingMode = BindingMode.ToTarget
            });

            var binding = new DataBinding
            {
                dataSourcePath = new PropertyPath("IsDay"),
                bindingMode = BindingMode.ToTarget
            };
            var converterGroup = new ConverterGroup("DayNightConverter");
            converterGroup.AddConverter((ref bool isDay) => isDay ? "Day" : "Night");
            binding.ApplyConverterGroupToUI(converterGroup);
            root.Q<Label>("DayNight").SetBinding("text", binding);

            _allyCount = root.Q<Label>("AllyCount");
            _enemyCount = root.Q<Label>("EnemyCount");
            MatchManager.Ins.OnInitComplete += state =>
            {
                _entities = state.EntityDict;
                Setup();
                _entities.CollectionChanged += UpdateCounter;
            };
        }

        private void Setup()
        {
            var ourSide = BoardUtils.OurSide();
            _allyCount.text = _entities.Count(v => v.Value.Color == ourSide).ToString();
            _enemyCount.text = _entities.Count(v => v.Value.Color == !ourSide).ToString();
        }

        private void UpdateCounter(in NotifyCollectionChangedEventArgs<KeyValuePair<int, Entity>> args)
        {
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    var added = args.NewItem.Value;
                    if (added is PieceLogic pieceLogicAdded)
                    {
                        var ourSide = BoardUtils.OurSide();
                        if (pieceLogicAdded.Color == ourSide)
                        {
                            _allyCount.text = _entities.Count(v => v.Value.Color == ourSide).ToString();
                        }
                        else
                        {
                            _enemyCount.text = _entities.Count(v => v.Value.Color == !ourSide).ToString();
                        }
                    }

                    break;
                case NotifyCollectionChangedAction.Remove:
                    var removed = args.OldItem.Value;
                    if (removed is PieceLogic pieceLogicRemoved)
                    {
                        var ourSide = BoardUtils.OurSide();
                        if (pieceLogicRemoved.Color == ourSide)
                        {
                            _allyCount.text = _entities.Count(v => v.Value.Color == ourSide).ToString();
                        }
                        else
                        {
                            _enemyCount.text = _entities.Count(v => v.Value.Color == !ourSide).ToString();
                        }
                    }

                    break;
                case NotifyCollectionChangedAction.Move:
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Reset:
                default:
                    break;
            }
        }
    }
}