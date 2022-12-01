using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour //Class that chamges color of gradient for specified sprite
{
    public Gradient Gradient; //adjust gradient
    public SpriteRenderer SpriteRenderer; //get sprite that will be dimmed

    public float MaxHealth = 100f; //high the number the brighter it will start out as, and will take longer to dim:
    private float currentHealth { get; set; }

    // Start is called before the first frame update
    void Start()
    {
    currentHealth = MaxHealth;
    Color color = Gradient.Evaluate(1f); //100% gradient opacity
    SpriteRenderer.color = color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other) //detects when the game object is hit
    {
        TakeDamage(5f); //decrease gradient by 5

    }
      

    public void TakeDamage(float amount) //takes damage and decreases gradient of sprite according to timePoint
    {
        currentHealth -= amount;
        float timePoint = currentHealth / MaxHealth;
        Color color = Gradient.Evaluate(timePoint);
        SpriteRenderer.color = color;
    }
}
