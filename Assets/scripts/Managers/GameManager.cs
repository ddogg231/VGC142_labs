using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class GameManager : Singleton<GameManager>
{
    public GameObject player; 
    public GameObject[] spawnLocations;

    [HideInInspector]
    public CharacterController playerInstance = null;
    [HideInInspector] public Transform currentSpawnPoint;
    public TakeInPA playerActions;
    CharacterController controller;

    

    public int maxLives = 5;
    private int _lives = 3;
    protected override void Awake()
    {

        base.Awake();
        playerActions = new TakeInPA();
    }

    private void OnEnable()
    {
        playerActions.Enable();
    }

    private void OnDisable()
    {
        playerActions.Disable();
    }
    public int lives
    {
        get { return _lives; }
        set
        {
            if (_lives > value)
                Respawn();

            _lives = value;

            if (_lives > maxLives)
                _lives = maxLives;

            //if (_lives < 0)
            //gameover


            //onLifeValueChanged?.Invoke(_lives);

            Debug.Log("Lives have been set to: " + _lives.ToString());
        }
    }
    // Start is called before the first frame update
    void Start()
    {
       // controller = GameObject.FindGameObjectWithTag("Player").GetComponent<topdownplayercontroller>();
        lives = maxLives;
        SpawnPlayer();
    }

    

    private void SpawnPlayer()
    {
        int spawn = Random.Range(0, spawnLocations.Length);
       
        GameObject.Instantiate(player, spawnLocations[spawn].transform.position, Quaternion.identity);
        
    }

    void Respawn()
    {
        if (playerInstance)
            playerInstance.transform.position = currentSpawnPoint.position;
    }
    // Update is called once per frame
    void Update()
    {

    }

    public Ray MousePos()
    {
        Vector3 screenSpacePos = playerActions.Player.Look.ReadValue<Vector2>();

        return Camera.main.ScreenPointToRay(screenSpacePos);
    }

    /*public void UpdateCheckpoint(Transform spawnPoint)
    {
        currentSpawnPoint = spawnPoint;
    }*/
}
