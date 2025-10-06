using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Transform playerTrans;
    public Vector3 firstPosition;

    [Header("事件监听")]
    public SceneLoadEventSO loadEventSO;
    public GameSceneEventSO firstLoadScene;

    [Header("广播")]
    public VoidEventSO afterSceneLoadedEvent;
    public FadeEventSO fadeEvent;

    private GameSceneEventSO currentLoadScene;
    private GameSceneEventSO sceneToLoad;
    private Vector3 positionToGo;
    private bool fadeScreen;
    private bool isLoading;
    public float fadeDuration;
    private void Awake()
    {
        //Addressables.LoadSceneAsync(firstLoadScene.sceneReference, LoadSceneMode.Additive);
        //currentLoadScene = firstLoadScene;
        //currentLoadScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
    }

    private void Start()
    {
        NewGame();
    }

    private void OnEnable()
    {
        loadEventSO.LoadRequestScene += OnLoadRequestEvent;
    }

    private void OnDisable()
    {
        loadEventSO.LoadRequestScene -= OnLoadRequestEvent;
    }

    private void NewGame()
    {
        sceneToLoad = firstLoadScene;
        //OnLoadRequestEvent(sceneToLoad, firstPosition, true);
        loadEventSO.RaiseLoadRequestEvent(sceneToLoad, firstPosition, true);
    }

    private void OnLoadRequestEvent(GameSceneEventSO locationToGo, Vector3 posToGo, bool fadeScreen)
    {
        if(isLoading)
            return;
        isLoading = true;
        sceneToLoad = locationToGo;
        positionToGo = posToGo;
        this.fadeScreen = fadeScreen;
        //Debug.Log("Load Request Event Received");
        if(currentLoadScene != null)
            StartCoroutine(UnLoadPreviousScene());
        else
            LoadNewScene();
    }

    private IEnumerator UnLoadPreviousScene()
    {
        if(fadeScreen)
        {
            fadeEvent.FadeIn(fadeDuration);
        }
        yield return new WaitForSeconds(fadeDuration);
       
        yield return currentLoadScene.sceneReference.UnLoadScene();
        playerTrans.gameObject.SetActive(false);
        LoadNewScene();
    }

    private void LoadNewScene()
    {
      var loadOptions = sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
        loadOptions.Completed += OnLoadCompleted;

    }

    private void OnLoadCompleted(AsyncOperationHandle<SceneInstance> handle)
    {
        currentLoadScene = sceneToLoad;
        playerTrans.position = positionToGo;
        playerTrans.gameObject.SetActive(true);
        if (fadeScreen)
        {
            fadeEvent.FadeOut(fadeDuration);
        }
        isLoading = false;

        if(currentLoadScene.SceneType == SceneType.Location)
            afterSceneLoadedEvent.RaiseEvent();
    }
}
