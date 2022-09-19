using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest 
{
    public enum QuestProgress
    {
        NOT_AVAILABLE,
        AVAILABLE,
        COMPLETE
    }
    public string title;
    public int id;
    public QuestProgress progress; // state of current quest
    public string questObjective;
    public int nextQuest;
    public int goldReward;
    public int gemReward;
    public string itemReward;

}
