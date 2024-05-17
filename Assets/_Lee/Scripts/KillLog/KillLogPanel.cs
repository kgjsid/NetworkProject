using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KillLogPanel : MonoBehaviour
{
    [SerializeField] TMP_Text killerText;
    [SerializeField] TMP_Text DieText;
    
    public void changeLog(string killer , string die )
    {
        killerText.text = killer;
        DieText.text = die;
    }
}
