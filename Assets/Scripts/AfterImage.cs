using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImage : MonoBehaviour
{
    // Time of which the next image is drawn
    [SerializeField]
    private float imageRate;

    // How long it takes for the image to fade to nothing
    [SerializeField]
    private float imageLife;

    // Colour of the after image at start
    Color startColour;
    Queue<GameObject> imageQueue = new Queue<GameObject>();
    GameObject nextInQueue;

    public void StartAfterImage()
    {
        // Stop after image
        StopAfterImage();

        // invoke a repeating method using imageRate as the repeat rate
        InvokeRepeating("SpawnImage", 0, imageRate);
    }

    public void StopAfterImage()
    {
        CancelInvoke();
    }

    public void SetColour(Color c)
    {
        startColour = c;
    }

    void SpawnImage()
    {
        // if nextInQueue is null and our queue is not empty, or if nextInQueue is NOT null and the spriteRenderer alpha is <= 0, then assign nextInQueue to the next item in our Queue
        if ((nextInQueue == null && imageQueue.Count > 0) || (nextInQueue != null && nextInQueue.GetComponent<SpriteRenderer>().color.a <= 0))
        {
            nextInQueue = imageQueue.Dequeue();
        }
        
        // Create an empty game object
        GameObject obj;
        SpriteRenderer sr;

        // if nextInQueue is NOT null and the spriteRenderer alpha is <= 0, assign obj to next and the sprite renderer of that to sr
        if (nextInQueue != null && nextInQueue.GetComponent<SpriteRenderer>().color.a <= 0)
        {
            obj = nextInQueue;
            sr = nextInQueue.GetComponent<SpriteRenderer>();
            nextInQueue = null;
        }
        // else, create an empty game object and sprite renderer
        else
        {
            obj = new GameObject();
            sr = obj.AddComponent<SpriteRenderer>();
        }

        // Add a sprite renderer to the new object and copy this object's current sprite to the new one
        sr.sprite = GetComponent<SpriteRenderer>().sprite;

        // Copy this object's position and local scale and put it into the new object
        obj.transform.position = transform.position;
        obj.transform.localScale = transform.localScale;

        // Pass the sprite renderer of the new object into a coroutine to fade out the object
        StartCoroutine(FadeImage(sr));

        // add obj to the queue
        imageQueue.Enqueue(obj);
    }

    IEnumerator FadeImage(SpriteRenderer sr)
    {
        // Create a new colour, which is a copy of the target colour with the alpha set to 0
        Color target = startColour;
        target.a = 0;

        // create a time counter
        float time = 0;

        // in a while loop, lerp the colour of the sprite renderer that starts at the start colour
        // and ends with the target colour. Loop ends when the time counter > imageLife
        do
        {
            time += Time.deltaTime;
            sr.color = Color.Lerp(startColour, target, time / imageLife);
            yield return null;
        } while (time < imageLife);

        // After the loop, set the sprite renderer colour to the target manually, in case the lerp didnt fully complete
        sr.color = target;
    }
}
