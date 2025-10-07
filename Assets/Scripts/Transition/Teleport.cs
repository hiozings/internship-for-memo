using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [Header("事件广播")]
    //public SceneLoadEventSO loadEventSO;
    public SceneLoadEventSO levelClearEventSO;

    public GameSceneEventSO sceneToGo;
    public Vector3 positionToGo;
    public int enemiesLeft;

    private void FixedUpdate()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            //TeleportToScene();
            //Debug.Log("No Enemies Left, Teleporting...");
            levelClearEventSO.RaiseLoadRequestEvent(sceneToGo, positionToGo, true);
        }
        //if (GameObject.FindGameObjectsWithTag("Enemy").Length != 0)
        //{
        //    Debug.Log("Enemies");
        //}
    }

    //public void OnEnemyCount()
    //{
    //    enemiesLeft--;
    //    if(enemiesLeft <= 0)
    //    {
    //        TeleportToScene();
    //        Debug.Log("No Enemies Left, Teleporting...");
    //    }
    //}

    //public void TeleportToScene()
    //{
    //    loadEventSO.RaiseLoadRequestEvent(sceneToGo, positionToGo, true);
    //}
}
