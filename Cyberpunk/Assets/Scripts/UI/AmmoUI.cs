using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HL
{
    public class AmmoUI : MonoBehaviour
    {
        [SerializeField] private GameObject roundUIPrefab;
        [SerializeField] private GameObject ammoRoundsParent;
        List<GameObject> rounds = new();

        public void UpdateUIAmmoCapacity(int ammoCapacity)
        {
            if (rounds.Count > 0)
                foreach (GameObject round in rounds)
                    Destroy(round);
            rounds.Clear();

            for (int i = 0; i < ammoCapacity; i++)
            {
                GameObject round = Instantiate(roundUIPrefab, ammoRoundsParent.transform);
                rounds.Add(round);
            }
        }

        public IEnumerator ReloadAllAmmoUI(float reloadTime)
        {
            yield return new WaitForSeconds(reloadTime);

            if (rounds.Count > 0)
            {
                foreach (GameObject round in rounds)
                {
                    Image roundImage = round.transform.GetChild(1).GetComponent<Image>();
                    roundImage.enabled = true;
                }
            }
        }

        public void UseOneAmmoUI()
        {
            // Iterate backwards through the loop so we start from the right hand side
            for (int i = rounds.Count - 1; i >= 0; i--)
            {
                Image roundImage = rounds[i].transform.GetChild(1).GetComponent<Image>();
                if (roundImage.enabled)
                {
                    roundImage.enabled = false;
                    break; // Exit the loop once the first enabled round is found
                }
            }
        }
    }
}