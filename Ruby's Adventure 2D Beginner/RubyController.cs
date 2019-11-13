using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    public float speed = 3.0f;
    public int maxHealth = 5;
    public int health { get { return currentHealth; } }

    int currentHealth;
    Rigidbody2D rigidbody2d;
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        Vector2 position = rigidbody2d.position;
        position.y = position.y + speed * vertical * Time.deltaTime;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        rigidbody2d.MovePosition(position);
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }
    }
    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        Debug.Log(currentHealth + "/" + maxHealth);
    }
}
