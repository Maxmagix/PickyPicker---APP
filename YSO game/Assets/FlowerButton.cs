using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlowerButton : MonoBehaviour
{
    [SerializeField]
    GameObject prefabParticle;
    [SerializeField]
    RawImage image;
    [SerializeField]
    Color correctColor;
    [SerializeField]
    Color badColor;

    public bool correct = false;
    public bool clicked = false;

    public void clickedOn()
    {
        if (clicked)
            return;
        GameObject scorer = GameObject.FindGameObjectWithTag("ScoreManager");
        ScoreManager manager = scorer.transform.GetComponent<ScoreManager>();
        manager.scoreFlower(correct);
        if (correct)
        {
            Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
            Instantiate(prefabParticle, pos, Quaternion.identity);
        }
        image.color = correct ? correctColor: badColor;
        clicked = true;
    }
}
