using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    public float rotationSpeed = 5.0f;

    [SerializeField] private ScoreSystem scoreSystem = null;

    private void Start()
    {
        scoreSystem = FindObjectOfType<ScoreSystem>();
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            scoreSystem.crystalFound = true;
            other.GetComponent<PlayerCore>().GetHUD().spellEditingController.crystalsLeft++;
            other.gameObject.GetComponent<PlayerCore>().ToggleSpellEditingUI();
            Destroy(gameObject);
        }
    }
}
