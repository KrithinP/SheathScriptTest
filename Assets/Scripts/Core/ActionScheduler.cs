using UnityEngine;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        IAction currentAction;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public void StartAction(IAction action)
        {
            if (currentAction == action) return;
            if(currentAction != null)
            {
                currentAction.Cancel();
                print("Canceling" + currentAction);
            }
            currentAction = action;
        }
        public void CancelCurrentAction()
        {
            Debug.LogWarning("In Cancel current action ");
            StartAction(null);
            Debug.LogWarning("current action: " + currentAction);
        }
    }
}
