using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    public float rotationSpeed = 5.0f;
    public GameObject audio; //audio

    [SerializeField] private ScoreSystem scoreSystem = null;
    [SerializeField] private Health heathSystem = null;

    private void Start()
    {
        scoreSystem = FindObjectOfType<ScoreSystem>();
        heathSystem = FindObjectOfType<Health>();
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {

            EnemyCore[] tempEnemies = GameObject.FindObjectsOfType<EnemyCore>();


            PlayerCore core = other.GetComponent<PlayerCore>();
            GlobalVariables.crystalsCollected++;
            Instantiate(audio, new Vector3(0, 0, 0), Quaternion.identity); //audio

            if (scoreSystem != null)
            {
                scoreSystem.crystalFound = true;
                if (tempEnemies != null)
                {
                    foreach (EnemyCore child in tempEnemies)
                    {
                        child.GetComponent<Health>().scaleWithCrystalsCollected = true;
                        child.GetComponent<Health>().UpdateMaxHealth();
                    }
                }          
            }

            if (core != null)
            {
                core.GetHUD().spellEditingController.crystalsLeft++;
                Debug.Log("Got a crystal!");
                //core.ToggleSpellEditingUI();
                core.cHealth.UpdateMaxHealth();
            }
            Destroy(gameObject);
        }
    }
}
