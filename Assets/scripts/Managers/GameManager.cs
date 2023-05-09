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
    public PAactions PAactions;

    PlayerAttack PlayerAttack;
    Playermovement Playermovement;
   // public p

    public int maxHealth = 20;
    private int _health = 5;

    

    protected override void Awake()
    {

        base.Awake();
        PAactions = new PAactions();
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
        Playermovement = GameObject.FindGameObjectWithTag("Player").GetComponent<Playermovement>();
       // PlayerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();

        health = maxHealth;
        

        if (!Playermovement) return;

       
    }

    private void OnEnable()
    {
        PAactions.Enable();
    }

    private void OnDisable()
    {
        PAactions.Disable();
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
       Vector3 screenSpacePos = PAactions.player.Look.ReadValue<Vector2>();
  
       return Camera.main.ScreenPointToRay(screenSpacePos);
   }

  public void UpdateCheckpoint(Transform spawnPoint)
    {
        currentSpawnPoint = spawnPoint;
    }
}


