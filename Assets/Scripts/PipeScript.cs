using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeScript : MonoBehaviour
{
    float[] rotations = { 0f, 90f, 180f, 270f };

    public float[] correctRotation;      // 1 or 2 valid angles in degrees
    [SerializeField]
    bool isPlaced = false;

    int PossibleRots = 1;

    GameManager gameManager;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Start()
    {
        PossibleRots = correctRotation.Length;

        int rand = Random.Range(0, rotations.Length);
        transform.eulerAngles = new Vector3(0, 0, rotations[rand]);

        // Check initial state
        if (IsInCorrectRotation())
        {
            isPlaced = true;
            gameManager.correctMove();
        }
    }

    private void OnMouseDown()
    {
        // Rotate 90 degrees
        transform.Rotate(new Vector3(0, 0, 90));

        bool nowCorrect = IsInCorrectRotation();

        // If it just became correct and wasn't before
        if (nowCorrect && !isPlaced)
        {
            isPlaced = true;
            gameManager.correctMove();
        }
        // If it was correct before and now isn't
        else if (!nowCorrect && isPlaced)
        {
            isPlaced = false;
            gameManager.wrongMove();
        }
        // If it was incorrect and still incorrect, do nothing
    }

    private bool IsInCorrectRotation()
    {
        // Normalize current z to 0–360 and round
        float z = transform.eulerAngles.z % 360f;
        z = Mathf.Round(z);

        for (int i = 0; i < correctRotation.Length; i++)
        {
            float target = correctRotation[i] % 360f;
            target = Mathf.Round(target);

            if (Mathf.Approximately(z, target))
            {
                return true;
            }
        }

        return false;
    }
}
