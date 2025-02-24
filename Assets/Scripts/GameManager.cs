using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("Game manager is NULL");
            return _instance;
        }
    }

    public GameObject coursesParent;
    private Course[] courses;

    private int[] playerIndex =
    {
        0, 0, 0, 0, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4
    };
    private int currentIndex = 0;

    private bool isBlinking = false;
    private float previousBlinkTime;
    private float blinkDuration = 0.1f;
    private int blinkIndex = 0;
    private AudioSource bgm;
    private AudioSource bannedSFX;
    private AudioSource selectedSFX;


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        previousBlinkTime = Time.time;
        courses = coursesParent.GetComponentsInChildren<Course>();
        bgm = GetComponent<AudioSource>();
        bannedSFX = transform.Find("Banned").GetComponent<AudioSource>();
        selectedSFX = transform.Find("Selected").GetComponent<AudioSource>();

        StartBlink();
    }


    void Update()
    {
        if (isBlinking)
        {
            if (Time.time - previousBlinkTime > blinkDuration)
            {
                if (courses[blinkIndex].isSelected == -1)
                    courses[blinkIndex].RemoveMask();
                GetNextBlinkIndex();
                courses[blinkIndex].SetMask(playerIndex[currentIndex]);
                previousBlinkTime = Time.time;
            }
        }
    }

    private void GetNextBlinkIndex()
    {
        blinkIndex++;
        if (blinkIndex == courses.Length)
            blinkIndex = 0;
        while (courses[blinkIndex].isSelected != -1)
        {
            blinkIndex++;
            if (blinkIndex == courses.Length)
                blinkIndex = 0;
        }
    }

    public int GetCurrentPlayer()
    {
        int ret = playerIndex[currentIndex];
        currentIndex += 1;
        return ret;
    }

    public void StartBlink()
    {
        isBlinking = true;
        bgm.Play();
    }

    public void StopBlink()
    {
        isBlinking = false;
        if (courses[blinkIndex].isSelected == -1)
            courses[blinkIndex].RemoveMask();
        bgm.Stop();
    }

    public void PlayBannedSFX()
    {
        bannedSFX.Play();
    }

    public void PlaySelectedSFX()
    {
        selectedSFX.Play();
    }
}
