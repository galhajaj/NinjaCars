using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Board : MonoBehaviour 
{
	public int BoardSizeX;
	public int BoardSizeY;
	public Transform TilePrefab;

    public List<Transform> Tiles = new List<Transform>();
    // ======================================================================================================================================== //
    void Awake()
    {
        generateTiles();
    }
    // ======================================================================================================================================== //
    void Start() 
	{

	}
	// ======================================================================================================================================== //
	private void generateTiles()
	{
		float boardOriginX = this.transform.position.x;
		float boardOriginY = this.transform.position.y;
		float tileWidth = TilePrefab.GetComponent<SpriteRenderer> ().bounds.size.x;
		float tileHeight = TilePrefab.GetComponent<SpriteRenderer> ().bounds.size.y;
		
		for ( int x = 0; x < BoardSizeX; x++ ) 
		{
			for ( int y = 0; y < BoardSizeY; y++ ) 
			{
				Vector3 tilePosition = new Vector3(boardOriginX + (x * tileWidth), boardOriginY + (y * tileHeight), 0);
				Transform tile = (Transform)Instantiate(TilePrefab, tilePosition, Quaternion.identity);
				tile.name = "Tile" + x + "_" + y;
				tile.parent = this.transform;

				tile.GetComponent<Tile>().PosX = x;
				tile.GetComponent<Tile>().PosY = y;

                Tiles.Add(tile);
			}
		}
	}
	// ======================================================================================================================================== //
}
