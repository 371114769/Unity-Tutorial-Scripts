using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public float speed;
    public Text scoretext;
    public Text winText;

    private int score = 0;
    private Rigidbody2D rb2d;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        UpdateScore();
        winText.text = "";
    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        rb2d.AddForce(movement * speed);


    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("PickUp"))
        {
            Destroy(other.gameObject);
            score += 1;
            UpdateScore();
        }
        if (score >= 7)
        {
            winText.text = "You Win!!!";
        }
    }

    void UpdateScore()
    {
        scoretext.text = "Score: " + score;
    }


}
