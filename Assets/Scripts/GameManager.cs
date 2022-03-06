using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using ExitGames.Client.Photon;

public class GameManager : MonoBehaviourPun
{
    public static GameManager Instance = null;

    [SerializeField] float x;
    [SerializeField] float y;
    [SerializeField] GameObject player1Prefab;
    [SerializeField] GameObject player2Prefab;

    public TMP_Text win;
    public TMP_Text lose;
    public GameObject Panel;
    [SerializeField] GameObject Deadpoint;
    [SerializeField] List<GameObject> ListPlayer = new List<GameObject>();
    public float isEndGame = 0;
    
    //public Photon.Realtime.Player playerData;
    // public string userID1;
    //public string userID2;
    public GameObject player1;
    public GameObject player2;
    public Vector2 startPoint;
    public Vector2 endPoint;
    public Vector2 direction;
    public Vector2 force;
    public float distance;
    public Trajectory trajectory;
    Camera cam;
    [SerializeField] float pushForce = 4f;
    bool isDragging = false;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        //trajectory = GetComponent<Trajectory>();
        cam = Camera.main;
        spawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            OnDragStart();
        }
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            OnDragEnd();
        }
        if (isDragging)
        {
            OnDrag();
        }
    }
    private void spawnPlayer()
    {
        /*for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            var playerData = PhotonNetwork.PlayerList[i];
            var player1 = PhotonNetwork.Instantiate(player1Prefab.name, new Vector3(x, y, 0f), Quaternion.identity);
            var Player1 = player1.GetComponent<Player>();
            Player1.PlayerData = playerData;

        }*/

        if (PhotonNetwork.PlayerList.Length == 1)
        {

            player1 = PhotonNetwork.Instantiate(player1Prefab.name, new Vector3(x, y, 0f), Quaternion.identity);
            var Player1 = player1.GetComponent<Player>();
            Player1.desactivateRb();
            
            //Debug.Log("userID: "+playerData.UserId);

        }
        else if (PhotonNetwork.PlayerList.Length == 2)

        {

            player2 = PhotonNetwork.Instantiate(player2Prefab.name, new Vector3(-x, y, 0f), Quaternion.identity);
            var Player2 = player2.GetComponent<Player>();
            Player2.desactivateRb();
            //Player2.setAnimSpine(Player2.idleAnimation);




        }


        Debug.Log("spawn player");
    }
    
    public void dead(Transform compareGameObject)

    {
        if (compareGameObject == null)
        {
            if (gameObject.transform.position.y < Deadpoint.transform.position.y)
            {

            }
            else
            {

            }
        }
    }
    public void OnDragStart()
    {
        trajectory.show();
       
            if(player1!=null)
            {
                player1.GetComponent<Player>().desactivateRb();
                startPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            }
            
        
        
            if(player2!=null)
            {
                player2.GetComponent<Player>().desactivateRb();
                startPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            }
            
        

    }
    public void OnDrag()
    {
        endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        distance = Vector2.Distance(startPoint, endPoint);
        direction = (startPoint - endPoint).normalized;
        force = direction * distance * pushForce;
        Debug.DrawLine(startPoint, endPoint);

       
            if(player1!=null)
            {
                var Player1 = player1.GetComponent<Player>();
                trajectory.updateDots(Player1.Pos, force);
            }
            
        
       
            if(player2!=null)
            {
                var Player2 = player2.GetComponent<Player>();
                trajectory.updateDots(Player2.Pos, force);
            }
            
        
        
    }
    public void OnDragEnd()
    {
        
            if(player1!=null)
            {
                var Player1 = player1.GetComponent<Player>();
                Player1.activateRb();
                Player1.Push(force);
            }
           
        
        
            if(player2!=null)
            {
                var Player2 = player2.GetComponent<Player>();
                Player2.activateRb();
                Player2.Push(force);
            }
           
        
        trajectory.hide();
    }
}
