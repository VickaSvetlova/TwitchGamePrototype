// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.IO;
// using System.Net.Sockets;
// using UnityEngine;
// using UnityEngine.UI;
// using Random = UnityEngine.Random;
//
// public class TwitchChat : MonoBehaviour
// {
//     private TcpClient _twitchClient;
//     private StreamReader _reader;
//     private StreamWriter _writer;
//
//     public string username, password, channelName;
//
//     [SerializeField] Text chatBox;
//     [SerializeField] private GameObject prefabPlayer;
//     [SerializeField] private float speed;
//     private Dictionary<string, User> _users = new Dictionary<string, User>();
//     [SerializeField] private Transform startPos;
//     [SerializeField] private PlayerScore playerScore;
//     [SerializeField] private int maxPlayerOn;
//     [SerializeField] private RandomSpawnCoint randomSpawnCoint;
//     private IEnumerator reconected;
//     [SerializeField] private float waitToReconnected;
//
//     private void Start()
//     {
//         Connect();
//         randomSpawnCoint.PlayerGetCont += PlayerGetCoint;
//         reconected = ReConnected();
//         StartCoroutine(reconected);
//     }
//
//     private void PlayerGetCoint(int coin, Character character)
//     {
//         character.coin += coin;
//         playerScore.SetTableScore(_users);
//     }
//
//     private IEnumerator ReConnected()
//     {
//         while (true)
//         {
//             yield return new WaitForSeconds(waitToReconnected);
//             Connect();
//         }
//     }
//
//     private void Update()
//     {
//         if (!_twitchClient.Connected)
//         {
//             Connect();
//         }
//
//         if (Input.GetKeyDown(KeyCode.R))
//         {
//             Connect();
//         }
//
//         ReadChat();
//     }
//
//     private void Connect()
//     {
//         _twitchClient = new TcpClient("irc.chat.twitch.tv", 6667);
//         _reader = new StreamReader(_twitchClient.GetStream());
//         _writer = new StreamWriter(_twitchClient.GetStream());
//
//         _writer.WriteLine("PASS " + password);
//         _writer.WriteLine("NICK " + username);
//         _writer.WriteLine("USER " + username + " 8 * :" + username);
//         _writer.WriteLine("JOIN #" + channelName);
//         _writer.Flush();
//     }
//
//
//     private void ReadChat()
//     {
//         if (_twitchClient.Available > 0)
//         {
//             var message = _reader.ReadLine(); //прочитать сообщение
//             if (message.Contains("PRIVMSG"))
//             {
//                 //взять имя полльзователя и разбить на строку
//                 var splitPoint = message.IndexOf("!", 1);
//                 var chatName = message.Substring(0, splitPoint);
//                 chatName = chatName.Substring(1);
//
//                 //взять сообщение пользователя разбить на строку
//                 splitPoint = message.IndexOf(":", 1);
//                 message = message.Substring(splitPoint + 1);
//                 //print(String.Format("{0}: {1}", chatName, message));
//
//                 if (!_users.ContainsKey(chatName))
//                 {
//                     if (_users.Count < maxPlayerOn)
//                     {
//                         if (message.ToLower() == "start")
//                         {
//                             var player = Instantiate(prefabPlayer);
//                             SetPositionPlayer(player);
//
//                             var temp = player.GetComponent<Character>();
//                             var color = GetColor();
//                             var user = new User(chatName, temp, player.GetComponent<Rigidbody>(), color);
//
//                             player.GetComponent<Renderer>().material.color = color;
//                             temp.user = user;
//                             temp.imDeath += IsDeatchPlayer;
//                             temp.imLoseCoin += IsLoseCoin;
//                             _users.Add(chatName, user);
//
//                             playerScore.SetTableScore(_users);
//                         }
//                     }
//                 }
//                 else
//                 {
//                     GameInputs(message, _users[chatName].rigidbody);
//                 }
//
//
//                 chatBox.text = chatBox.text + "\n" + String.Format("{0}: {1}", chatName, message);
//                 //управление в игре
//             }
//         }
//     }
//
//     private void IsLoseCoin(GameObject obj)
//     {
//         playerScore.SetTableScore(_users);
//     }
//
//     private void IsDeatchPlayer(GameObject player)
//     {
//         var temp = player.GetComponent<Character>();
//         temp.coin -= 1;
//         if (temp.coin <= 0)
//         {
//             _users.Remove(temp.user.name);
//             temp.imDeath -= IsDeatchPlayer;
//             temp.imLoseCoin -= IsLoseCoin;
//             Destroy(player);
//         }
//         else
//         {
//             SetPositionPlayer(player);
//         }
//
//         playerScore.SetTableScore(_users);
//     }
//
//     private void SetPositionPlayer(GameObject player)
//     {
//         player.transform.position = startPos.position;
//     }
//
//     public void GameInputs(string chatInputs, Rigidbody userPlayer)
//     {
//         if (chatInputs.ToLower().Contains("left"))
//         {
//             userPlayer.AddForce(Vector3.left * (speed * 1000));
//         }
//
//         if (chatInputs.ToLower().Contains("right"))
//         {
//             userPlayer.AddForce(Vector3.right * (speed * 1000));
//         }
//
//         if (chatInputs.ToLower().Contains("up"))
//         {
//             userPlayer.AddForce(Vector3.forward * (speed * 1000));
//         }
//
//         if (chatInputs.ToLower().Contains("down"))
//         {
//             userPlayer.AddForce(Vector3.back * (speed * 1000));
//         }
//     }
//
//     private Color GetColor()
//     {
//         return Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
//     }
// }