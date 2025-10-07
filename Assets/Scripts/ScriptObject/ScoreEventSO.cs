using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/ScoreEventSO")]
public class ScoreEventSO : ScriptableObject
{
   public UnityAction<int> OnEventRaised;

    public void RaiseEvent(int score)
    {
        OnEventRaised?.Invoke(score);
    }
}
