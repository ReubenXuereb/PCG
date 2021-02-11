using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (SceneManager.GetActiveScene().name == "Task3.2Level1" && collider.transform.CompareTag("Player"))
        {
            SceneManager.LoadScene("Task3.2Level2");
        }
        else if(SceneManager.GetActiveScene().name == "Task3.2Level2" && collider.transform.CompareTag("Player"))
        {
            SceneManager.LoadScene("Task3.2Level3");
        }
        else if (SceneManager.GetActiveScene().name == "Task3.2Level3" && collider.transform.CompareTag("Player"))
        {
            SceneManager.LoadScene("Game Over");
        }
    }

    public void restartGame()
    {
        if (SceneManager.GetActiveScene().name == "Game Over")
        {
            SceneManager.LoadScene("Task3.2Level1");
        }
    }
}
