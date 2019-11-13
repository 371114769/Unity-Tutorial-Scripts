using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact2 : MonoBehaviour
{
    public GameObject explosion;
    public GameObject playerexplosion;
    public int scoreValue;
    private GameController2 gameController;

    void Start()
    {
        GameObject gamecontrollerobject = GameObject.FindWithTag("GameController");
        if(gamecontrollerobject != null)
        {
            gameController = gamecontrollerobject.GetComponent<GameController2>();
        }
        if(gamecontrollerobject == null)
        {
            Debug.Log("can't find 'GameController2' script");
        }
    }


    // Destroy everything that enters the trigger
    void OnTriggerEnter(Collider other){
    
        if(other.CompareTag("Boundary") || other.CompareTag("Enemy"))
        {
            return;
        }

        if (explosion != null)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }

        if (other.tag == "Player")
        {
            Debug.Log(other.name + "contacted");
            Instantiate(playerexplosion, other.transform.position, other.transform.rotation);
            gameController.Gameover();
            scoreValue = 0;
        }
        gameController.addscore(scoreValue);
        Destroy(other.gameObject);
        Destroy(gameObject);

    }


}
