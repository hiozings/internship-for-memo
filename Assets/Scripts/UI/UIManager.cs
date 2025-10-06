using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerStatBar playerStatBar;

    [Header("事件监听")]
    public CharacterEventSO healthEvent;
    public CharacterEventSO buffEvent;
    public SceneLoadEventSO loadEvent;

    private void OnEnable()
    {
        healthEvent.OnEventRaised += OnHealthEvent;
        buffEvent.OnEventRaised += OnBuffEvent;
        loadEvent.LoadRequestScene += OnLoadEvent;
    }

    private void OnDisable()
    {
        healthEvent.OnEventRaised -= OnHealthEvent;
        buffEvent.OnEventRaised -= OnBuffEvent;
        loadEvent.LoadRequestScene -= OnLoadEvent;
    }

    
    private void OnHealthEvent(Character character)
    {
        playerStatBar.OnHealthChange(character.currentHealth);
    }

    private void OnBuffEvent(Character character)
    {
        playerStatBar.OnBuffChange(character.buffType);
    }

    private void OnLoadEvent(GameSceneEventSO sceneToLoad, Vector3 arg1, bool arg2)
    {
       var isMenu = sceneToLoad.SceneType == SceneType.Menu;
        playerStatBar.gameObject.SetActive(!isMenu);
    }

}
