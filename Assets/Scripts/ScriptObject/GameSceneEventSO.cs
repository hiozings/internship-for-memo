using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "Game Scene/GameSceneEventSO")]
public class GameSceneEventSO : ScriptableObject
{
    public SceneType SceneType;
    public AssetReference sceneReference;
}
