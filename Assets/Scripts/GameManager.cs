using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : Singleton<GameManager>
{
    private GameState _currentState;
    public GameState CurrentState { 
        get 
        {
            return _currentState;
        } 
        set 
        {
            _currentState = value;
            SceneManager.LoadScene("GameScene"); //we change our game state and then reload the scene
        } 
    }
    private void Awake() //we want some code to happen as soon as the script loads
    {
        if(Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject); //this makes sure we don't destroy our game manager
        }
        Physics.autoSyncTransforms = true; //this is to make sure that colliders in our scene update immediately
        
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
public enum GameState
{
    Building,
    Attacking,
    Visiting
}
