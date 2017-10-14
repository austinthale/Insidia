using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jack
{
    public class TypeP2 : MonoBehaviour
    {
        public float speed = 5;
        public GameObject destination;
        public float distracedDistance = 2;
        public float stoppingDistance = 1;

        private IEnumerator mainPath;
        private IEnumerator deviate;

        // Use this for initialization
        void Start()
        {
            mainPath = travel(destination);
            StartCoroutine(mainPath);
        }

        IEnumerator travel(GameObject destin)
        {
            while(gameObject.transform.position != destin.transform.position)
            {
                Move(destin);
                yield return null;
            }
        }

        void Move(GameObject target)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, target.transform.position, speed * Time.deltaTime);
        }

        IEnumerator distracted(GameObject distractor)
        {
            while(gameObject.transform.position != distractor.transform.position)
            {
                Move(distractor);
                yield return null;
            }
        }

        private void OnTriggerEnter(Collider other)
        { 
            StopCoroutine(mainPath);
            deviate = distracted(other.gameObject);
            StartCoroutine(deviate);

        }

        private void OnTriggerExit(Collider other)
        {
            StopCoroutine(deviate);
            StartCoroutine(mainPath);

        }
    }
}
