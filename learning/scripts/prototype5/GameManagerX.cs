﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine.SocialPlatforms.Impl;

public class GameManagerX : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI leftTimeText;
    public TextMeshProUGUI gameOverText;
    public GameObject titleScreen;
    public Button restartButton; 

    public List<GameObject> targetPrefabs;

    private int score;
    private int leftTime;
    private int timeLimit = 10;

    private float spawnRate = 1.5f;
    public bool isGameActive;

    private float spaceBetweenSquares = 2.5f; 
    private float minValueX = -3.75f; //  x value of the center of the left-most square
    private float minValueY = -3.75f; //  y value of the center of the bottom-most square
    
    // Start the game, remove title screen, reset score, and adjust spawnRate based on difficulty button clicked
    public void StartGame(int difficulty)
    {
        spawnRate /= difficulty;
        isGameActive = true;
        score = 0;
        leftTime = timeLimit;
        UpdateScore(0);
        UpdateLeftTime();
        titleScreen.SetActive(false);
        StartCoroutine(SpawnTarget());
        StartCoroutine(LeftTimeTimer());
    }

    // While game is active spawn a random target
    IEnumerator SpawnTarget()
    {
        int targetPrefabsLength = targetPrefabs.Count;
        while (isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            int index = UnityEngine.Random.Range(0, targetPrefabsLength);

            if (isGameActive)
            {
                Instantiate(targetPrefabs[index], RandomSpawnPosition(), targetPrefabs[index].transform.rotation);
            }
            
        }
    }
    IEnumerator LeftTimeTimer()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(1);
            UpdateLeftTime();
        }
    }

    // Generate a random spawn position based on a random index from 0 to 3
    Vector3 RandomSpawnPosition()
    {
        float spawnPosX = minValueX + (RandomSquareIndex() * spaceBetweenSquares);
        float spawnPosY = minValueY + (RandomSquareIndex() * spaceBetweenSquares);

        Vector3 spawnPosition = new Vector3(spawnPosX, spawnPosY, 0);
        return spawnPosition;

    }

    // Generates random square index from 0 to 3, which determines which square the target will appear in
    int RandomSquareIndex()
    {
        return UnityEngine.Random.Range(0, 4);
    }

    // Update score with value from target clicked
    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }
    public void UpdateLeftTime()
    {
        if(leftTime<=0)
        { 
            GameOver();
        }
        leftTime -= 1;
        leftTimeText.text = "Left time: " + leftTime;
    }

    // Stop game, bring up game over text and restart button
    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        isGameActive = false;
    }

    // Restart game by reloading the scene
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}