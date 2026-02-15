using TMPro;
using UnityEngine;

public class HPDisplay : MonoBehaviour
{
    public TMP_Text hpText;

    void Start()
    {
        if (hpText == null)
            Debug.Log("hpText is NULL");
        else
            Debug.Log("hpText assigned");
    }

}
