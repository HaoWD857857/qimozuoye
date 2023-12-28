using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public void StartMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

   public void QuitMenu()
    {
        Application.Quit();
        Debug.Log("ÍË³öÓÎÏ·");
    }
    
}
