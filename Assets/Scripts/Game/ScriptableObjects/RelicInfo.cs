using Game.Relics;
using UnityEngine;

namespace Game.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Relic", menuName = "ScriptableObjects/Relic")]
    public class RelicInfo: ScriptableObject
    {
        [SerializeField] public RelicType type;
        [SerializeField] public string key;
        [SerializeField] public Texture2D icon;
        [SerializeField] public sbyte cooldown;
    }
}