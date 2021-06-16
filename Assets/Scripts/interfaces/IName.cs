using UnityEngine;

namespace twitch.game.Iface
{
    public interface IName
    {
        public string CharacterName { get; set; }
        public GameObject CharacterGameObject { get; set; }
    }
}