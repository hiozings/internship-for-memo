using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    public SceneLoadEventSO loadEventSO;
    public GameSceneEventSO menuScene;
    public Vector3 menuPosition;
    public  UIManager uiManager;

    public void ExitGame()
    {
        Debug.Log("Exit Game");
        Application.Quit();
    }

    public void ReturnMenu()
    {
        loadEventSO.RaiseLoadRequestEvent(menuScene, menuPosition, true);
        if(uiManager.pausePanel.activeInHierarchy)
        {
            uiManager.pausePanel.SetActive(false);
            Time.timeScale = 1.0f;
        }
        if (uiManager.levelClearPanel.activeInHierarchy)
        {
            uiManager.levelClearPanel.SetActive(false);
            Time.timeScale = 1.0f;
        }
        if(uiManager.gameOverPanel.activeInHierarchy)
        {
            uiManager.gameOverPanel.SetActive(false);
            Time.timeScale = 1.0f;
        }
        //EventSystem.current.SetSelectedGameObject(null);
        //EventSystem.current.SetSelectedGameObject(GameObject.Find("NewGameButton"));
    }
}
