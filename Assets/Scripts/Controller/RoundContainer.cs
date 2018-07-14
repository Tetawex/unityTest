using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Interface;

namespace Assets.Scripts.Controller
{
    public class RoundContainer : MonoBehaviour
    {
        List<IDrawer> currentEnemies;
        public List<IDrawer> CurrentEnemies
        {
            get { return currentEnemies; }
        }

        int roundIndex = -1;
        public int TotalRounds { get { return transform.childCount; } }
        public bool HasMoreRounds { get { return roundIndex < TotalRounds-1; } }

        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        void Start()
        {
            SpawnNextRound();
        }

        public void SpawnNextRound()
        {
            if (roundIndex >= 0)
                transform.GetChild(roundIndex).name = "Dead Enemies " + roundIndex.ToString();

            roundIndex++;
            var newRoundParent = transform.GetChild(roundIndex);
            newRoundParent.gameObject.SetActive(true);
            newRoundParent.name = "Enemies";
            currentEnemies = new List<IDrawer>(newRoundParent.GetComponentsInChildren<IDrawer>());

            animator.Rebind();
            animator.SetTrigger("Fall");
            //animator.ResetTrigger("Fall");
        }

        void Update()
        {

        }
    }
}