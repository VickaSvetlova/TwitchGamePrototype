using System.Collections.Generic;
using System.Linq;
using Script.interfaces;
using UnityEngine;

namespace Script.controllers
{
    public class SurvivorStatisticController : MonoBehaviour
    {
        private Dictionary<User, SurvivorStatisticGame> _survivorsStatistics =
            new Dictionary<User, SurvivorStatisticGame>();

        public void NextWave()
        {
            foreach (var user in _survivorsStatistics.Values)
            {
                user.StatisticsGame.Add(new SurvivorStatisticWave());
            }
        }

        public void CreatedUser(User user)
        {
            if (!_survivorsStatistics.ContainsKey(user))
            {
                var temp = new SurvivorStatisticGame(new List<ISurvivorStatistic>());
                _survivorsStatistics.Add(user, temp);
                SubscribeEventBullet(user);
            }
        }

        private void SubscribeEventBullet(User user)
        {
            user.Character.OnEventBullet += ChangeStatistic;
        }

        private void ChangeStatistic(BaseBullet bullet)
        {
            var user = bullet.Owner.user;
            var statistic = _survivorsStatistics[user].StatisticsGame.Last();

            statistic.TotalShoot++;

            bullet.OnHit += (hitInfo) =>
            {
                statistic.AimHits += hitInfo.ShootAiming ? 1 : 0;
                statistic.HeadHits += hitInfo.Head ? 1 : 0;
                statistic.TotalKills += hitInfo.Dead ? 1 : 0;
            };
        }
    }
}