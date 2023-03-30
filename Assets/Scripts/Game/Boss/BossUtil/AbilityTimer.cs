using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using BossArena.game;

namespace Assets.Scripts.Game.Boss.BossUtil
{
    public class AbilityTimer
    {
        public delegate void AbilityTimerCallBack();
        private float TimerCount { get; set; }
        private float currentTime;
        private bool isRunning;

        private AbilityTimerCallBack callback;

        public AbilityTimer(float time, AbilityTimerCallBack callback) {
            TimerCount = time;
            this.currentTime = TimerCount;
            this.callback = callback;
            this.isRunning = false;
        }

        public void Restart()
        {
            currentTime = TimerCount;
            isRunning = true;
        }

        public void Pause()
        {
            isRunning = false;
        }

        public void Run()
        {
            isRunning = true;
        }

        public void Update()
        {
            if (isRunning)
            {
                currentTime -= Time.deltaTime;
                if (currentTime < 0)
                {
                    callback();
                    isRunning = false;
                }
            }
        }

        public float GetCurrentTime()
        {
            return currentTime;
        }

    }
}