using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Spine.Unity;
using ExitGames.Client.Photon;

public class Player : MonoBehaviourPunCallbacks
{
    public Rigidbody2D rg;
    private byte SYNCING_POSITION_EVENT = 0;
    private byte ANIM_ATTACK_EVENT = 0;

    private float onGround = 0;
    private Transform compareGameobject;

    public PhotonView view;

    SkeletonAnimation skeletonAnimation;

    [SpineAnimation("Idle")]
    public string idleAnimation;

    [SpineAnimation]
    public string attackAnimation;

    [SpineSlot]
    public string eyesSlot;

    [SpineAttachment(currentSkinOnly: true, slotField: "eyesSlot")]
    public string eyesOpenAttachment;

    [SpineAttachment(currentSkinOnly: true, slotField: "eyesSlot")]
    public string blinkAttachment;

    [Range(0, 0.2f)]
    public float blinkDuration = 0.05f;

    public Photon.Realtime.Player PlayerData { get; set; }
    bool isDataSended = false;
    private bool isAttack1 = false;
    private bool isAttack2 = false;

    [HideInInspector] public Vector3 Pos { get { return transform.position; } }
    private void Awake()
    {      
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        skeletonAnimation.OnRebuild += Apply;
        setAnimSpine(idleAnimation);
    }
    // Start is called before the first frame update
    
