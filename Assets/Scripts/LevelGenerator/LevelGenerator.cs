using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using AssemblyCSharp;

public class LevelGenerator : NetworkBehaviour
{
    public Text MessagesTextForDebug;

    public int LEVEL_WIDTH = 50;
    public int LEVEL_HEIGHT = 50;
    public int BLOCKS_SIZE = 3;
    private int MAX_SHAPE_LENGTH = 3;

    public GameObject Portal;
    public GameObject Cube;
    public GameObject DestroyedCube;
    public int NumberOfPortals = 4;
    public int NumberOfWalls = 40;
    private float _portalMaxDistance = 30f;
    private float _portalMinDistance = 20f;

    private Color[,] _blocksMap;

    [SyncVar]
    private int _randomSharedSeed;

    public override void OnStartServer()
    {
        // generate random seed on server
        _randomSharedSeed = Random.Range(0, int.MaxValue);

        //MessagesTextForDebug.text += "OnStartServer " + _randomSharedSeed.ToString() + "\n";
    }

    public override void OnStartClient()
    {
        //MessagesTextForDebug.text += "OnStartClient " + _randomSharedSeed.ToString() + "\n";

        // same seed for all = same level generated
        Random.InitState(_randomSharedSeed); 

        //GeneratePortals();
        GenerateFence();
        GenerateWalls();
        ClearSpawnArea();
        InstantiateBlocks();
    }

    void Awake()
    {

    }

    void Start ()
    {
        
    }
	
	void Update ()
    {
		
	}

    void GeneratePortals()
    {
        float angleRange = Mathf.PI / NumberOfPortals;
        for (int i = 0; i < NumberOfPortals; ++i)
        {
            float distance = Random.Range(_portalMinDistance, _portalMaxDistance);
            float angle = Random.Range(0, angleRange*100) / 100f;
            float x = distance * Mathf.Cos(angleRange * i + angle);
            float y = distance * Mathf.Sin(angleRange * i + angle);
            Vector3 portalPosition = new Vector3(x, y, 0);
            GameObject go = (GameObject)Instantiate (Portal, portalPosition, Quaternion.identity);
            go.transform.parent = this.transform;
        }

    }

    void GenerateWalls()
    {
        float shapesInARow = Mathf.Sqrt(NumberOfWalls);
        int shapeLength = (int)((float)LEVEL_WIDTH / (float)shapesInARow);

        for (int i = 0; i < shapesInARow; ++i)
        {
            for (int j = 0; j < shapesInARow; ++j)
            {
                if (Random.Range(0, 100) > 70)
                    continue;   //30% not to instantiate the wall

                Shape shape = GetRandomShape();
                Tuple<int,int>[][] cubes = shape.GetShape();
                int x = Random.Range(0, 10);
                int y = Random.Range(0, 10);

                //temp. decide first what you wanna do
                Color[] colorsList = {Color.red, Color.blue, Color.yellow};
                int colorID = Random.Range(0, 3);

                foreach (Tuple<int,int> pair in cubes[Random.Range(0, cubes.Length)])
                {
                    if (i * shapeLength + x + pair.First >= LEVEL_WIDTH ||
                        j * shapeLength + y + pair.Second >= LEVEL_HEIGHT)
                        //avoid out of bounds. could be more accurate.
                        continue;

                    if (_blocksMap[i * shapeLength + x + pair.First, j * shapeLength + y + pair.Second] == Color.clear)
                    {
                        _blocksMap[i * shapeLength + x + pair.First, j * shapeLength + y + pair.Second] = colorsList[colorID];
                    }
                    else
                    {
                        _blocksMap[i * shapeLength + x + pair.First, j * shapeLength + y + pair.Second] = Color.black;
                    }
                }

            }
        }
    }

    Color clearColor()
    {
        //return Color.white;
        return Color.clear;
    }

    void ClearSpawnArea()
    {
        //clean spawn area
        for (int i = 3; i < LEVEL_HEIGHT; ++i)
        {
            _blocksMap[3, i] = clearColor();
            _blocksMap[4, i] = clearColor();

            _blocksMap[LEVEL_WIDTH - 1, i] = clearColor();
            _blocksMap[LEVEL_WIDTH - 2, i] = clearColor();
        }
        for (int i = 3; i < LEVEL_WIDTH; ++i)
        {
            _blocksMap[i, 3] = clearColor();
            _blocksMap[i, 4] = clearColor();

            _blocksMap[i, LEVEL_HEIGHT - 1] = clearColor();
            _blocksMap[i, LEVEL_HEIGHT - 2] = clearColor();
        }
    }
        
    Shape GetRandomShape()
    {
        //rewrite
        int shapeId = Random.Range(0, 4);
        switch (shapeId)
        {
            case 0:
                return new Shape1();
            case 1:
                return new Shape2();
            case 2:
                return new Shape3();
            case 3:
                return new Shape4();
            default:
                return new Shape1();
        }
    }

