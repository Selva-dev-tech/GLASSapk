using UnityEngine;
using System.IO;

namespace WorkflowSystem
{
    public class WorkflowManager : MonoBehaviour
    {
        public static WorkflowManager Instance { get; private set; }
        
        private Workflow currentWorkflow;
        private int currentStepIndex = 0;

        public Workflow CurrentWorkflow => currentWorkflow;
        public int CurrentStepIndex => currentStepIndex;

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

        void Start()
        {
            LoadJsonData();
        }

        void LoadJsonData()
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, "workflow.json");
            string json = File.ReadAllText(filePath);
            currentWorkflow = JsonUtility.FromJson<Workflow>(json);
            UIManager.Instance.DisplayStep(currentWorkflow.stepData[currentStepIndex]);

        }

        public void NavigateToStep(string nextStepId)
        {
            if (nextStepId == "end")
            {
                QuitWorkflow();
                return;
            }

            for (int i = 0; i < currentWorkflow.stepData.Length; i++)
            {
                if (currentWorkflow.stepData[i].stepId == nextStepId)
                {
                    currentStepIndex = i;
                    UIManager.Instance.DisplayStep(currentWorkflow.stepData[currentStepIndex]);
                    break;
                }
            }
        }

        private void QuitWorkflow()
        {
            Debug.Log("Workflow Ended");
            // Application.Quit(); // Uncomment to quit the application when workflow ends
        }
    }

    [System.Serializable]
    public class Workflow
    {
        public string workflowId;
        public string workflowName;
        public string createdBy;
        public string created;
        public Toggles toggles;
        public StepData[] stepData;
    }

    [System.Serializable]
    public class Toggles
    {
        public bool callSupport;
        public bool documentation;
        public bool pauseTask;
        public bool cancelTask;
    }

    [System.Serializable]
    public class StepData
    {
        public string stepId;
        public string stepName;
        public int uiTemplate;
        public string imageUrl;
        public int imageScaleType;
        public string title;
        public string text;
        public ButtonData[] buttons;
    }

    [System.Serializable]
    public class ButtonData
    {
        public int id;
        public string text;
        public string nextStepId;
    }
}