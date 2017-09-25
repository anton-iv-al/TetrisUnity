using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace TetrisGame
{
    class DrawManager
    {
        public static DrawManager Instance = new DrawManager();
        private DrawManager()
        {
        }

        private Texture2D _squareRed;
        private Texture2D _squareBlue;
        private Texture2D _squareGreen;
        private Texture2D _squareYellow;
        private Texture2D _squareViolet;
        private Texture2D _squareOrange;
        private Texture2D _squareLightBlue;

        private int _squareSizeX = 30;
        private int _squareSizeY = 30;

        private GameObject _gameOver;
        private GameObject _fieldObject;
        private GameObject _figureObject;
        private GameObject _previewObject;

        public int MatrixSizeX { get; set; }
        public int MatrixSizeY { get; set; }
        
        public void Initialize()
        {
            _gameOver = GameObject.Find("GameOver");
            _fieldObject = GameObject.Find("Field");
            _figureObject = GameObject.Find("Figure");
            _previewObject = GameObject.Find("Preview");
        }

        public void LoadTextures(List<Texture2D> textures)
        {
            _squareRed = textures[0];
            _squareBlue = textures[1];
            _squareGreen = textures[2];
            _squareYellow = textures[3];
            _squareOrange = textures[4];
            _squareViolet = textures[5];
            _squareLightBlue = textures[6];
        }

        public void ShowGameOver()
        {
            _gameOver.SetActive(true);
        }

        public void HideGameOver()
        {
            _gameOver.SetActive(false);
        }

        private Texture2D GetSquareTexture(SquareColor color)
        {
            Texture2D squareTexture;
            switch (color)
            {
                case SquareColor.Empty: return null;
                case SquareColor.Red: squareTexture = _squareRed; break;
                case SquareColor.Blue: squareTexture = _squareBlue; break;
                case SquareColor.Green: squareTexture = _squareGreen; break;
                case SquareColor.Yellow: squareTexture = _squareYellow; break;
                case SquareColor.Orange: squareTexture = _squareOrange; break;
                case SquareColor.Violet: squareTexture = _squareViolet; break;
                case SquareColor.LightBlue: squareTexture = _squareLightBlue; break;
                default: throw new Exception("Unknown SquareColor");
            }
            return squareTexture;
        }

        private void CreateSquare(GameObject parent,int x, int y, SquareColor color)    // координаты в матрице
        {
            Texture2D texture = GetSquareTexture(color);
            if (texture == null) return;

            var square = new GameObject();
            var image = square.AddComponent<Image>();
            image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.up);

            square.transform.SetParent(parent.transform);
            square.transform.localPosition = new Vector3(x * _squareSizeX, -y * _squareSizeY, 0);
            square.transform.localScale = new Vector2(1f, 1f);
            var rt = square.GetComponent<RectTransform>();
            rt.pivot = Vector2.up;
            rt.sizeDelta = new Vector2(_squareSizeX, _squareSizeY);
        }
        public void CreateFieldSquare(int x, int y, SquareColor color) {    // координаты в матрице
            y -= 4;
            if (x < 0 || x >= MatrixSizeX || y < 0 || y >= MatrixSizeY) return;
            CreateSquare(_fieldObject, x, y, color);
        }
        public void CreateFigureSquare(int x, int y, SquareColor color){    // координаты в матрице
            y -= 4;
            if (x < 0 || x >= MatrixSizeX || y < 0 || y >= MatrixSizeY) return;
            CreateSquare(_figureObject, x, y, color);
        }
        public void CreatePreviewSquare(int x, int y, SquareColor color) {  // координаты в матрице
            CreateSquare(_previewObject, x, y, color);
        }

        private void ClearChilds(GameObject parent) {
            foreach(Transform child in parent.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
        public void ClearField(){
            ClearChilds(_fieldObject);
        }
        public void ClearFifure()
        {
            ClearChilds(_figureObject);
        }
        public void ClearPreview()
        {
            ClearChilds(_previewObject);
        }
    }
}
