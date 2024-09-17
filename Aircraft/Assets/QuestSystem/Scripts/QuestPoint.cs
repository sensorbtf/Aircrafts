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

    private bool _playerIsNear = false;
    private string _questId;
    private QuestState _currentQuestState;

    private QuestIcon _questIcon;

    public QuestState CurrentQuestState => _currentQuestState;

    private void Awake() 
    {
        _questId = questInfoForPoint.id;
        _questIcon = GetComponentInChildren<QuestIcon>();
    }

    private void OnEnable()
    {
        EventsManager.Instance.QuestEvents.OnQuestStateChange += QuestStateChange;
        EventsManager.Instance.InputEvents.OnSubmitPressed += SubmitPressed;
    }

    private void OnDisable()
    {
        EventsManager.Instance.QuestEvents.OnQuestStateChange -= QuestStateChange;
        EventsManager.Instance.InputEvents.OnSubmitPressed -= SubmitPressed;
    }

    public void SubmitPressed()
    {
        //if (!playerIsNear)
        //{
        //    return;
        //}

        // start or finish a quest
        if (_currentQuestState.Equals(QuestState.CAN_START) && startPoint)
        {
            EventsManager.Instance.QuestEvents.StartQuest(_questId);
        }
        else if (_currentQuestState.Equals(QuestState.CAN_FINISH) && finishPoint)
        {
            EventsManager.Instance.QuestEvents.FinishQuest(_questId);
        }
    }

    private void QuestStateChange(Quest quest)
    {
        // only update the quest state if this point has the corresponding quest
        if (quest.Info.id.Equals(_questId))
        {
            _currentQuestState = quest.CurrentState;
            _questIcon.SetState(_currentQuestState, startPoint, finishPoint);
        }
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.CompareTag("Vehicle"))
        {
            _playerIsNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D otherCollider)
    {
        if (otherCollider.CompareTag("Vehicle"))
        {
            _playerIsNear = false;
        }
    }
}
