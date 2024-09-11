using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewController : MonoBehaviour
{
    [SerializeField]
    GameObject objMarkLeft;
    [SerializeField]
    GameObject objAnchorLeft;

    ViewportHandler mainCamViewport;
    // Start is called before the first frame update

    private void Awake()
    {
        mainCamViewport = this.GetComponent<ViewportHandler>();
    }
    void Start()
    {
        float posX_MarkLeft = objMarkLeft.transform.position.x;
        float posX_AnchorLeft = objAnchorLeft.transform.position.x;

        posX_AnchorLeft = posX_AnchorLeft - posX_MarkLeft;

        mainCamViewport.UnitsSize = mainCamViewport.UnitsSize + posX_AnchorLeft*3.333f;
    }

}
