using UnityEngine;

namespace WorkflowSystem
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        public TitleText titleTextComponent;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void DisplayStep(StepData stepData)
        {
            // Update title and text
            titleTextComponent.UpdateTitleAndText(stepData.title, stepData.text);

            // Update buttons
            titleTextComponent.UpdateButtons(stepData.buttons);
        }
    }
}