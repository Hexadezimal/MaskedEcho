using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUI : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject aboutMenu;
    public GameObject creditsMenu;

    // Start is called before the first frame update
    void Start()
    {
        //Setzt alle Seiten des Startmenüs auf die richtige Sichtbarkeit für den Start
        //startMenu.SetActive(true);
        //aboutMenu.SetActive(false);
        //creditsMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(string sceneName){
        SceneManager.LoadScene(sceneName);
    }

    public void OpenMenu(GameObject menu){
        menu.SetActive(true);
    }

    public void CloseMenu(GameObject menu){
        menu.SetActive(false);
    }

    public void Exit(){
        Application.Quit();
    }
}
