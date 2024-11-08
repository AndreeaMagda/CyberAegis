using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    // Prefabs for friend and stranger characters
    public GameObject friendPrefab;
    public GameObject strangerPrefab;

    // List of spawn points for the characters
    public Transform[] spawnPoints;

    // Time interval between spawns
    public float spawnInterval = 3.0f;

    // Probability of spawning a friend vs a stranger (e.g., 70% friends, 30% strangers)
    [Range(0, 1)]
    public float friendProbability = 0.7f;

    private InteractionPrompt interaction;
    private bool CanSpawn = true;

    private void Start()
    {
        interaction = FindObjectOfType<InteractionPrompt>();
        StartCoroutine(SpawnCharacters());
    }

    private IEnumerator SpawnCharacters()
    {
        while (true)
        {
            if (CanSpawn)
            {
                yield return new WaitForSeconds(spawnInterval);

                // Choose a random spawn point
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

                // Determine if the spawned character will be a friend or a stranger
                bool isFriend = Random.value < friendProbability;
                GameObject characterToSpawn = Random.value < friendProbability ? friendPrefab : strangerPrefab;

                // Instantiate the chosen character at the selected spawn point
                GameObject spawnedCharacter = Instantiate(characterToSpawn, spawnPoint.position, spawnPoint.rotation);

                if (interaction != null)
                {
                    CanSpawn = false;
                    interaction.ShowPrompt(isFriend, spawnedCharacter);
                }
            }
        }
    }
    // Method to allow spawning again, called from InteractionPrompt after a choice is made
    public void EnableSpawning()
    {
        canSpawn = true;
    }
}
