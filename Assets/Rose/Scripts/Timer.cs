using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{

    public class Timer : MonoBehaviour
    {
        [SerializeField] float timeUntilWin;
        private float timeLeft;
        private float second;
        public static bool isAlive;
        public static bool youWin;

        private Text timer;

        private void Start()
        {
            isAlive = true;
            youWin = false;
            timeLeft = timeUntilWin;
            timer = GetComponent<Text>();
            second = 0;
        }

        private void Update()
        {
            timer.text = "Time: " + timeLeft;
            second += Time.deltaTime;
            if (second >= 1)
            {
                timeLeft -= 1;
                second = 0;
            }
            if (!isAlive)
            {
                timeLeft = timeUntilWin;
                isAlive = true;
                second = 0;
            }

            if (timeLeft <= 0f)
            {
                youWin = true;
                timeLeft = 0;
            }
        }

    }
}
