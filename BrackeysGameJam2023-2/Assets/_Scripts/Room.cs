using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool isPresetRoom;
    public bool activateOnStart;
    public TextMeshPro roomNumber;
    public GameObject frontWallFacade;
    public float roomRevealFadeDuration;
    public AnimationCurve roomRevealFadeCurve;
    public bool isActive;
    public EnableFloor floor;

    [Header("Smashables")]
    public Transform smashablesHolder;
    public GameObject[] smashablePrefabs;
    public float numberOfSmashablesToSpawn;
    public List<Smashable> smashables = new List<Smashable>();
    public Vector2 xConstraints;
    public Vector2 zConstraints;
    public float minDistanceBetweenSmashables;

    [Header("Hostiles")]
    public Transform hostileHolder;
    public GameObject[] hostilePrefabs;
    public float numberOfHostilesToSpawn;
    public List<GameObject> hostiles = new List<GameObject>();

    void Start()
    {
        if(activateOnStart) ActivateRoom();
    }
    public void PopulateRoom(int roomNum)
    {
        roomNumber.text = roomNum.ToString();

        //Dont populate Preset Rooms
        if (isPresetRoom) return;

        if(smashablePrefabs.Length > 0)
        {
            // Define the grid cell size based on the minimum distance
            float gridCellSize = minDistanceBetweenSmashables;

            // Calculate the number of cells in each dimension
            int gridCellsX = Mathf.CeilToInt((xConstraints.y - xConstraints.x) / gridCellSize);
            int gridCellsZ = Mathf.CeilToInt((zConstraints.y - zConstraints.x) / gridCellSize);

            // Create a 2D array to represent the grid cells
            bool[,] occupiedGridCells = new bool[gridCellsX, gridCellsZ];

            //Spawn Smashables
            for (int smashableNum = 0; smashableNum < numberOfSmashablesToSpawn; smashableNum++)
            {
                int randomSmashable = Random.Range(0, smashablePrefabs.Length);
                Smashable newSmashable = Instantiate(smashablePrefabs[randomSmashable], smashablesHolder).GetComponent<Smashable>();
                smashables.Add(newSmashable);

                int maxSpawnAttempts = 20;
                // Set position with minimum distance check using grid
                Vector3 pos = Vector3.zero;

                bool validPositionFound = false;
                int attempts = 0;

                while (!validPositionFound && attempts < maxSpawnAttempts)
                {
                    pos = new Vector3(Random.Range(xConstraints.x, xConstraints.y), 0.5f, Random.Range(zConstraints.x, zConstraints.y));

                    int cellX = Mathf.FloorToInt((pos.x - xConstraints.x) / gridCellSize);
                    int cellZ = Mathf.FloorToInt((pos.z - zConstraints.x) / gridCellSize);

                    // Check adjacent grid cells for Smashables
                    bool tooClose = false;
                    for (int x = Mathf.Max(0, cellX - 1); x <= Mathf.Min(gridCellsX - 1, cellX + 1); x++)
                    {
                        for (int z = Mathf.Max(0, cellZ - 1); z <= Mathf.Min(gridCellsZ - 1, cellZ + 1); z++)
                        {
                            if (occupiedGridCells[x, z])
                            {
                                tooClose = true;
                                break;
                            }
                        }
                        if (tooClose)
                            break;
                    }

                    if (!tooClose)
                    {
                        validPositionFound = true;
                        // Mark grid cells as occupied
                        occupiedGridCells[cellX, cellZ] = true;
                    }

                    attempts++;
                }

                if (!validPositionFound)
                {
                    //Debug.LogWarning("Unable to find a valid position after " + maxSpawnAttempts + " attempts.");
                    break; // Exit the spawning loop
                }

                newSmashable.transform.localPosition = pos;
            }
        }
        

        if(hostilePrefabs.Length > 0)
        {
            //Spawn Hostiles
            for (int hostileNum = 0; hostileNum < numberOfHostilesToSpawn; hostileNum++)
            {
                int randomHostile = Random.Range(0, hostilePrefabs.Length);
                GameObject newHostile = Instantiate(hostilePrefabs[randomHostile], hostileHolder);
                hostiles.Add(newHostile);
            }
        }
        

    }

    private void Update()
    {
        if(isActive)
        {
            if(frontWallFacade)
                if(frontWallFacade.activeSelf) ActivateRoom(true);
        }
        
    }
    public void ActivateRoom(bool forceActivate = false)
    {
        if(!forceActivate)
            if (isActive) return;
        floor.EnableFloorRenderers();
        isActive = true;
        if(frontWallFacade)
            StartCoroutine(RevealRoom());
    }

    public void DeactivateRoom()
    {
        if (!isActive) return;
        isActive = false;
        floor.DisableFloorRenderers();
    }


    IEnumerator RevealRoom()
    {
        float timeDuration = 0;
        MeshRenderer renderer = frontWallFacade.GetComponent<MeshRenderer>();
        while (timeDuration < roomRevealFadeDuration)
        {
            timeDuration += Time.deltaTime;
            Material mat = renderer.material;
            float alpha = 1 - roomRevealFadeCurve.Evaluate(timeDuration / roomRevealFadeDuration);
            Color colour = mat.color;
            colour.a = alpha;
            mat.SetColor("_BaseColor", colour);
            renderer.material = mat;
            yield return null;
        }
        frontWallFacade.SetActive(false);
        yield return null;

    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            ActivateRoom();
        }
    }

}
