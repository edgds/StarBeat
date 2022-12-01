using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Note : MonoBehaviour
{
    public Vector2 center; // determines where the "center" is, and where the notes move towards
    private Vector2 notePosition; // holds x, y coordinates of note
    private Vector2 tapPosition; // holds x, y coordinates of tap

    public float speed; // determines how fast note moves towards center
    public float range; // determines how far from the center of the note you can tap for the ntoe to count as being tapped (larger = further)

    public AudioClip _audioClip; // audio for tap star burst effect
    public Transform explosion; //tapped note effect
    public Transform redSpark;//missed note effect
    public Transform spawn; //spawn effect
    public GameObject myObject; //gameobject that dims everytime ntoe is missed (only in butter level)
    

    private int collideCount; // record how many ring colliders a note has hit, will be used to determine accuracy when note is tapped or missed
    private double[] noteAccuracy; // values for how accurate a note is, index of variable correspodns to collideCount

    static private float totalAccuracy; // records averaged-out accuracy over course of the game
    
    // counters for how many of each level of accuracy player has scored, making it a separate class lets the array values carry over between note scripts
    public class Global
    {
        public static int[] numAccuracy = new int[] { 0, 0, 0, 0 }; // # of times player scored a 0, 25, 75, or 100;
    }

    public GameObject rotationPoint; // 
    public TMP_Text scoreText; // Score UI text
    public int rotationSpeed; // 
    

    // Start is called before the first frame update
    void Start()
    {
        Transform newExplosion2 = Instantiate(spawn, transform.position, transform.rotation);
        Destroy(newExplosion2.gameObject, 2.0f);

        scoreText = GameObject.FindGameObjectWithTag("scoreboard").GetComponent<TMP_Text>(); // link the scoreboard in Unity to scoreText

//        GetComponent<SpriteRenderer>().enabled = true;

        notePosition = transform.position; // record coordinates of note
//        Debug.Log("Note coordinates: " + notePosition);

        center = new Vector2(0.0f, 0.0f); // sets center coordinates to 0, 0; might be redundant given public declaration

        collideCount = 0; // initialize # of colliders hit
        noteAccuracy = new double[] { 0.0, 25.0, 75.0, 100.0, 75.0, 25.0, 0.0, 0.0 }; // set accuracy for each stage/collider note goes through (less -> more -> less)
//        Debug.Log("Ring colliders hit: " + collideCount + ", Current Note accuracy: " + tapAccuracy);

        totalAccuracy = 0.00f; // initialize current score as 0; might be redundant given allocation below
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveCenter(); // every frame, note is moved toward center
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) // if touch detected on screen...
        {
            Touch touch = Input.GetTouch(0); // record touch
            tapPosition = Camera.main.ScreenToWorldPoint(touch.position); // record coordinates of touch

//            Debug.Log("Type: " + touch.type);
//            Debug.Log("Raw Pos X, Y: " + touch.rawPosition.x + ", " + touch.rawPosition.y);

//            Debug.Log("Tap detected at: " + tapPosition);
            if (Vector2.Distance(tapPosition, notePosition) < range) // if touch coordinates are within a certain distance (rwange) of note coordinates (middle of note)...
            {
                //                Debug.Log("Note tapped, range: " + Vector2.Distance(tapPosition, notePosition));
                //                Debug.Log("Note tapped, accuracy: " + noteAccuracy[collideCount]);
                
                totalAccuracy = CalAccuracy(collideCount); // update overall accuracy
                scoreText.text = "Score: " + totalAccuracy.ToString("N0") + "%"; // update scoreboard using overall accuracy

                AudioSource.PlayClipAtPoint(_audioClip, transform.position, 2f); // play sound for tapping note
                Transform newExplosion = Instantiate(explosion, transform.position, transform.rotation); // explosion tap effect 
                Destroy(newExplosion.gameObject, 2.0f); // 

                //make objects with ChangeColor script lighter by calling TakeDamage function:
                //myObject references the purple planet (center object) that has the ChangeColor script
                myObject.GetComponent<ChangeColor>().TakeDamage(-5f);

                gameObject.SetActive(false); // make note invisible/non-functioning
                Destroy(gameObject); // get rid of note to reduce load on game
                
            }
        }
    }

    // moves note towards center, mimics gravity between note and center piece
    void MoveCenter()
    {
        transform.parent = null;
        float step = speed * Time.deltaTime; // use speed to determine how fast note moves

        transform.position = Vector2.MoveTowards(transform.position, center, step); // move note towards center at a rate of step
        transform.Rotate(Vector3.forward * Time.deltaTime * rotationSpeed); // 
        notePosition = transform.position; // update recorded coordinates
        rotationPoint = GameObject.FindGameObjectWithTag("Center"); // 
    }

    // when note collider contacts any other collider
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ring")) // if note collides with one of the Ring colliders...
        {
            collideCount++; // increment collideCount by 1
//            Debug.Log("Ring colliders hit: " + collideCount + ", Current Note accuracy: " + noteAccuracy[collideCount]); // check
        }

        if (other.gameObject.CompareTag("Center")) // if note collides with center piece...
        {
            Transform newExplosion2 = Instantiate(redSpark, transform.position, transform.rotation); // red spark effect
            Destroy(newExplosion2.gameObject, 2.0f);

            gameObject.SetActive(false); // deactivate note
            Destroy(gameObject); // destroy note

            totalAccuracy = CalAccuracy(collideCount); // update accuracy since note was never tapped 
            scoreText.text = "Score: " + totalAccuracy.ToString("N0") + "%"; // update scoreboard with new accuracy
//            Debug.Log("Center hit, Note active: " + gameObject.activeSelf);
        }
    }

    // calculate overall accuracy
    float CalAccuracy(int colCt)
    {
        float totAcc; // variable to hold overall accuracy, to be returned by function
        
        // increment counter (numAccuracy) for # of times accuracy hit (if hit at 100, increment numAccuracy[3] by 1) 
        if (noteAccuracy[colCt] == 0.0)
        {
            Global.numAccuracy[0] = Global.numAccuracy[0] + 1;
        }
        else if (noteAccuracy[colCt] == 25.0)
        {
            Global.numAccuracy[1] = Global.numAccuracy[1] + 1;
        }
        else if (noteAccuracy[colCt] == 75.0)
        {
            Global.numAccuracy[2] = Global.numAccuracy[2] + 1;
        }
        else if (noteAccuracy[colCt] == 100.0)
        {
            Global.numAccuracy[3] = Global.numAccuracy[3] + 1;
        }

/*        int i = 0;
        while (i < 4)
        {
            Debug.Log(Global.numAccuracy[i]);
            i++;
        }*/

        // average out all of the accuracies using their counters ((100l + 75m + 25n + 0o)/(l+m+n+o))
        totAcc = ((100*Global.numAccuracy[3]) + (75*Global.numAccuracy[2]) + (25*Global.numAccuracy[1]))/(Global.numAccuracy[0] + Global.numAccuracy[1] + Global.numAccuracy[2] + Global.numAccuracy[3]);
//        Debug.Log("Total accuracy = " + totAcc);
        return totAcc; // return overall accuracy
    }
}