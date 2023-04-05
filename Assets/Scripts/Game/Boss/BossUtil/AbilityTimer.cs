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
    public class AbilityTimer<T, V>
    {
        public delegate T AbilityTimerCallBack(V t);
        private float TimerCount { get; set; }
        private float currentTime;
        private bool isRunning;
        private T type;
        private V argument;

        private AbilityTimerCallBack callback;

        public AbilityTimer(float time, AbilityTimerCallBack callback) {
            TimerCount = time;
            this.currentTime = TimerCount;
            this.callback = callback;
            this.isRunning = false;
        }

        public void SetArgument(V v)
        {
            this.argument = v;
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
                    callback(argument);
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