using System.Collections.Generic;
using System.Linq;
using Script.interfaces;
using UnityEngine;

namespace Script.controllers
{
    public class SurvivorStatisticController : MonoBehaviour
    {
        private Dictionary<User, ISurvivorStatistic> _survivorsStatistics = new Dictionary<User, ISurvivorStatistic>();

        public void CreatedUser(User user)
        {
            if (!_survivorsStatistics.ContainsKey(user))
            {
                _survivorsStatistics.Add(user, new SurvivorStatisticWave());
                SubscribeEventBullet(user);
            }
        }

        private void SubscribeEventBullet(User user)
        {
            user.Character.OnEventBullet += ChangeStatistic;
        }


        private void ChangeStatistic(BaseBullet bullet)
        {
            
        }
    }
}