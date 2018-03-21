using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Popup : MonoBehaviour {
    Transform image;
    Transform audio;
    int[] latinSquare = { 1, 2, 3, 4,   2, 3, 4, 1,     3, 4, 1, 2,     4, 1, 2, 3 };
    List<int> order = new List<int>();
    List<int> newSquare = new List<int>();
    int index;
    string condition;
    string targetSide;
    string soundSide;
    float time;
    // Use this for initialization
    void Start () {
        image = transform.Find("Image");
        audio = transform.Find("Audio");
        
        for (int i = 0; i < 4; i++)
        {
            order.Add(i);
        }
        for (int i = 0; i < 4; i++)
        {
            int temp = order[i];
            int randomIndex = Random.Range(i, 4);
            order[i] = order[randomIndex];
            order[randomIndex] = temp;
        }

        for (int i = 0; i < 4; i++)
        {
            newSquare.Add(latinSquare[order[i] * 4]);
            print(newSquare[i * 4]);
            newSquare.Add(latinSquare[order[i] * 4 + 1]);
            newSquare.Add(latinSquare[order[i] * 4 + 2]);
            newSquare.Add(latinSquare[order[i]  *4 + 3]);
        }

        index = 0;
        print(newSquare);
        StartCoroutine(OpenImage());
        if (newSquare[index] == 3)
        {
            StartCoroutine(DistractorSound());
        }
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.Space))
        {
            if (image.gameObject.activeSelf == true)
            {
                float lum = 0.2126f * image.gameObject.GetComponent<Image>().color.r + 0.7152f * image.gameObject.GetComponent<Image>().color.g + 0.0722f * image.gameObject.GetComponent<Image>().color.b;
                WriteString(condition + "," + targetSide + "," + soundSide + "," + time + "," + Time.time + "," + (Time.time - time));
                image.gameObject.SetActive(false);
                StartCoroutine(OpenImage());
                if (newSquare[index] == 3)
                {
                    StartCoroutine(DistractorSound());
                }
            }
            
        }
	}

    IEnumerator CloseImage()
    {
        yield return new WaitForSeconds(1);
        if (image.gameObject.activeSelf == true)
        {
            float lum = 0.2126f * image.gameObject.GetComponent<Image>().color.r + 0.7152f * image.gameObject.GetComponent<Image>().color.g + 0.0722f * image.gameObject.GetComponent<Image>().color.b;
            WriteString(condition + "," + targetSide+ "," + soundSide + "," + time + "," + Time.time + "," + (Time.time - time));
            image.gameObject.SetActive(false);
            StartCoroutine(OpenImage());
            if (newSquare[index] == 3)
            {
                StartCoroutine(DistractorSound());
            }
        }
    }

    IEnumerator OpenImage()
    {
        if (index < 16)
        {
            float waitU = Random.Range(0, 0.999f);
            float waitTime = Mathf.Log(1 - waitU) / (-0.3f);
            yield return new WaitForSeconds(waitTime);
            int side = Random.Range(0, 2); //0 is left, 1 is right
            int aSide = Random.Range(0, 2);
            print(Screen.width / 2);
            if (image.gameObject.activeSelf == false)
            {
                image.gameObject.SetActive(true);
                if (side == 0)
                {
                    image.gameObject.transform.position = new Vector2(-4.0f, 0);
                }
                else
                {
                    image.gameObject.transform.position = new Vector2(4.0f, 0);
                }

                if (newSquare[index] == 1)
                {
                    condition = "none";
                    if (side == 0)
                        targetSide = "L";
                    else
                        targetSide = "R";
                    soundSide = "none";
                }
                else if (newSquare[index] == 2)
                {
                    condition = "non_dir";
                    if (side == 0)
                        targetSide = "L";
                    else
                        targetSide = "R";
                    soundSide = "none";
                    audio.GetComponent<AudioSource>().panStereo = 0;
                    audio.GetComponent<AudioSource>().Play();
                }
                else if (newSquare[index] == 3)
                {
                    condition = "dir";
                    if (side == 0)
                        targetSide = "L";
                    else
                        targetSide = "R";
                    if (aSide == 0)
                    {
                        audio.GetComponent<AudioSource>().panStereo = -0.7f;
                        audio.GetComponent<AudioSource>().Play();
                        soundSide = "L";
                    }
                    else
                    {
                        audio.GetComponent<AudioSource>().panStereo = 0.7f;
                        audio.GetComponent<AudioSource>().Play();
                        soundSide = "R";
                    }
                }
                else
                {
                    condition = "distractor";
                    if (side == 0)
                        targetSide = "L";
                    else
                        targetSide = "R";
                }
                time = Time.time;
                index++;
            }
        }
        else
        {
            print("Done");
        }
    }

    IEnumerator DistractorSound()
    {
        if (index < 16)
        {
            float waitU = Random.Range(0, 0.999f);
            float waitTime = Mathf.Log(1 - waitU) / (-0.7f);
            yield return new WaitForSeconds(waitTime);
            int aSide = Random.Range(0, 2);
            if (aSide == 0)
            {
                audio.GetComponent<AudioSource>().panStereo = -0.7f;
                audio.GetComponent<AudioSource>().Play();
                soundSide = "L";
            }
            else
            {
                audio.GetComponent<AudioSource>().panStereo = 0.3f;
                audio.GetComponent<AudioSource>().Play();
                soundSide = "R";
            }
        }
    }

    static void WriteString(string text)
    {
        string path = "Assets/Resources/Test.txt";

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(text + "\n");
        writer.Close();
    }
}
