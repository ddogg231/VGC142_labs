using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(-1)]
public class GameManager : Singleton<GameManager>
{

    public UnityEvent<int> onLifeValueChanged;
    public GameObject player; 
    public GameObject[] spawnLocations;

    [HideInInspector]
    public CharacterController playerInstance = null;
    [HideInInspector] public Transform currentSpawnPoint;
    public TakeInPA playerActions;
    CharacterController controller;

    

    public int maxHealth = 20;
    private int _health = 5;
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


            //onLifeValueChanged?.Invoke(_health);

            Debug.Log("Lives have been set to: " + _health.ToString());
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        // controller = GameObject.FindGameObjectWithTag("Player").GetComponent<topdownplayercontroller>();
        health = maxHealth;
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

  public void UpdateCheckpoint(Transform spawnPoint)
    {
        currentSpawnPoint = spawnPoint;
    }
}


