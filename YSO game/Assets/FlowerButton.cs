using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlowerButton : MonoBehaviour
{
    [SerializeField]
    RawImage image;
    [SerializeField]
    Button button;
    public bool correct = false;

    public void clickedOn()
    {
        GameObject scorer = GameObject.FindGameObjectWithTag("ScoreManager");
        ScoreManager manager = scorer.transform.GetComponent<ScoreManager>();
        //if (!manager.checkGameRunning())
            //return;
        manager.scoreFlower(correct);
        image.color = correct ? Color.yellow : Color.red;
        button.interactable = false;
    }
}
