using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class QuestPoint : MonoBehaviour
{
    [Header("Quest")]
    [SerializeField] private QuestInfoSO questInfoForPoint;

    [Header("Config")]
    [SerializeField] private bool startPoint = true;
    [SerializeField] private bool finishPoint = true;

    private bool playerIsNear = false;
    private string questId;
    private QuestState currentQuestState;

    private QuestIcon questIcon;

    private void Awake() 
    {
        questId = questInfoForPoint.id;
        questIcon = GetComponentInChildren<QuestIcon>();
    }

    private void OnEnable()
    {
        EventsManager.Instance.questEvents.OnQuestStateChange += QuestStateChange;
        EventsManager.Instance.inputEvents.OnSubmitPressed += SubmitPressed;
    }

    private void OnDisable()
    {
        EventsManager.Instance.questEvents.OnQuestStateChange -= QuestStateChange;
        EventsManager.Instance.inputEvents.OnSubmitPressed -= SubmitPressed;
    }

    private void SubmitPressed()
    {
        if (!playerIsNear)
        {
            return;
        }

        // start or finish a quest
        if (currentQuestState.Equals(QuestState.CAN_START) && startPoint)
        {
            EventsManager.Instance.questEvents.StartQuest(questId);
        }
        else if (currentQuestState.Equals(QuestState.CAN_FINISH) && finishPoint)
        {
            EventsManager.Instance.questEvents.FinishQuest(questId);
        }
    }

    private void QuestStateChange(Quest quest)
    {
        // only update the quest state if this point has the corresponding quest
        if (quest.Info.id.Equals(questId))
        {
            currentQuestState = quest.CurrentState;
            questIcon.SetState(currentQuestState, startPoint, finishPoint);
        }
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.CompareTag("Vehicle"))
        {
            playerIsNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D otherCollider)
    {
        if (otherCollider.CompareTag("Vehicle"))
        {
            playerIsNear = false;
        }
    }
}
