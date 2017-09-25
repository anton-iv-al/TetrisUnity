using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TetrisGame;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public GameObject levelText;
    public GameObject linesText;

    public Texture2D squareRed;
    public Texture2D squareBlue;
    public Texture2D squareGreen;
    public Texture2D squareYellow;
    public Texture2D squareViolet;
    public Texture2D squareOrange;
    public Texture2D squareLightBlue;

    private int _gameLevel = 1;
    private int GameLevel
    {
        get => _gameLevel;
        set
        {
            _gameLevel = value;
            levelText.GetComponent<Text>().text = value.ToString();
        }
    }

    private int _linesCount = 0;
    private int LinesCount
    {
        get => _linesCount;
        set
        {
            _linesCount = value;
            linesText.GetComponent<Text>().text = value.ToString();
        }
    }

    private int _gameSpeed = 1;
    private bool _isSpeedUp = false;

    private int _figureCounter = 0;
    private int _nextLevelFigureCount = 10;

    private bool _isRun = true;

    private bool _isGameOver = false;
    private bool IsGameOver{
        get => _isGameOver;
        set{
            _isGameOver = value;
            if (value) DrawManager.Instance.ShowGameOver();
            else DrawManager.Instance.HideGameOver();
        }
    }

    private Field _field;
    private FigureTypes _figureTypes;
    private Figure _currentFigure;
    private Figure _nextFigure;

    private int _maxGameLevel = 10;
    private int _firstLevelTimeDelay = 1000;
    private int _lastLevelTimeDelay = 20;

    private float _currentTime1 = 0;
    private float UpdatePeriodTime1 => _firstLevelTimeDelay + (_lastLevelTimeDelay - _firstLevelTimeDelay) / (_maxGameLevel - 1) * (_gameSpeed - 1);
    private float _currentTime2 = 0;
    private float UpdatePeriodTime2 { get; } = 50;
    private float _currentTime3 = 0;
    private float UpdatePeriodTime3 { get; } = 100;

    // Use this for initialization
    void Start()
    {
        try {
            Screen.SetResolution(600, 700, false);

            DrawManager.Instance.LoadTextures(new List<Texture2D>(){ squareRed, squareBlue, squareGreen, squareYellow, squareViolet, squareOrange, squareLightBlue });
            DrawManager.Instance.Initialize();

            _field = new Field();
            _figureTypes = new FigureTypes();

            _nextFigure = new Figure(_figureTypes.GetRandomMatrix());
            CreateNewFigure();

            Restart();
        }
        catch (System.Exception e)
        {
            _isRun = false;
            ShowExceptionMessage(e);
        }
    }

    // Update is called once per frame
    void Update()
    {
        try{
            if (Input.GetKey(KeyCode.Escape)) Application.Quit();
            if (IsGameOver && Input.GetKey(KeyCode.Return)) Restart();
            if (!_isRun) return;
            if (Input.GetKey(KeyCode.Space)) SpeedUp(true);
            if (Input.GetKeyUp(KeyCode.Space)) SpeedUp(false);

            PeriodicUpdate3();
            PeriodicUpdate2();
            PeriodicUpdate1();
        }
        catch (System.Exception e)
        {
            _isRun = false;
            ShowExceptionMessage(e);
        }
    }

    private void ShowExceptionMessage(Exception e)
    {
//        UnityEditor.EditorUtility.DisplayDialog("Error",
//            e.GetType().Name + "\n\n" +
//            e.Message + "\n\n" +
//            e.StackTrace
//            , "OK");
    }

    private void CreateNewFigure()
    {
        _currentFigure = _nextFigure;
        _nextFigure = new Figure(_figureTypes.GetRandomMatrix());
        _figureCounter++;
        TryAddLevel();

        _nextFigure.RedrawInPreview();
    }

    private void TryAddLevel()
    {
        if (_figureCounter >= _nextLevelFigureCount && GameLevel < _maxGameLevel)
        {
            _figureCounter = 0;
            GameLevel += 1;
            if (!_isSpeedUp) _gameSpeed += 1;
        }
    }

    private void PeriodicUpdate1()
    {
        _currentTime1 += Time.deltaTime * 1000;
        if (_currentTime1 < UpdatePeriodTime1) return;
        else _currentTime1 = 0;

        _currentFigure.Y += 1;
        if (!_field.CheckFigureIntersection(_currentFigure))
        {
            _currentFigure.Y -= 1;
            _field.AddFigure(_currentFigure);
            LinesCount += _field.RemoveLines();
            if (_field.CheckGamoOver()) GameOver();
            else CreateNewFigure();

            _field.Redraw();
        }
        _currentFigure.Redraw();
    }

    private void PeriodicUpdate2()
    {
        _currentTime2 += Time.deltaTime * 1000;
        if (_currentTime2 < UpdatePeriodTime2) return;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _currentFigure.X -= 1;
            if (!_field.CheckFigureIntersection(_currentFigure)) {
                _currentFigure.X += 1;
            } else {
                _currentFigure.Redraw();
            }
            _currentTime2 = 0;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            _currentFigure.X += 1;
            if (!_field.CheckFigureIntersection(_currentFigure)) {
                _currentFigure.X -= 1;
            } else {
                _currentFigure.Redraw();
            }
            _currentTime2 = 0;
        }
    }

    private void PeriodicUpdate3()
    {
        _currentTime3 += Time.deltaTime * 1000;
        if (_currentTime3 < UpdatePeriodTime3) return;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            _currentFigure.RotateRight();
            if (!_field.CheckFigureIntersection(_currentFigure)) {
                _currentFigure.RotateLeft();
            } else {
                _currentFigure.Redraw();
            }
            _currentTime3 = 0;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            _currentFigure.RotateLeft();
            if (!_field.CheckFigureIntersection(_currentFigure)) {
                _currentFigure.RotateRight();
            } else {
                _currentFigure.Redraw();
            }
            _currentTime3 = 0;
        }
    }

    private void Restart()
    {
        _isRun = true;
        IsGameOver = false;
        _field.Clear();
        CreateNewFigure();
        GameLevel = 1;
        LinesCount = 0;
        _gameSpeed = 1;
        _figureCounter = 0;

        _field.Redraw();
        _currentFigure.Redraw();
        _nextFigure.RedrawInPreview();
    }

    private void GameOver()
    {
        _isRun = false;
        IsGameOver = true;
    }

    private void SpeedUp(bool isEnabled)
    {
        _isSpeedUp = isEnabled;
        if (isEnabled) _gameSpeed = _maxGameLevel;
        else _gameSpeed = GameLevel;
    }
}
