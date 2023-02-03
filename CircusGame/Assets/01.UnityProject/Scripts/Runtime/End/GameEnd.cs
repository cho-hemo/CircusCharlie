using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnd : MonoBehaviour
{
    private GameObject UICanvas = default;
    private GameObject gameEndTxt = default;
    private GameObject scoreTxt = default;
    // Start is called before the first frame update
    void Start()
    {
        UICanvas = GFunc.GetRootObj(GData.UICANVAS_OBJ_NAME);
        gameEndTxt = GFunc.FindChildObj(UICanvas, "Since");
        scoreTxt = GFunc.FindChildObj(UICanvas, "ScoreNumTxt");

        if(PlayerPrefs.GetInt("win",0) == 1)
        gameEndTxt.SetTmpText("GAME WIN");
        else
        gameEndTxt.SetTmpText("GAME OVER");

        scoreTxt.SetTmpText($"{PlayerPrefs.GetInt("score",0)}");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown)
        {
            GFunc.LoadScene(GData.SCENE_NAME_STAGE_1);
        }
    }
}
