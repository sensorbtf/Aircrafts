using System;
using Objects.Vehicles;
using Resources;
using UnityEngine;

namespace Objects
{
    public class PointOfInterest: IngameObject
    {
        [Header("PointOfInterest")]
        private QuestPoint _quest;

        public Action<PointOfInterest, bool> OnPoIClicked;

        internal void OnEnable()
        {
            Initialize();
            CanvasInfo.HealthBar.gameObject.SetActive(false);
            _quest = GetComponent<QuestPoint>();
        }

        public override void CheckState()
        {
            if (_quest != null && _quest.CurrentQuestState == QuestState.CAN_START)
                SetNewStateTexts(Actions.AcceptQuest);
            else
                ResetStateText(Actions.AcceptQuest);
        }

        public override void MakeAction(Actions p_actionType, IngameObject p_requester, IngameObject p_receiver)
        {
            switch (p_actionType)
            {
                case Actions.AcceptQuest:
                    if (p_requester is Vehicle collector)
                    {
                        _quest.SubmitPressed();
                    }
                    break;
            }
        }
    }
}