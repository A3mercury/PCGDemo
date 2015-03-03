using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour
{
    public Dictionary<WorldPos, Chunk> chunks = new Dictionary<WorldPos, Chunk>();

    Chunk[, ,] worldChunks;

    public WorldPos PlayerPos;
    public GameObject Player;
    public GameObject chunkPrefab;

    public bool genChunk;

    const int SIZE = 1014;
    const int HALF = SIZE / 2;
    const int YSTART = -1;
    const int YEND = 3;

    int newX = 0;
    int newY = 0;
    int newZ = 0;

    void Start()
    {
        worldChunks = new Chunk[SIZE, 4, SIZE];

        Player = GameObject.Find("Player");
        PlayerPos = GetPlayerPos();

        // triple for-loop to generate series of Chunks around the player's position
        for(int x = -(Chunk.chunkSize / 2); x < Chunk.chunkSize / 2; x++)
        {
            for(int z = -(Chunk.chunkSize / 2); z < Chunk.chunkSize / 2; z++)
            {
                for(int y = YSTART; y < YEND; y++)
                {
                    newX = (PlayerPos.x + x) * Chunk.chunkSize;
                    newY = (PlayerPos.y + y) * Chunk.chunkSize;
                    newZ = (PlayerPos.z + z) * Chunk.chunkSize;

                    worldChunks[HALF + x, y - YSTART, HALF + z] = CreateChunk(newX, newY, newZ);
                }
            }
        }


    }

    void Update()
    {
        WorldPos CurrPos = GetPlayerPos();

        if (CurrPos.z >= PlayerPos.z + (Chunk.chunkSize / 2))
            MoveZPlus();
        else if (CurrPos.z < PlayerPos.z - (Chunk.chunkSize / 2))
            MoveZMinus();
        else if (CurrPos.x >= PlayerPos.x + (Chunk.chunkSize / 2))
            MoveXPlus();
        else if (CurrPos.x < PlayerPos.x - (Chunk.chunkSize / 2))
            MoveXMinus();
    }

    WorldPos GetPlayerPos()
    {
        // returns player position wherever in the game
        return new WorldPos((int)Player.transform.position.x,
                            (int)Player.transform.position.y,
                            (int)Player.transform.position.z);
    }

    void MoveZPlus()
    {
        PlayerPos = GetPlayerPos();

        for (int x = -(Chunk.chunkSize / 2); x < Chunk.chunkSize / 2; x++)
        {
            for (int y = YSTART; y < YEND; y++)
            {
                newX = PlayerPos.x + (x * Chunk.chunkSize);
                newY = y * Chunk.chunkSize;
                newZ = PlayerPos.z + Chunk.chunkSize * (Chunk.chunkSize / 2) - Chunk.chunkSize;

                worldChunks[HALF + x, y - YSTART, HALF] = CreateChunk(newX, newY, newZ);

                //Debug.Log("newZ: " + newZ);
                //if (newZ == 120)
                //{
                //    DestroyChunk(newX, newY, newZ);
                //}
            }
        }
    }

    void MoveZMinus()
    {
        PlayerPos = GetPlayerPos();

        Debug.Log("MoveZMinus() PlayerPos(" + PlayerPos.x + ", " + PlayerPos.y + ", " + PlayerPos.z + ")");

        //for (int x = -(setWidth / 2); x < setWidth / 2; x++)
        //{
        for (int x = -(Chunk.chunkSize / 2); x < Chunk.chunkSize / 2; x++)
        {
            for (int y = YSTART; y < YEND; y++)
            {
                newX = PlayerPos.x + (x * Chunk.chunkSize);
                newY = y * Chunk.chunkSize;
                newZ = PlayerPos.z - Chunk.chunkSize * (Chunk.chunkSize / 2) + Chunk.chunkSize;

                worldChunks[HALF + x, y - YSTART, HALF] = CreateChunk(newX, newY, newZ);
                //Debug.Log("x: " + x + " y: " + y + " worldChunks[" + (mid + x) + ", " + (y - yCoordStart) + ", " + ")");

            }
        }
    }

    void MoveXPlus()
    {
        PlayerPos = GetPlayerPos();

        Debug.Log("MoveXPlus() PlayerPos(" + PlayerPos.x + ", " + PlayerPos.y + ", " + PlayerPos.z + ")");

        //for (int z = -(setWidth / 2); z < setWidth / 2; z++)
        //{
        for (int z = -(Chunk.chunkSize / 2); z < Chunk.chunkSize / 2; z++)
        {
            for (int y = YSTART; y < YEND; y++)
            {
                newX = PlayerPos.x + Chunk.chunkSize * (Chunk.chunkSize / 2) - Chunk.chunkSize;
                newY = y * Chunk.chunkSize;
                newZ = PlayerPos.z + (z * Chunk.chunkSize);

                worldChunks[HALF, y - YSTART, HALF + z] = CreateChunk(newX, newY, newZ);
                //Debug.Log("z: " + z + " y: " + y + " worldChunks[" + (mid + z) + ", " + (y - yCoordStart) + ", " + ")");

            }
        }
    }

    void MoveXMinus()
    {
        PlayerPos = GetPlayerPos();

        Debug.Log("MoveXMinus() PlayerPos(" + PlayerPos.x + ", " + PlayerPos.y + ", " + PlayerPos.z + ")");

        //for (int z = -(setWidth / 2); z < setWidth / 2; z++)
        //{
        for (int z = -(Chunk.chunkSize / 2); z < Chunk.chunkSize / 2; z++)
        {
            for (int y = YSTART; y < YEND; y++)
            {
                newX = PlayerPos.x - Chunk.chunkSize * (Chunk.chunkSize / 2) + Chunk.chunkSize;
                newY = y * Chunk.chunkSize;
                newZ = PlayerPos.z + (z * Chunk.chunkSize);

                worldChunks[HALF, y - YSTART, HALF + z] = CreateChunk(newX, newY, newZ);
            }
        }
    }

    WorldPos ConvertCameraPosition()
    {
        Vector3 camPos = Camera.main.transform.position;
        return new WorldPos((int)Mathf.Round(camPos.x / Chunk.chunkSize) * Chunk.chunkSize,
                            (int)Mathf.Round(camPos.y / Chunk.chunkSize) * Chunk.chunkSize,
                            (int)Mathf.Round(camPos.z / Chunk.chunkSize) * Chunk.chunkSize);
    }

    WorldPos GetPotentialStartPosition()
    {
        return GetPlayerPos();
    }

    public Chunk CreateChunk(int x, int y, int z)
    {
        // assign a new world position with the values passed
        WorldPos worldPos = new WorldPos(x, y, z);

        // Instantiate the chunk at the coordinates using the chunk prefab
        GameObject newChunkObject = Instantiate(chunkPrefab, new Vector3(x, y, z), Quaternion.Euler(Vector3.zero)) as GameObject;

        // assigns the instantiated chunk to newChunk
        Chunk newChunk = newChunkObject.GetComponent<Chunk>();

        newChunk.pos = worldPos;
        newChunk.world = this;

        // Add it to the chunks dictionary with the position as the key
        chunks.Add(worldPos, newChunk);

        var terrainGen = new TerrainGen();
        newChunk = terrainGen.ChunkGen(newChunk);

        newChunk.SetBlocksUnmodified();

        return newChunk;
    }

    public void DestroyChunk(int x, int y, int z)
    {
        Chunk chunk = null;
        if(chunks.TryGetValue(new WorldPos(x, y, z), out chunk))
        {
            UnityEngine.Object.Destroy(chunk.gameObject);
            chunks.Remove(new WorldPos(x, y, z));
        }
    }

    public Chunk GetChunk(int x, int y, int z)
    {
        WorldPos pos = new WorldPos();
        float multiple = Chunk.chunkSize;
        pos.x = Mathf.FloorToInt(x / multiple) * Chunk.chunkSize;
        pos.y = Mathf.FloorToInt(y / multiple) * Chunk.chunkSize;
        pos.z = Mathf.FloorToInt(z / multiple) * Chunk.chunkSize;

        Chunk containerChunk = null;

        chunks.TryGetValue(pos, out containerChunk);

        return containerChunk;
    }

    public Block GetBlock(int x, int y, int z)
    {
        Chunk containerChunk = GetChunk(x, y, z);

        if(containerChunk != null)
        {
            Block block = containerChunk.GetBlock(
                x - containerChunk.pos.x,
                y - containerChunk.pos.y,
                z - containerChunk.pos.z);

            return block;
        }
        else
        {
            return new BlockAir();
        }
    }

    public void SetBlock(int x, int y, int z, Block block)
    {
        Chunk chunk = GetChunk(x, y, z);

        if (chunk != null)
        {
            chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, block);
            chunk.update = true;

            UpdateIfEqual(x - chunk.pos.x, 0, new WorldPos(x - 1, y, z));
            UpdateIfEqual(x - chunk.pos.x, Chunk.chunkSize - 1, new WorldPos(x + 1, y, z));
            UpdateIfEqual(y - chunk.pos.y, 0, new WorldPos(x, y - 1, z));
            UpdateIfEqual(y - chunk.pos.y, Chunk.chunkSize - 1, new WorldPos(x, y + 1, z));
            UpdateIfEqual(z - chunk.pos.z, 0, new WorldPos(x, y, z - 1));
            UpdateIfEqual(z - chunk.pos.z, Chunk.chunkSize - 1, new WorldPos(x, y, z + 1));
        }
    }

    void UpdateIfEqual(int value1, int value2, WorldPos pos)
    {
        if(value1 == value2)
        {
            Chunk chunk = GetChunk(pos.x, pos.y, pos.z);
            if (chunk != null)
                chunk.update = true;
        }
    }
}