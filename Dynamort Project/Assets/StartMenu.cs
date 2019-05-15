using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private RectTransform menuContainer;
    [Header("Smooth")]
    [SerializeField] private bool smooth;
    [SerializeField] private float smoothSpeed = 0.1f;
    [SerializeField] private Vector3 desiredPosition;
    [Header("Logic")]
    private Vector3[] menuPositions;
    
    void Start() {
        menuPositions = new Vector3[menuContainer.childCount];
        Vector3 halfScreen = new Vector3(Screen.width/2, Screen.height/2,0); //Screen.width/2
        for (int i = 0; i < menuPositions.Length; i++){
            menuPositions[i] = menuContainer.GetChild(i).position - halfScreen;
            print("menuPositions " + i + ": " + menuPositions[i]);
        }
    }

    void Update(){
        if(smooth){
            menuContainer.anchoredPosition = Vector3.Lerp(menuContainer.anchoredPosition, desiredPosition, smoothSpeed);
        } else {
        menuContainer.anchoredPosition = desiredPosition;
        }
    }
    public void StartGame(){
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
    public void QuitGame(){
        Application.Quit();
    }
    public void moveMenu(int id){
        desiredPosition = -menuPositions[id];
        print("desiredPosition: " + desiredPosition);
    }
}
