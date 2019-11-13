using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController2 : MonoBehaviour {

    public GameObject[] harzards;
    public Vector3 spawnValue;
    public int spawncount;

    public float startwait;
    public float spawnwait;
    public float wavewait;

    public Text scoreText;
    public Text gameoverText;
    public Text restartText;

    private int score = 0;
    private bool gameover = false;
    private bool restartgame = false;

	// Use this for initialization
    void Start () {
        gameoverText.text = "";
        restartText.text = "";
        scoreUpdate();
        StartCoroutine(harzardWaves());
	}

    void Update()
    {
        if(restartgame)
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }
        }
    }

    IEnumerator harzardWaves()
    {
        yield return new WaitForSeconds(startwait);
        while (true)
        {
            for (int i = 0; i < spawncount; i++)
            {
                GameObject harzard = harzards[Random.Range(0, harzards.Length)];
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValue.x, spawnValue.x), spawnValue.y, spawnValue.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(harzard, spawnPosition, spawnRotation);
                Debug.Log(spawnRotation.x);
                yield return new WaitForSeconds(spawnwait);
            }
            yield return new WaitForSeconds(wavewait);

            if(gameover)
            {
                restartgame = true;
                restartText.text = "Press 'R' To restart";
                break;
            }
        }
    }

    public void addscore(int value)
    {
        score += value;
        scoreUpdate();
    }

    void scoreUpdate()
    {
        scoreText.text = "Score: " + score;
    }

    public void Gameover()
    {
        gameover = true;
        gameoverText.text = "Game Over";

    }

}
