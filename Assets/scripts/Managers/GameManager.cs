using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class GameManager : Singleton<GameManager>
{
    public UnityEvent<int> onLifeValueChanged;
    public GameObject Player; 
    public GameObject[] spawnLocations;
    [HideInInspector]
    public CharacterController playerInstance = null;
    [HideInInspector] public Transform currentSpawnPoint;
    public PlayerInput playerInput;
    Playercontroller Playercontroller;
    public int maxHealth = 20;
    private int _health = 5;
    bool gameOverLoaded = false;
    protected override void Awake()
    {

        base.Awake();
     
    }

    public int health
    {
        get { return _health; }
        set
        {
            if (_health > value)
                Respawn();

            _health = value;

            if (_health > maxHealth)
                _health = maxHealth;

            //if (_lives < 0)
            //gameover


            onLifeValueChanged?.Invoke(_health);

            Debug.Log("Health have been set to: " + _health.ToString());
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        SpawnPlayer();

        health = maxHealth;
        if (!Playercontroller) return;  
    }



    private void SpawnPlayer()
    {
        int spawn = Random.Range(0, spawnLocations.Length);
       
        //GameObject.Instantiate(Player, spawnLocations[spawn].transform.position, Quaternion.identity);
        
    }

    void Respawn()
    {
        if (playerInstance)
            playerInstance.transform.position = currentSpawnPoint.position;
    }
    // Update is called once per frame
    void Update()
    {
        if (_health <= 0 && !gameOverLoaded)
        {

            SceneManager.LoadScene(2);
            gameOverLoaded = true;
        }
    }

  // public Ray MousePos()
  // {
  //     Vector3 screenSpacePos = PAactions.player.Look.ReadValue<Vector2>();
  //
  //     return Camera.main.ScreenPointToRay(screenSpacePos);
  // }

  public void UpdateCheckpoint(Transform spawnPoint)
    {
        currentSpawnPoint = spawnPoint;
    }
}

/*public void Move(InputAction.CallbackContext ctx)
    {
        if (ctx.canceled)
        {
            curMoveInput = Vector3.zero;
            moveDir = Vector3.zero;
            return;
        }

        Vector2 move = ctx.action.ReadValue<Vector2>();
        

        moveDir = new Vector3(move.x, 0, move.y).normalized;
        curMoveInput = moveDir * Playerspeed;
    }
    public void Punch(InputAction.CallbackContext ctx)
    {
        
    }
    public void Kick(InputAction.CallbackContext ctx)
    {

    }
    public void Fire(InputAction.CallbackContext ctx)
    {

    }*/
