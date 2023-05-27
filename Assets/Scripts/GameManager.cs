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

    private List<List<Cell>> _cellList;

    private bool _isAutoPlaying;
    [SerializeField] private TMP_Text playButtonText;
    [SerializeField] private Slider ticksPerSecondSlider;
    [SerializeField] private TMP_Text sliderText;

    private Coroutine _runningCoroutine;
    private float _timeDelay = 1f;

    [SerializeField] private TMP_InputField xResize;
    [SerializeField] private TMP_InputField yResize;

    public int _currentBoardSizeX = 15;
    public int _currentBoardSizeY = 15;

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
        CreateBoard(_currentBoardSizeX,_currentBoardSizeY);
    }

    private void CreateBoard(int x, int y)
    {
        _cellList = new List<List<Cell>>();
        for (var i = 0; i < y; i++)
        {
            _cellList.Add(new List<Cell>());
            for (var j = 0; j < x; j++)
            {
                var newLifeTile = Instantiate(lifeCubePrefab, new Vector3(i,0,j), lifeCubePrefab.transform.rotation);
                _cellList[i].Add(newLifeTile.GetComponent<Cell>());
            }
        }
        _currentBoardSizeX = x;
        _currentBoardSizeY = y;
    }
    
    //called by ui button
    public void NextGeneration()
    {
        foreach (var cell in _cellList.SelectMany(cellRow => cellRow))
        {
            cell.SetLivingNeighbors(_cellList);
        }
        foreach (var cell in _cellList.SelectMany(cellRow => cellRow))
        {
            cell.NewGeneration();
        }
    }

    //called by ui button
    public void ClearBoard()
    {
        foreach (var cell in _cellList.SelectMany(cellRow => cellRow))
        {
            cell.SetCellDead();
        }
    }
    
    //called by play button
    public void PlayButtonClick()
    {
        switch (_isAutoPlaying)
        {
            //pressed play
            case false:
                _isAutoPlaying = true;
                _runningCoroutine = StartCoroutine(AutoPlay());
                playButtonText.text = "Pause";
                break;
            //pressed pause
            case true:
                _isAutoPlaying = false;
                StopCoroutine(_runningCoroutine);
                playButtonText.text = "Play";
                break;
        }
    }
    
    //called by slider on value changed
    public void SetSliderText()
    {
        sliderText.text = ticksPerSecondSlider.value + " (Ticks/Second)";
        _timeDelay = ticksPerSecondSlider.value;
    }

    private IEnumerator AutoPlay()
    {
        while (_isAutoPlaying)
        {
            NextGeneration();
            yield return new WaitForSeconds(1 / _timeDelay);
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

        foreach (var cell in _cellList.SelectMany(cellRow => cellRow))
        {
            Destroy(cell.gameObject);
        }
        CreateBoard(xSizeInt,ySizeInt);
    }
    
    
}
