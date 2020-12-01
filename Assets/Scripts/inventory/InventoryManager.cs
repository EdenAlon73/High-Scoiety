using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject Key;
    [SerializeField] private GameObject File;
    [SerializeField] private GameObject Player;
    [SerializeField] private OpenOfficeDoor OfficeDoor;
    [SerializeField] private GameObject FullJoint;
    [SerializeField] private GameObject SecondFullJoint;
    [SerializeField] private GameObject ThirdFullJoint;
    [SerializeField] private GameObject FourthFullJoint;
    [SerializeField] private GameObject FithFullJoint;
    [SerializeField] private WinChacker winChacker;
    private bool PlayerHasKey = false;
    private bool PlayerHasFile = false;

    private float JointMeter =100f;
    public void PickedKeyItem()
    {
        Key.SetActive(true);
        PlayerHasKey = true;
        Debug.Log("KEY");
    }

    public void PickedFilesItem()
    {
        File.SetActive(true);
        PlayerHasFile = true;
        Debug.Log("YES");
        
    }

    private void OfficeDoorOpenenr()
    {
        if(PlayerHasKey == true)
        {
            OfficeDoor.OpenDoor();
        }
    }

    private void WinDoorOpener()
    {
        if (PlayerHasFile == true)
        {
            
            winChacker.WinConditionOn();
           
        }
        
    }

    public void DecreasJointMeter()
    {
        if (JointMeter > 0f)
        {
            JointMeter = JointMeter - 25f;
        }
    }

    public void IncreasJointMeter()
    {
        if(JointMeter < 100f)
        {
        JointMeter = JointMeter + 0.05f;

        }
    }

    private void Update()
    {
        JointMeterGfx();
        OfficeDoorOpenenr();
        WinDoorOpener();
        Debug.Log(PlayerHasFile);
        Debug.Log(PlayerHasKey);
    }

    private void JointMeterGfx()
    {
        if(JointMeter >= 100f)
        {
            FullJoint.SetActive(true);
            SecondFullJoint.SetActive(false);
            ThirdFullJoint.SetActive(false);
            FourthFullJoint.SetActive(false);
            FithFullJoint.SetActive(false);
        }

        if(JointMeter >= 75f && JointMeter < 100f)
        {
            SecondFullJoint.SetActive(true);
            FullJoint.SetActive(false);
            ThirdFullJoint.SetActive(false);
        }

        if(JointMeter >= 50f && JointMeter < 75f)
        {
            ThirdFullJoint.SetActive(true);
            SecondFullJoint.SetActive(false);
            FourthFullJoint.SetActive(false);
        }

        if(JointMeter >= 25f && JointMeter < 50f)
        {
            FourthFullJoint.SetActive(true);
            ThirdFullJoint.SetActive(false);
            FithFullJoint.SetActive(false);
        }

        if(JointMeter > 0f && JointMeter < 25)
        {
            FithFullJoint.SetActive(true);
            FourthFullJoint.SetActive(false);

        }

        if(JointMeter <= 0f)
        {
            FithFullJoint.SetActive(false);
        }
    }

}
