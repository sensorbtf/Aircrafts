[System.Serializable]
public class QuestData
{
    public QuestState state;
    public int questStepIndex;
    public QuestStepState[] questStepStates;

    public QuestData(QuestState p_state, int p_questStepIndex, QuestStepState[] p_questStepStates)
    {
        state = p_state;
        questStepIndex = p_questStepIndex;
        questStepStates = p_questStepStates;
    }
}
