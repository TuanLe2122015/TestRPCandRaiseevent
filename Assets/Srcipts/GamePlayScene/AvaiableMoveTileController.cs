using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvaiableMoveTileController : MonoBehaviour
{
    // Srcipt nay` dung` de di chuyen () 4 dau' muoi~ ten quanh o co` (Tile)

    [SerializeField]
    GameObject Center, Con, Con_1, Con_2, Con_3;
    Vector3 direct_1, direct_2, direct_3, direct_4;

    const float timeRotateDirect = 0.1f;
    const float speed = 0.4f;

    float curTime;
    int directInt = -1;
    float distThisFrame;

    private void Awake()
    {
        curTime = Time.time;
        Center = transform.GetChild(4).gameObject;
        Con = transform.GetChild(5).gameObject;
        Con_1 = transform.GetChild(6).gameObject;
        Con_2 = transform.GetChild(7).gameObject;
        Con_3 = transform.GetChild(8).gameObject;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        // Moi~ 0.3s dao~ chieu` dy chuyen~ muoi~ ten
        distThisFrame = speed * Time.deltaTime;

        direct_1 = Con.transform.position - Center.transform.position;
        direct_2 = Con_1.transform.position - Center.transform.position;
        direct_3 = Con_2.transform.position - Center.transform.position;
        direct_4 = Con_3.transform.position - Center.transform.position;

        Vector3 directArrow = transform.GetChild(0).transform.position - Center.transform.position;

        transform.GetChild(0).Translate(direct_1.normalized * directInt * distThisFrame, Space.World);
        transform.GetChild(1).Translate(direct_2.normalized * directInt * distThisFrame, Space.World);
        transform.GetChild(2).Translate(direct_3.normalized * directInt * distThisFrame, Space.World);
        transform.GetChild(3).Translate(direct_4.normalized * directInt * distThisFrame, Space.World);

        if ((directArrow.magnitude < distThisFrame + 0.55f || directArrow.magnitude > distThisFrame + 0.9f) && Time.time - curTime > timeRotateDirect)
        {
            directInt *= -1;
            curTime = Time.time;
        }
    }
}
