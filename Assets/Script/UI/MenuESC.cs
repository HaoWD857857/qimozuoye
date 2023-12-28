using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//这个脚本挂载在相机上，弹出菜单

public class MenuESC : MonoBehaviour
{
    public GameObject menuList;
    private bool menuKeys = true;
    [SerializeField] private AudioSource bgmSound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (menuKeys)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                menuList.SetActive(true);
                menuKeys = false;
                Time.timeScale = 0; //时间停止
                bgmSound.Pause();
            }
        }else if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuList.SetActive(false);
            menuKeys = true;
            Time.timeScale = 1;
            bgmSound.Play();
        }
        
    }

    public void Return() //返回游戏
    {
        menuList.SetActive(false);
        menuKeys = true;
        Time.timeScale = 1;
        bgmSound.Play();
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void Exit()
    {
        Application.Quit();
        Debug.Log("退出游戏");
    }
}
