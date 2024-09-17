using Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectJunkQuest : QuestStep
{
    [SerializeField] private int junkToCollect = 0;
    private int junkCollected = 0;

    private void OnEnable()
    {
        EventsManager.Instance.MiscEvents.OnResourceCollected += CollectJunk;
    }

    private void OnDisable()
    {
        EventsManager.Instance.MiscEvents.OnResourceCollected -= CollectJunk;
    }

    private void CollectJunk(ResourceInUnit p_res)
    {
        if (junkCollected < junkToCollect) { junkCollected++; }

        if (junkCollected >= junkToCollect)
        {
            FinishQuestStep();
        }
    }

    protected override void SetQuestStepState(string state)
    {
    }
}
