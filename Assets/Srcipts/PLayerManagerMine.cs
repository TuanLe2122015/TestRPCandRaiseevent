using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLayerManagerMine : MonoBehaviour
{
    public string InGameName = "";
    
    public static PLayerManagerMine Instance;

    byte UserType; // Cung~ la` User Slot
    // UserType = 0 is player with Red team
    //          = 1 is teamate of Red player
    //          2 -> 9 is viewer of Rec team

    // UserType = 10 is player with Black team
    // UserType = 11 is teamate of Black team
    //          12 -> 19 is viewwer of Black team

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void SetUserType(byte typeUs)
    {
        UserType = typeUs;
    }

    public byte GetUserType()
    {
        return UserType;
    }
}
