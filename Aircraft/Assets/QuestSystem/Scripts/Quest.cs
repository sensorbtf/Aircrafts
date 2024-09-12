using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    // static info
    public QuestInfoSO Info;

    // state info
    public QuestState CurrentState;
    private int _currentQuestStepIndex;
    private QuestStepState[] _questStepStates;

    public Quest(QuestInfoSO questInfo)
    {
        Info = questInfo;
        CurrentState = QuestState.REQUIREMENTS_NOT_MET;
        _currentQuestStepIndex = 0;
        _questStepStates = new QuestStepState[Info.questStepPrefabs.Length];
        for (int i = 0; i < _questStepStates.Length; i++)
        {
            _questStepStates[i] = new QuestStepState();
        }
    }

    public Quest(QuestInfoSO p_questInfo, QuestState p_questState, int p_currentQuestStepIndex, QuestStepState[] p_questStepStates)
    {
        Info = p_questInfo;
        CurrentState = p_questState;
        _currentQuestStepIndex = p_currentQuestStepIndex;
        _questStepStates = p_questStepStates;

        // if the quest step states and prefabs are different lengths,
        // something has changed during development and the saved data is out of sync.
        if (_questStepStates.Length != Info.questStepPrefabs.Length)
        {
            Debug.LogWarning("Quest Step Prefabs and Quest Step States are "
                + "of different lengths. This indicates something changed "
                + "with the QuestInfo and the saved data is now out of sync. "
                + "Reset your data - as this might cause issues. QuestId: " + Info.id);
        }
    }

    public void MoveToNextStep()
    {
        _currentQuestStepIndex++;
    }

    public bool CurrentStepExists()
    {
        return (_currentQuestStepIndex < Info.questStepPrefabs.Length);
    }

    public void InstantiateCurrentQuestStep(Transform parentTransform)
    {
        GameObject questStepPrefab = GetCurrentQuestStepPrefab();
        if (questStepPrefab != null)
        {
            QuestStep questStep = Object.Instantiate<GameObject>(questStepPrefab, parentTransform)
                .GetComponent<QuestStep>();
            questStep.InitializeQuestStep(Info.id, _currentQuestStepIndex, _questStepStates[_currentQuestStepIndex].state);
        }
    }

    private GameObject GetCurrentQuestStepPrefab()
    {
        GameObject questStepPrefab = null;

        if (CurrentStepExists())
        {
            questStepPrefab = Info.questStepPrefabs[_currentQuestStepIndex];
        }
        else 
        {
            Debug.LogWarning("Tried to get quest step prefab, but stepIndex was out of range indicating that "
                + "there's no current step: QuestId=" + Info.id + ", stepIndex=" + _currentQuestStepIndex);
        }

        return questStepPrefab;
    }

    public void StoreQuestStepState(QuestStepState questStepState, int stepIndex)
    {
        if (stepIndex < _questStepStates.Length)
        {
            _questStepStates[stepIndex].state = questStepState.state;
            _questStepStates[stepIndex].status = questStepState.status;
        }
        else 
        {
            Debug.LogWarning("Tried to access quest step data, but stepIndex was out of range: "
                + "Quest Id = " + Info.id + ", Step Index = " + stepIndex);
        }
    }

    public QuestData GetQuestData()
    {
        return new QuestData(CurrentState, _currentQuestStepIndex, _questStepStates);
    }

    public string GetFullStatusText()
    {
        string fullStatus = "";

        if (CurrentState == QuestState.REQUIREMENTS_NOT_MET)
        {
            fullStatus = "Requirements are not yet met to start this quest.";
        }
        else if (CurrentState == QuestState.CAN_START)
        {
            fullStatus = "This quest can be started!";
        }
        else 
        {
            // display all previous quests with strikethroughs
            for (int i = 0; i < _currentQuestStepIndex; i++)
            {
                fullStatus += "<s>" + _questStepStates[i].status + "</s>\n";
            }
            // display the current step, if it exists
            if (CurrentStepExists())
            {
                fullStatus += _questStepStates[_currentQuestStepIndex].status;
            }
            // when the quest is completed or turned in
            if (CurrentState == QuestState.CAN_FINISH)
            {
                fullStatus += "The quest is ready to be turned in.";
            }
            else if (CurrentState == QuestState.FINISHED)
            {
                fullStatus += "The quest has been completed!";
            }
        }

        return fullStatus;
    }
}
