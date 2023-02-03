using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private const string SCORE_OBJ_NAME = "PlayerScoreTxt";
    private const string HIGH_OBJ_NAME = "HighScoreTxt";
    private const string BONUS_OBJ_NAME = "BonusScoreTxt";
    private const float MOVE_SPEED = 7f;
    private const float JUMP_FORCE = 700f;
    private GameObject GameObjectCanvas = default;
    private GameObject UICanvas = default;
    private GameObject mainCamera = default;
    private GameObject scoreTxt = default;
    private GameObject highScoreTxt = default;
    private GameObject bonusTxt = default;
    private Rigidbody2D rigidbody = default;
    private Animator charAnim = default;
    private Animator rionAnim = default;
    private int score = 0;
    private int highScore = 0;
    private int bonusScore = 0;
    private float time = 0;
    private bool jump = false;
    private bool stop = false;
    private bool isEnd = false;
    private bool isGameOver = false;


    // Start is called before the first frame update
    void Start()
    {
        GameObjectCanvas = GFunc.GetRootObj(GData.CANVAS_OBJ_NAME);
        mainCamera = GFunc.FindChildObj(GameObjectCanvas, GData.CAMERA_OBJ_NAME);
        rigidbody = gameObject.GetComponentMust<Rigidbody2D>();
        charAnim = GFunc.FindChildObj(GameObjectCanvas, GData.CHAR_OBJ_NAME).GetComponentMust<Animator>();
        rionAnim = GFunc.FindChildObj(GameObjectCanvas, GData.RION_OBJ_NAME).GetComponentMust<Animator>();
        
        UICanvas = GFunc.GetRootObj(GData.UICANVAS_OBJ_NAME);
        scoreTxt = GFunc.FindChildObj(UICanvas, SCORE_OBJ_NAME);
        highScoreTxt = GFunc.FindChildObj(UICanvas, HIGH_OBJ_NAME);
        bonusTxt = GFunc.FindChildObj(UICanvas, BONUS_OBJ_NAME);
        highScore = PlayerPrefs.GetInt("high",0);
        PlayerPrefs.SetInt("score",0);
        bonusScore = 5000;
        scoreTxt.SetTmpText($"player-{score}");
        highScoreTxt.SetTmpText($"high-{highScore}");
    }

    // Update is called once per frame
    void Update()
    {
        if(highScore<score)
        {
            highScore = score;
            highScoreTxt.SetTmpText($"high-{highScore}");
            PlayerPrefs.SetInt("high",highScore);
        }

        if (!isEnd&&!isGameOver)
        {
            if(bonusScore>=0)
            Bonus();
            StopAnim();
            if (mainCamera.transform.position.x <= 0)
            {
                stop = true;
            }
            else
            {
                stop = false;
            }
            // -> 방향키를 눌렀을 때 앞으로 이동
            if (Input.GetKey(KeyCode.RightArrow))
            {
                MoveAnim();
                mainCamera.transform.position += Vector3.right *Time.deltaTime * MOVE_SPEED;
                gameObject.transform.position += Vector3.right *Time.deltaTime * MOVE_SPEED;
            }
            if (Input.GetKey(KeyCode.LeftArrow) && !stop)
            {
                MoveAnim();
                mainCamera.transform.position += Vector3.left*Time.deltaTime * MOVE_SPEED;
                gameObject.transform.position += Vector3.left*Time.deltaTime * MOVE_SPEED;
            }
            if (Input.GetKeyDown(KeyCode.Space) && !jump)
            {
                jump = true;
                rigidbody.AddForce(Vector2.up * JUMP_FORCE);
            }
        }
    }

    private void Bonus()
    {
        time += Time.deltaTime*2;
        if(time > 1)
        {
            bonusScore -= (int)time;
            bonusTxt.SetTmpText($"{bonusScore}");
            time = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Enemy")
        {
            isGameOver = true;
            GameOverAnim();
            StartCoroutine(Die());
        }
        else if (other.transform.tag == "Money")
        {
            other.gameObject.SetActive(false);
            score += 500;
            scoreTxt.SetTmpText($"player-{score}");
        }
        else if (other.transform.tag == "Ring")
        {
            score += 300;
            scoreTxt.SetTmpText($"player-{score}");
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "End")
        {
            isEnd = true;
            WinAnim();
            StartCoroutine(Win());
        }
        else if(other.transform.tag == "Bottom")
        {
            jump = false;
        }
    }

    private void MoveAnim()
    {
        charAnim.SetBool("Move", true);
        rionAnim.SetBool("Move", true);
    }

    private void StopAnim()
    {
        charAnim.SetBool("Move", false);
        rionAnim.SetBool("Move", false);
    }
    
    private void GameOverAnim()
    {
        charAnim.SetTrigger("GameOver");
        rionAnim.SetTrigger("GameOver");
    }
    
    private void WinAnim()
    {
        charAnim.SetTrigger("Win");
        rionAnim.SetTrigger("Win");
    }

    private void SceneChange()
    {
        PlayerPrefs.SetInt("score",score);
        GFunc.LoadScene(GData.SCENE_NAME_GAMEOVER);
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(3);
        PlayerPrefs.SetInt("win",0);
        SceneChange();
    }

    private IEnumerator Win()
    {
        yield return new WaitForSeconds(0.5f);
        score = bonusScore;
        bonusScore = 0;
        scoreTxt.SetTmpText($"player-{score}");
        yield return new WaitForSeconds(0.5f);
        bonusTxt.SetTmpText($"{bonusScore}");
        yield return new WaitForSeconds(3);
        PlayerPrefs.SetInt("win", 1);
        SceneChange();
    }

}
