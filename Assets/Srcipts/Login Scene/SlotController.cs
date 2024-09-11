using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotController : MonoBehaviour
{
    Button thisBtn;
    public byte indexOfSlot = 0;
    private void Awake()
    {
        thisBtn = GetComponent<Button>();
        thisBtn.onClick.AddListener(delegate { OnMyClick(); });
    }
    private void OnMyClick()
    {
        ShowPlayerJoinRoom.Instance.OnClickSlotButton(indexOfSlot);
    }
}
