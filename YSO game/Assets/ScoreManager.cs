using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int spawnedFlowers = 0;
    private int correctFlowers = 0;
    private int incorrectFlowers = 0;

    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text errorText;
    [SerializeField]
    private Image queenReaction;
    [SerializeField]
    private Image frameImage;
    [SerializeField]
    private Text commentText;
    [SerializeField]
    private Sprite[] queenFaces;
    [SerializeField]
    private Sprite[] frameSkins;
    [SerializeField]
    private string[] textsReact;

    [SerializeField]
    private GameObject debriefOverlay;

    enum QUEEN_STATES
    {
        NEUTRAL,
        HAPPY,
        SAD
    };

    public void resetScore()
    {
        spawnedFlowers = 0;
        correctFlowers = 0;
        incorrectFlowers = 0;
    }

    public void scoreFlower(bool correct)
    {
        if (correct)
            correctFlowers++;
        else
            incorrectFlowers++;
    }

    public void showResult()
    {
        debriefOverlay.SetActive(true);
        scoreText.text = string.Format("{0}/{1}", correctFlowers, spawnedFlowers);
        errorText.text = string.Format("{0} INCORRECT", incorrectFlowers);
        //perfect
        if (correctFlowers == spawnedFlowers && incorrectFlowers == 0)
        {
            commentText.text = textsReact[0];
            queenReaction.sprite = queenFaces[0];
            frameImage.sprite = frameSkins[0];
        }
        //BAD
        else if (1 + incorrectFlowers > correctFlowers * 2)
        {
            commentText.text = textsReact[3];
            queenReaction.sprite = queenFaces[2];
            frameImage.sprite = frameSkins[3];
        }
        //GOOD
        else if ((correctFlowers + spawnedFlowers) - incorrectFlowers >= spawnedFlowers)
        {
            commentText.text = textsReact[1];
            queenReaction.sprite = queenFaces[0];
            frameImage.sprite = frameSkins[1];
        }
        //OK
        else
        {
            commentText.text = textsReact[2];
            queenReaction.sprite = queenFaces[1];
            frameImage.sprite = frameSkins[2];
        }
    }
}