    bool isBlue = true;
    void AddFenceShapeToGrid(ref Tuple<int,int> nextBlockPosition, bool isWidthWall)
    {
        if (nextBlockPosition.First < 0 && nextBlockPosition.Second < 0)
            throw new System.Exception();  //how do exceptions go
        Shape sh = GetRandomShape();   //use it in the future in the function;
        int rand = UnityEngine.Random.Range(0, sh.GetShape().Length);
        Tuple<int,int> newNBP = new Tuple<int,int>(nextBlockPosition);
        isBlue = !isBlue;
        foreach (Tuple<int,int> tup in sh.GetShape()[rand])
        {
            if (isBlue)
                _blocksMap[tup.First + nextBlockPosition.First, tup.Second + nextBlockPosition.Second] = Color.blue;
            else
                _blocksMap[tup.First + nextBlockPosition.First, tup.Second + nextBlockPosition.Second] = Color.red;
            if (isWidthWall && tup.First + nextBlockPosition.First >= newNBP.First)
            {
                newNBP.First = tup.First + nextBlockPosition.First + 1;
            }
            if (!isWidthWall && tup.Second + nextBlockPosition.Second >= newNBP.Second)
            {
                newNBP.Second = tup.Second + nextBlockPosition.Second + 1;
            }
        }

        if (isWidthWall)
        {
            nextBlockPosition.First = newNBP.First;
        }
        else
        {
            nextBlockPosition.Second = newNBP.Second;
        }
    }

    void GenerateFence()
    {
        Tuple<int,int> nextBlockPosition = new Tuple<int,int>(0,0);
        _blocksMap = new Color[LEVEL_HEIGHT + MAX_SHAPE_LENGTH * 3 ,LEVEL_WIDTH + MAX_SHAPE_LENGTH * 3] ;

        for (int i = 0; i < LEVEL_HEIGHT + MAX_SHAPE_LENGTH * 3; ++i)
            for (int j = 0; j < LEVEL_WIDTH + MAX_SHAPE_LENGTH * 3; ++j)
            {
                _blocksMap[i, j] = new Color();
                _blocksMap[i, j] = Color.clear;
            }

        while (_blocksMap[LEVEL_WIDTH - 1, 0] == Color.clear &&
            _blocksMap[LEVEL_WIDTH - 1, 1] == Color.clear &&
            _blocksMap[LEVEL_WIDTH - 1, 2] == Color.clear)
        {
            AddFenceShapeToGrid(ref nextBlockPosition, true);
        }

        nextBlockPosition = new Tuple<int,int>(0, 0);
        while (_blocksMap[0, LEVEL_HEIGHT - 1] == Color.clear &&
            _blocksMap[1, LEVEL_HEIGHT - 1] == Color.clear &&
            _blocksMap[2, LEVEL_HEIGHT - 1] == Color.clear)
        {
            AddFenceShapeToGrid(ref nextBlockPosition, false);
        }

        nextBlockPosition = new Tuple<int,int>(0, LEVEL_HEIGHT);
        while (_blocksMap[LEVEL_WIDTH, LEVEL_HEIGHT] == Color.clear &&
            _blocksMap[LEVEL_WIDTH, LEVEL_HEIGHT + 1] == Color.clear &&
            _blocksMap[LEVEL_WIDTH, LEVEL_HEIGHT + 2] == Color.clear)
        {
            AddFenceShapeToGrid(ref nextBlockPosition, true);
        }

        nextBlockPosition = new Tuple<int,int>(LEVEL_WIDTH, 0);
        while (_blocksMap[LEVEL_WIDTH, LEVEL_HEIGHT - 1] == Color.clear &&
            _blocksMap[LEVEL_WIDTH + 1, LEVEL_HEIGHT - 1] == Color.clear &&
            _blocksMap[LEVEL_WIDTH + 2, LEVEL_HEIGHT - 1] == Color.clear)
        {
            AddFenceShapeToGrid(ref nextBlockPosition, false);
        }






/*
 * Temporary for debuging. creats a file in which i can see the fence is mapped well
*/
        //temp. creates a temp file with the cubes mapping.
        System.IO.File.Create(@"c:\temp\temp.txt");
        string fileMap = "";
        for (int i = 0; i < LEVEL_HEIGHT + MAX_SHAPE_LENGTH * 2; ++i)
        {
            for (int j = 0; j < LEVEL_WIDTH + MAX_SHAPE_LENGTH * 2; ++j)
            {
                if (_blocksMap[i, j] == Color.clear)
                    fileMap += "X";
                else
                    fileMap += "V";
            }
            fileMap += "balon";
        }
/* not sure what's the problem with the following line. but it's irrelevant either way
        System.IO.File.WriteAllText(@"c:\temp\temp.txt", fileMap);
*/
        
    }

    void InstantiateCube(ref GameObject go, Vector3 cubePosition, Color cubeColor)
    {
        GameObject igo = (GameObject)Instantiate(go, cubePosition * BLOCKS_SIZE, Quaternion.identity);
        SpriteRenderer sr = igo.GetComponent<SpriteRenderer>();
        sr.color = cubeColor;
        sr.transform.parent = this.transform;        
    }
    void InstantiateBlocks()
    {
        if (_blocksMap == null)
            throw new System.Exception();   //how to handle exceptions?

        for (int i = 0; i < LEVEL_HEIGHT + MAX_SHAPE_LENGTH * 2; ++i)
        {
            for (int j = 0; j < LEVEL_WIDTH + MAX_SHAPE_LENGTH * 2; ++j)
            {
                if (_blocksMap[i, j] == Color.clear)
                    continue;

                Vector3 cubePosition = new Vector3(i - MAX_SHAPE_LENGTH - LEVEL_WIDTH / 2, j - MAX_SHAPE_LENGTH - LEVEL_HEIGHT / 2, 0);
                if (_blocksMap[i, j] != Color.black)
                {
                    InstantiateCube(ref Cube, cubePosition, _blocksMap[i, j]);
                }
                else
                {
                    InstantiateCube(ref DestroyedCube, cubePosition, _blocksMap[i, j]);
                }
            }
        }
    }
}
