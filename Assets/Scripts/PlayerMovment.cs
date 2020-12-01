using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerMovment : MonoBehaviour
{
    public GameManager gameManager;
    public InventoryManager Inventory;

    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;

    
    private bool isPlayerOnTestLine=false;
    private bool TestBegin;

    Vector2 movement;
    private Vector2 drunkVector;
    public ConstantForce2D drunkforce;
    public GameObject endTestLine;
    public GameObject endTestPukeOn;
    public GameObject StrightLine;
    public GameObject CrazyLine;

    public GameObject Smoke;//Prefab of SmokeWeed Effect
    private Transform[] smokePos; // Var Containt Player Child gameobject (SmokePos) transform

    float JointUse = 100f;


    private void Start()
    {
        drunkVector = drunkforce.relativeForce;
        smokePos = GetComponentsInChildren<Transform>();
       
        
    }


    void Update()
    {
        //Input

        movement.x= Input.GetAxisRaw("Horizontal"); //Left , Right 
        movement.y = Input.GetAxisRaw("Vertical");// up , down
        // Animation Controll
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
        PlayerNotInTheLine();
        CheackForDirction();
        SmokeOn();
        FillJointUse();
        
        


    }

    void FixedUpdate()
    {
        //Movement
        if (gameManager.canPlayerMove)
        {
            drunkVector = new Vector2(Random.Range(-moveSpeed*0.88f*100f, moveSpeed * 0.88f*100f), Random.Range(-moveSpeed * 0.88f*100f, moveSpeed * 0.88f*100f));
            drunkforce.relativeForce = drunkVector;
            rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
            animator.SetBool("CanPlayerMove", true);
            if (movement == new Vector2(0, 0))
            {
                drunkforce.relativeForce = new Vector2(0, 0);
                drunkforce.torque = 0;

            }
            else
            {
                drunkforce.torque = 1;
            }

        }
        else
        {
            animator.SetBool("CanPlayerMove", false);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag=="TestLine")
        {
            Debug.Log("Player On Test Line");
            isPlayerOnTestLine = true;
            TestBegin = true;
        }

        if(other.name== "EndTestLine")
        {
            StartCoroutine("EndTestLine");
        }

        if (other.name == "EndTestPukeOn")
        {
            StartCoroutine("NextLevel");
            

        }

    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "TestLine")
        {
            Debug.Log("Player left Test Line");
            isPlayerOnTestLine = false;
        }
    }
      public void PlayerNotInTheLine()
    {
        if(isPlayerOnTestLine==false&&TestBegin==true)
        {
            gameManager.timeOffRoad +=1;
            Debug.Log(gameManager.timeOffRoad);
        }

    }
    IEnumerator EndTestLine()
    {
        TestBegin = false;
        rb.bodyType = RigidbodyType2D.Static;
        gameManager.canPlayerMove = false;
        endTestLine.gameObject.SetActive(false);
        StrightLine.gameObject.SetActive(false);
        gameManager.EndFirstLineTestDialogueTrigger();
        yield return new WaitForSeconds(2f);
        Debug.Log("You Doing Great!!,Now Come Back");
        rb.bodyType = RigidbodyType2D.Dynamic;
        gameManager.canPlayerMove = true;
        endTestPukeOn.gameObject.SetActive(true);
        CrazyLine.gameObject.SetActive(true);
        TestBegin = true;


    }
    void CheackForDirction()
    {
        if (movement.x == 1) animator.SetInteger("Direction", 1);//Right
        else if(movement.x == -1) animator.SetInteger("Direction", 2);//Left
        else if (movement.y == 1) animator.SetInteger("Direction", 3);//UP
        else if (movement.y == -1) animator.SetInteger("Direction", 4);//Down
        Debug.Log(animator.GetInteger("Direction"));
    }
    IEnumerator NextLevel()
    {
        TestBegin = false;
        animator.SetBool("EndTestPukeOn", true);
        gameManager.canPlayerMove = false;
        rb.bodyType = RigidbodyType2D.Static;
        yield return new WaitForSeconds(5);
        gameManager.NextLevel();
            
    }
    void SmokeOn()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            if (JointUse > 0f)
            {
                JointUse = JointUse-25f;
                GameObject smokeRef = Instantiate(Smoke, smokePos[1].position, smokePos[1].rotation);// smokePos[1] = SmokePos transform

                Inventory.DecreasJointMeter();

                StartCoroutine(SetSmokingBool());
            }
            else Debug.Log("no more joint");
        }
        
    }
    
    IEnumerator SetSmokingBool()
    {
        animator.SetBool("IsSmoking", true);
        yield return new WaitForSeconds(0.9f);
        animator.SetBool("IsSmoking", false);
        yield return new WaitForSeconds(9f);
        

    }
    void FillJointUse()
    {
        if (JointUse < 100f)
        {
            JointUse = JointUse + 0.05f;
            Inventory.IncreasJointMeter();
        }
            
        
    }

}
