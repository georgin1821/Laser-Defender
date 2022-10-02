using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Task List", fileName = "New Task List")]
public class TaskListO : ScriptableObject
{
    [SerializeField] List<string> tasks = new List<string>();

    public List<string> GetTasks()
    {
        return tasks;
    }

    public void Save(List<string> savedTasks)
    {
        tasks.Clear();
        tasks = savedTasks;
    }
}
