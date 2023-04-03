using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Assets.Scripts.Game.Boss.BossUtil
{
    public class AbilityTimer
    {
        public delegate void AbilityTimerCallBack();

        private float startTime;
        private float currentTime;
        private AbilityTimerCallBack callback;

        public AbilityTimer(float time, AbilityTimerCallBack callback) {
            startTime = time;
            currentTime = startTime;
            this.callback = callback;
        }

        public void Tick(float delta)
        {
            currentTime -= delta;
            if (currentTime < 0) {
                Stop();
            }
        }

        public void Stop()
        {
            callback();
            currentTime = startTime; 
        }

    }
}