    private void OnEnable()
    {
      
            PhotonNetwork.NetworkingClient.EventReceived += NetwokingClient_EventReceived;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetwokingClient_EventReceived;
    }
    private void NetwokingClient_EventReceived(EventData obj)
    {
        
        
        
        if (obj.Code == ANIM_ATTACK_EVENT)
        {
            
                object[] datas = (object[])obj.CustomData;


                
                
                int ViewID = (int)datas[2];
            
            
            
                if (view.ViewID==1001)
                {
                    isAttack1= (bool)datas[0];
                    if (isAttack1)
                    {
                        setAnimSpine(attackAnimation);
                        //isAttack = false;
                        PhotonNetwork.NetworkingClient.EventReceived -= NetwokingClient_EventReceived;
                        Debug.Log("viewid: " + ViewID);
                        Debug.Log("isattack: " + isAttack1);

                    }





                }
                else if(view.ViewID==2001)
            {
                isAttack2 = (bool)datas[1];
                if (isAttack2)
                {
                    setAnimSpine(attackAnimation);
                    //isAttack = false;
                    PhotonNetwork.NetworkingClient.EventReceived -= NetwokingClient_EventReceived;
                    Debug.Log("viewid: " + ViewID);
                    Debug.Log("isattack: " + isAttack2);

                }
            }    

            

           
        }
        else if(obj.Code == SYNCING_POSITION_EVENT)
        {
            object[] datas = (object[])obj.CustomData;
            var pos = (Vector2)datas[3];
            int ViewID = (int)datas[2];
            
                if (view.ViewID==ViewID)
                {



                    gameObject.transform.position = pos;
                    PhotonNetwork.NetworkingClient.EventReceived -= NetwokingClient_EventReceived;
                    

                }
            

            
        }

    }
    void Start()
    {

        
       // view = GetComponent<PhotonView>();
        //rg = gameObject.GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "player1" || collision.gameObject.tag == "player2")
        {
            compareGameobject = collision.gameObject.transform;
            compareAltitude(compareGameobject);
            skeletonAnimation.AnimationName = idleAnimation;
        }
        if (collision.gameObject.tag == "pillar")
        {
            Debug.Log("on ground");
            onGround = 1;
            //rg.AddForce(new Vector3(0, 1f, 0) * 40);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        onGround = 0;
    }
    

    
    // Update is called once per frame
    void Update()
    {
       
            object[] datas = new object[] {isAttack1,isAttack2,view.ViewID,gameObject.transform.position };
            PhotonNetwork.RaiseEvent(SYNCING_POSITION_EVENT, datas, Photon.Realtime.RaiseEventOptions.Default, SendOptions.SendUnreliable);
        
      
        //if ((PhotonNetwork.CountOfPlayers == 2 && gameObject.tag == "player2") || PhotonNetwork.CountOfPlayers == 1 && gameObject.tag == "player1")
        //{
        /* if (onGround == 1)
         {

             if (Input.GetKey(KeyCode.LeftArrow))
             {
                 rg.AddForce(new Vector3(-10f, 20f, 0));
                 skeletonAnimation.AnimationName = attackAnimation;
             GameManager.Instance.isAttack = 1;
             object[] datas = new object[] { GameManager.Instance.isAttack,view.ViewID };
             PhotonNetwork.RaiseEvent(ANIM_ATTACK_EVENT, datas, Photon.Realtime.RaiseEventOptions.Default, SendOptions.SendUnreliable);
         }
         else if (Input.GetKey(KeyCode.RightArrow))
             {
                 rg.AddForce(new Vector3(10f, 20f, 0));
                 skeletonAnimation.AnimationName = attackAnimation;
             GameManager.Instance.isAttack = 1;
             object[] datas = new object[] { GameManager.Instance.isAttack, view.ViewID };
             PhotonNetwork.RaiseEvent(ANIM_ATTACK_EVENT, datas, Photon.Realtime.RaiseEventOptions.Default, SendOptions.SendUnreliable);
         }
         else if (Input.GetKey(KeyCode.UpArrow))
             {
                 rg.AddForce(new Vector3(0, 2f, 0));
             }
             else
             {

             }

         }
         //else
         //{

         //}
         if(compareGameobject!=null)
         {
             GameManager.Instance.compareAltitude(compareGameobject);
         }

     else
     {

     }*/
        //}




    }
    void Apply(SkeletonRenderer skeletonRenderer)
    {
        StartCoroutine(Blink());
    }
    IEnumerator Blink()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.25f, 3f));
            skeletonAnimation.Skeleton.SetAttachment(eyesSlot, blinkAttachment);
            yield return new WaitForSeconds(blinkDuration);
            skeletonAnimation.Skeleton.SetAttachment(eyesSlot, eyesOpenAttachment);
        }
    }
    
    public void setAnimSpine(string AnimName)
    {
       
      skeletonAnimation.AnimationName = AnimName;
       
    }
    public void Push(Vector2 Force)
    {
        if (view.ViewID==1001)
        {
            rg.AddForce(Force, ForceMode2D.Impulse);
            skeletonAnimation.AnimationName = attackAnimation;
            isAttack1 = true;

            object[] datas = new object[] { isAttack1,isAttack2, view.ViewID };
            PhotonNetwork.RaiseEvent(ANIM_ATTACK_EVENT, datas, Photon.Realtime.RaiseEventOptions.Default, SendOptions.SendUnreliable);

            Debug.Log("isattack: " + isAttack1);
        }
        else if(view.ViewID==2001)
        {
            rg.AddForce(Force, ForceMode2D.Impulse);
            skeletonAnimation.AnimationName = attackAnimation;
            isAttack2 = true;

            object[] datas = new object[] { isAttack1, isAttack2, view.ViewID };
            PhotonNetwork.RaiseEvent(ANIM_ATTACK_EVENT, datas, Photon.Realtime.RaiseEventOptions.Default, SendOptions.SendUnreliable);

            Debug.Log("isattack: " + isAttack2);
        }    
    
    }    

    public void activateRb()
    {
       // rg.isKinematic = false;
    }    
    public void desactivateRb()
    {
      // rg.isKinematic = true;
      // rg.velocity = Vector2.zero;
        //rg.angularVelocity=0f;
    }    

    private void addForceDead(int viewID,Rigidbody2D rg)
    {
        if(viewID==1001)
        {
            rg.AddForce(new Vector3(-10f, 20f, 0)*50);
        }
        else if(viewID==2001)
        {
            rg.AddForce(new Vector3(10f, 20f, 0)*50);
        }
    }
    public void compareAltitude(Transform compareGameobject)
    {
        if (compareGameobject != null)
        {
            var viewID = compareGameobject.GetComponent<PhotonView>().ViewID;
            var rb = compareGameobject.GetComponent<Rigidbody2D>();
            GameManager.Instance.Panel.SetActive(true);
            if (compareGameobject.position.y < gameObject.transform.position.y && GameManager.Instance.isEndGame == 0)
            {
                addForceDead(viewID,rb);
                GameManager.Instance.lose.enabled = true;
                GameManager.Instance.isEndGame = 1;
            }
            else if(compareGameobject.position.y> gameObject.transform.position.y && GameManager.Instance.isEndGame == 0)
            {
                //addForceDead(viewID,rb);
                GameManager.Instance.win.enabled = true;
                GameManager.Instance.isEndGame = 1;
            }
        }
    }
}
