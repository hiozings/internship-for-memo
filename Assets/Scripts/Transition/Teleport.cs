using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public SceneLoadEventSO loadEventSO;
    public GameSceneEventSO sceneToGo;
    public Vector3 positionToGo;
    public int enemiesLeft;

    private void FixedUpdate()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            TeleportToScene();
            //Debug.Log("No Enemies Left, Teleporting...");
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

    public void TeleportToScene()
    {
        loadEventSO.RaiseLoadRequestEvent(sceneToGo, positionToGo, true);
    }
}
