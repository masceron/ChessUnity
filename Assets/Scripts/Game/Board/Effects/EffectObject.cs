using Game.Board.General;
using UnityEngine;

namespace Game.Board.Effects
{
    [CreateAssetMenu(fileName = "EffectObject", menuName = "ScriptableObjects/EffectObject")]
    public class EffectObject : ScriptableObject
    {
        [SerializeField] public EffectType type;
        [SerializeField] public ObserverPriority effectCategory;
        [SerializeField] public ObserverType activeWhen;
    }
}
