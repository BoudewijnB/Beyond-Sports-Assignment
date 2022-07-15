using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataReader : MonoBehaviour
{
    public string fileName;
    private string[] trackedObjects;

    // Player positions
    // Frames dictionary, containing (key:) frame number & (value:) KVP with playerID(K) & playerPos(V)
    protected Dictionary<int, List<KeyValuePair<string, Vector3>>> framesDict = new Dictionary<int, List<KeyValuePair<string, Vector3>>>();
    private List<KeyValuePair<string, Vector3>> playerPositionsKVP = new List<KeyValuePair<string, Vector3>>();

    // Ball pos dict
    protected Dictionary<int, Vector3> ballPosDict = new Dictionary<int, Vector3>();

    private void Start()
    {
        // store data file path name and read it
        fileName = "Assets/Data/Applicant-test.dat";
        ReadFile(fileName);
    }

    void ReadFile(string fileName)
    {
        StreamReader reader = new StreamReader(fileName);

        while (!reader.EndOfStream)
        {
            // Read data file and store split data
            string line = reader.ReadLine();
            string[] values = line.Split(':');

            int frame = int.Parse(values[0]);

            // Make new KVP for new line/frame
            playerPositionsKVP = new List<KeyValuePair<string, Vector3>>();

            // Go through all tracked objects
            trackedObjects = values[1].Split(';');
            foreach (string trackedObject in trackedObjects)
            {
                if (!string.IsNullOrEmpty(trackedObject))
                {
                    // Store object's values
                    string[] objectValues = trackedObject.Split(',');
                    int team = int.Parse(objectValues[0]);
                    int trackingID = int.Parse(objectValues[1]);
                    int playerNumber = int.Parse(objectValues[2]);
                    float xPos = float.Parse(objectValues[3]) / 40; // make field size 40x smaller
                    float zPos = float.Parse(objectValues[4]) / 40; // make field size 40x smaller
                    float speed = float.Parse(objectValues[5]);

                    Debug.Log("Frame: " + frame + " Team: " + team + " TrackingID: " + trackingID + " PlayerNumber: " + playerNumber + " X-Position: " + xPos + " Y-Position: " + zPos + " Speed: " + speed);

                    // Locate player GameObject - note: Find() very inefficient should store references
                    GameObject player = GameObject.Find(team.ToString() + "." + playerNumber.ToString());

                    Vector3 playerPosition = new Vector3();

                    // Create player object if it wasn't created yet and store position values in KVP
                    if (player == null)
                    {
                        playerPosition = new Vector3(xPos, 0, zPos);
                        KeyValuePair<string, Vector3> myItem = new KeyValuePair<string, Vector3>(team.ToString() + "." + playerNumber.ToString(), new Vector3(xPos, 0, zPos));

                        // Store player pos in KVP inside dict
                        if (!framesDict.ContainsKey(frame))
                        {
                            playerPositionsKVP.Add(myItem);
                            framesDict.Add(frame, playerPositionsKVP);
                        }
                        else
                        {
                            framesDict[frame].Add(myItem);
                        }

                        // Create, position, color and name player cube objects 
                        if (team == 0)
                        {
                            player = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            player.transform.position = new Vector3(xPos, 0.5f, zPos);
                            player.GetComponent<Renderer>().material.color = Color.red;
                            player.name = team.ToString() + "."  + playerNumber.ToString();

                            //player.transform.localScale = new Vector3(40, 60, 40);
                            player.transform.localScale = new Vector3(2, 3, 2);
                        }
                        else
                        {
                            player = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            player.transform.position = new Vector3(xPos, 0.5f, zPos);
                            player.GetComponent<Renderer>().material.color = Color.blue;
                            player.name = team.ToString() + "." + playerNumber.ToString();

                            //player.transform.localScale = new Vector3(40, 60, 40);
                            player.transform.localScale = new Vector3(2, 3, 2);
                        }
                    }
                    // If player already exists, add playerPos to KVP and add KVP to dict
                    else
                    {
                        KeyValuePair<string, Vector3> myItem = new KeyValuePair<string, Vector3>(team.ToString() + "." + playerNumber.ToString(), new Vector3(xPos, 0, zPos));

                        // Store player pos in KVP inside dict
                        if (!framesDict.ContainsKey(frame))
                        {
                            playerPositionsKVP.Add(myItem);
                            framesDict.Add(frame, playerPositionsKVP);
                        }
                        else
                        {
                            framesDict[frame].Add(myItem);
                        }
                    }
                }
            }

            // Split ball data
            string[] ballData = values[2].Split(';');
            foreach (string ballDatum in ballData)
            {
                if (!string.IsNullOrEmpty(ballDatum))
                {
                    string[] objectValues = ballDatum.Split(',');
                    float xPos = float.Parse(objectValues[0]) / 40; // make field size 40x smaller
                    float zPos = float.Parse(objectValues[1]) / 40; // make field size 40x smaller
                    float yPos = float.Parse(objectValues[2]) / 40; // make field size 40x smaller
                    float ballSpeed = float.Parse(objectValues[3]);

                    Debug.Log("Frame: " + frame + " X-Position: " + xPos + " Y-Position: " + yPos + " Z-Position: " + zPos + " BallSpeed: " + ballSpeed);

                    // Store ball position in ball pos dictionary
                    Vector3 ballPosition = new Vector3(xPos, yPos, zPos);
                    ballPosDict.Add(frame, ballPosition);
                }
            }
        }

        reader.Close();
    }
}