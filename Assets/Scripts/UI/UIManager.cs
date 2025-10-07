using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;
public class UIManager : MonoBehaviour
{

    private int levelScore;
    private int totalScore;

    [Header("事件")]
    public CharacterEventSO healthEvent;
    public CharacterEventSO buffEvent;
    public SceneLoadEventSO loadEvent;
    public SceneLoadEventSO levelClearEvent;
    public SceneLoadEventSO loadEventSO;
    public ScoreEventSO scoreEvent;

    private GameSceneEventSO sceneToGo;
    private Vector3 positionToGo;
    [Header("组件")]
    public PlayerStatBar playerStatBar;
    public Button pauseBtn;
    public GameObject pausePanel;
    public GameObject levelClearPanel;
    public GameObject gameOverPanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI levelScoreText;
    public TextMeshProUGUI totalScoreText;
    public TextMeshProUGUI finalScoreText;
    public Character character;

    private void Awake()
    {
        pauseBtn.onClick.AddListener(TogglePausePanel);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePausePanel();
        }
        if(levelClearPanel.activeInHierarchy && Input.GetKeyDown(KeyCode.Space))
        {
            ToggleLevelClearPanel();
        }

    }

    private void OnEnable()
    {
        healthEvent.OnEventRaised += OnHealthEvent;
        buffEvent.OnEventRaised += OnBuffEvent;
        loadEvent.LoadRequestScene += OnLoadEvent;
        levelClearEvent.LoadRequestScene += OnLevelClear;
        scoreEvent.OnEventRaised += OnScoreChange;
    }

    
    private void OnDisable()
    {
        healthEvent.OnEventRaised -= OnHealthEvent;
        buffEvent.OnEventRaised -= OnBuffEvent;
        loadEvent.LoadRequestScene -= OnLoadEvent;
        levelClearEvent.LoadRequestScene -= OnLevelClear;
        scoreEvent.OnEventRaised -= OnScoreChange;
    }

    
    public void TogglePausePanel()
    {
       if(pausePanel.activeInHierarchy)
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1.0f;
        }
       else
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void ToggleLevelClearPanel()
    {
        if (levelClearPanel.activeInHierarchy)
        {
            levelClearPanel.SetActive(false);
            Time.timeScale = 1.0f;
            loadEventSO.RaiseLoadRequestEvent(sceneToGo, positionToGo, true);
            //Debug.Log("Loading Next Scene");
            levelScore = 0;
            scoreText.text = "0";
            character.currentHealth = character.maxHealth;
            character.OnHealthChange?.Invoke(character);
            character.ResetBuff(BuffType.Nobuff);
        }
        else
        {
            levelClearPanel.SetActive(true);
            Time.timeScale = 0f;
        }
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
    private void OnLevelClear(GameSceneEventSO sceneToLoad, Vector3 posToGo, bool fadeScreen)
    {
       levelClearPanel.SetActive(true);
        Time.timeScale = 0f;
        sceneToGo = sceneToLoad;
        positionToGo = posToGo;
        //Debug.Log("Level Clear Panel Opened");
    }

    private void OnScoreChange(int score)
    {
        levelScore += score;
        totalScore += score;
        scoreText.text = $"{levelScore}";
        levelScoreText.text = "level score: "+ $"{levelScore}";
        totalScoreText.text = "total score: " + $"{totalScore}";
        finalScoreText.text = "final score: " + $"{totalScore}";
    }

}
