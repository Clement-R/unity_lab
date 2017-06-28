using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPooling {
    public class SpawnObjects : MonoBehaviour {
        public float timeBetweenSpawns;
        public Stuff[] stuffPrefabs;
        public float velocity;

        float timeSinceLastSpawn;

        void FixedUpdate() {
            timeSinceLastSpawn += Time.deltaTime;
            if (timeSinceLastSpawn >= timeBetweenSpawns) {
                timeSinceLastSpawn -= timeBetweenSpawns;
                SpawnStuff();
            }
        }

        void SpawnStuff() {
            Stuff prefab = stuffPrefabs[Random.Range(0, stuffPrefabs.Length)];
            Stuff spawn = prefab.GetPooledInstance<Stuff>();
            spawn.transform.localPosition = transform.position;

            if(spawn.GetComponent<Rigidbody>()) {
                spawn.GetComponent<Rigidbody>().velocity = transform.up * velocity;
            } else {
                spawn.GetComponent<Rigidbody2D>().velocity = transform.up * velocity;
            }
        }
    }
}
