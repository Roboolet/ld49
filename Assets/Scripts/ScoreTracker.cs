using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTracker : MonoBehaviour
{
    [SerializeField] Image forwardButtonImage;
    [SerializeField] GameObject backButton;
    [SerializeField] LevelPoints[] levels;
    Dictionary<int, LevelPoints> levelPoints = new Dictionary<int, LevelPoints>();


    Button forwardButton;
    int currentLevel;

    int _Score;
    public int Score
    {
        get { return _Score; }
        set
        {
            _Score = value;
            if (levelPoints[currentLevel].pointsReq > 0)
            {
                if (_Score > 0)
                {
                    forwardButtonImage.fillAmount = (float)_Score / (float)levelPoints[currentLevel].pointsReq;

                    if (_Score >= levelPoints[currentLevel].pointsReq) BeatLevel();
                    else forwardButton.interactable = false;
                }
                else
                {
                    forwardButton.interactable = false;
                    forwardButtonImage.fillAmount = 0;
                }                
            }
            else BeatLevel();            
        }
    }

    void BeatLevel()
    {
        forwardButton.interactable = true;
        forwardButtonImage.fillAmount = 1;
        levelPoints[currentLevel].beaten = true;
    }

    private void Awake()
    {
        for(int i = 0; i < levels.Length; i++)
        {
            levelPoints.Add(i, levels[i]);
        }

        forwardButton = forwardButtonImage.GetComponent<Button>();
    }

    public void SetCurrentLevel(int i)
    {
        currentLevel = i;
        if (i > 0)
        {
            Score = 0;
            backButton.SetActive(true);
        }
        else
        {
            backButton.SetActive(false);
        }
        
        if(levelPoints[i].beaten || i <= 0)
        {
            BeatLevel();
            Score = 999;
            
        }
    }

    public void Increment()
    {
        Score++;
    }

    public void ResetScore() => SetCurrentLevel(currentLevel);
}

[System.Serializable]
public class LevelPoints
{
    public int pointsReq;
    public bool beaten;
}
