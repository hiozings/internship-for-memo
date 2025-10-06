using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/SceneLoadEventSO")]
public class SceneLoadEventSO : ScriptableObject
{
    public UnityAction<GameSceneEventSO, Vector3, bool> LoadRequestScene;

    public void RaiseLoadRequestEvent(GameSceneEventSO locationToLoad, Vector3 posToGo, bool fadeScreen)
    {
        LoadRequestScene?.Invoke(locationToLoad, posToGo, fadeScreen);
    }
}
