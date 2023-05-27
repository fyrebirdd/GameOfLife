using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] private GameObject lifeCubePrefab;

    private List<List<Cell>> cellList;

    private bool isAutoPlaying;
    [SerializeField] private TMP_Text playButtonText;
    [SerializeField] private Slider ticksPerSecondSlider;
    [SerializeField] private TMP_Text sliderText;

    private Coroutine runningCoroutine;
    private float timeDelay = 1f;

    [SerializeField] private TMP_InputField xResize;
    [SerializeField] private TMP_InputField yResize;

    public int currentBoardSizeX = 15;
    public int currentBoardSizeY = 15;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        CreateBoard(currentBoardSizeX,currentBoardSizeY);
    }

    private void CreateBoard(int x, int y)
    {
        cellList = new List<List<Cell>>();
        for (var i = 0; i < y; i++)
        {
            cellList.Add(new List<Cell>());
            for (var j = 0; j < x; j++)
            {
                var newLifeTile = Instantiate(lifeCubePrefab, new Vector3(i,0,j), lifeCubePrefab.transform.rotation);
                cellList[i].Add(newLifeTile.GetComponent<Cell>());
            }
        }
        currentBoardSizeX = x;
        currentBoardSizeY = y;
    }
    
    //called by ui button
    public void NextGeneration()
    {
        foreach (var cell in cellList.SelectMany(cellRow => cellRow))
        {
            cell.SetLivingNeighbors(cellList);
        }
        foreach (var cell in cellList.SelectMany(cellRow => cellRow))
        {
            cell.NewGeneration();
        }
    }

    //called by ui button
    public void ClearBoard()
    {
        foreach (var cell in cellList.SelectMany(cellRow => cellRow))
        {
            cell.SetCellDead();
        }
    }
    
    //called by play button
    public void PlayButtonClick()
    {
        switch (isAutoPlaying)
        {
            //pressed play
            case false:
                isAutoPlaying = true;
                runningCoroutine = StartCoroutine(AutoPlay());
                playButtonText.text = "Pause";
                break;
            //pressed pause
            case true:
                isAutoPlaying = false;
                StopCoroutine(runningCoroutine);
                playButtonText.text = "Play";
                break;
        }
    }
    
    //called by slider on value changed
    public void SetSliderText()
    {
        sliderText.text = ticksPerSecondSlider.value + " (Ticks/Second)";
        timeDelay = ticksPerSecondSlider.value;
    }

    private IEnumerator AutoPlay()
    {
        while (isAutoPlaying)
        {
            NextGeneration();
            yield return new WaitForSeconds(1 / timeDelay);
        }
    }

    public void ResizeBoard()
    {
        int xSizeInt;
        int ySizeInt;
        try
        {
            xSizeInt = int.Parse(yResize.text);
            ySizeInt = int.Parse(xResize.text);
        }
        catch (FormatException)
        {
            Debug.LogError("Input was not a number, please only input numbers");
            return;
        }
        catch (OverflowException)
        {
            Debug.LogError("Input number was too large, try a smaller one");
            return;
        }
        catch (ArgumentNullException)
        {
            Debug.LogError("No input provided, please input a number");
            return;
        }

        foreach (var cell in cellList.SelectMany(cellRow => cellRow))
        {
            Destroy(cell.gameObject);
        }
        CreateBoard(xSizeInt,ySizeInt);
    }
    
    
}
