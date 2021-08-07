using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMap : MonoBehaviour
{
    public GameObject Wall;
    public GameObject Brick;
    public GameObject Ground;

    private GameObject blocks;
    private GameObject floor;

    public void Awake()
    {
        blocks = transform.GetChild(0).gameObject;
        floor = transform.GetChild(1).gameObject;
        LoadGame();
    }

    private void LoadGame()
    {
        for(int i = 0; i < 13; i++)
        {
            for(int j = 0; j < 13; j++)
            {
                var obj = Instantiate(Ground, new Vector3(i, -0.5f, j), Quaternion.identity);
                obj.transform.SetParent(floor.transform);
            }
        }

        for(int k = 0; k < 13; k++)
        {
            var pos = new Vector3(0, .5f, k);
            var obj = Instantiate(Wall, pos, Quaternion.identity);
            obj.transform.SetParent(blocks.transform);

            pos = new Vector3(12, .5f, k);
            obj = Instantiate(Wall, pos, Quaternion.identity);
            obj.transform.SetParent(blocks.transform);

            pos = new Vector3(k, .5f, 0);
            obj = Instantiate(Wall, pos, Quaternion.identity);
            obj.transform.SetParent(blocks.transform);

            pos = new Vector3(k, .5f, 12);
            obj = Instantiate(Wall, pos, Quaternion.identity);
            obj.transform.SetParent(blocks.transform);
        }

        for(int i = 2; i < 13; i += 2)
        {
            for(int j = 2; j < 13; j += 2)
            {
                var obj = Instantiate(Wall, new Vector3(i, 0.5f, j), Quaternion.identity);
                obj.transform.SetParent(blocks.transform);
            }
        }
        // create bricks...
        for(int i = 3; i < 10; i++)
        {
            var brick = Instantiate(Brick, new Vector3(i, .5f, 1f), Quaternion.identity);
            brick.transform.SetParent(blocks.transform);
        }
    }
}