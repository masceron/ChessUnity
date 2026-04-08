using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

namespace UX.UI.Toolkit.Ingame
{
    public class TopInfoBox: MonoBehaviour
    {
        private BoardViewer _boardViewer;
        private Button _surrender;
        private Button _callDraw;

        private void Awake()
        {
            _boardViewer = GetComponent<BoardViewer>();

            var root = GetComponent<UIDocument>().rootVisualElement;

            _surrender = root.Q<Button>("Surrender");
            _callDraw = root.Q<Button>("CallDraw");
            
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
        }
    }
}