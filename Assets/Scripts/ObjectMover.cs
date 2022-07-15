using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : DataReader
{
    // Ball
    [SerializeField] private GameObject ball;

    // FPS
    [SerializeField] private int framesPerSecond = 25;

    // UI Anims
    [SerializeField] private Animator uiAnimator;

    // Play the sequence, called on button press
    public void PlaySequence()
    {
        StartCoroutine(MoveObjects());
    }

    IEnumerator MoveObjects()
    {
        // Play UI fade out anim
        uiAnimator.SetTrigger("Fade Out");

        // Move players per frame, wait 1/25 sec per frame for 25 fps play speed
        foreach (int frame in framesDict.Keys)
        {
            // Move players
            foreach (KeyValuePair<string, Vector3> playerPos in framesDict[frame])
            {
                Debug.Log("frame: " + frame + ", player: " + playerPos.Key + ", position: " + playerPos.Value);

                // CHANGE! - Find() is very inefficient, should store references
                GameObject player = GameObject.Find(playerPos.Key.ToString());
                player.transform.position = playerPos.Value;
            }

            // Move ball
            ball.transform.position = ballPosDict[frame];

            yield return new WaitForSeconds(1 / framesPerSecond);
        }

        // Play UI fade in anim
        uiAnimator.SetTrigger("Fade In");
    }
}
