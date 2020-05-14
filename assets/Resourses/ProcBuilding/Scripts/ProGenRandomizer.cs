using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ProGen))]
public class ProGenRandomizer : MonoBehaviour
{
    [SerializeField]
    [Range(0, 20)]
    private float randomizeInSeconds = 5.0f;

    [SerializeField]
    [Range(1, 20)]
    private int minRows = 1;

    [SerializeField]
    [Range(1, 20)]
    private int maxRows = 1;

    [SerializeField]
    [Range(1, 20)]
    private int minColumns = 1;

    [SerializeField]
    [Range(1, 20)]
    private int maxColumns = 1;

    [SerializeField]
    [Range(1, 20)]
    private int minFloors = 1;

    [SerializeField]
    [Range(1, 20)]
    private int maxFloors = 1;

    [SerializeField]
    [Range(0, 20)]
    private int minCellUnitSize = 1;

    [SerializeField]
    [Range(0, 20)]
    private int maxCellUnitSize = 1;

    [SerializeField]
    private bool randomizeRoofInclusion = false;

    private float randomizeTimer = 0;

    private ProGen proGen;

    private void Awake()
    {
        proGen = GetComponent<ProGen>();
    }

    private void Update()
    {
        if (randomizeTimer >= randomizeInSeconds)
        {
            proGen.rows = Random.Range(minRows, maxRows);
            proGen.columns = Random.Range(minColumns, maxColumns);
            proGen.numberOfFloors = Random.Range(minFloors, maxFloors);
            proGen.cellUnitSize = Random.Range(minCellUnitSize, maxCellUnitSize);
            if (randomizeRoofInclusion)
            {
                proGen.includeRoof = !proGen.includeRoof;
            }
            StaticValue.RefreshWallColor();
            proGen.Generate();
            randomizeTimer = 0;
        }

        randomizeTimer += Time.deltaTime * 1.0f;
    }
}
