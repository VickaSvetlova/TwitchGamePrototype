using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using Script;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public partial class ChatController : MonoBehaviour
{
    #region AUTH

    string username = "Tory_Shepard", password = "oauth:c9nuxgq3lain6rt0z1uzrq53q4cb30", channelName = "tory_shepard";

    #endregion

    [SerializeField] Text chatBox;
    [SerializeField] private GameObject prefabPlayer;
    [SerializeField] private Transform startPos;
    [SerializeField] private int maxPlayerOn;
    [SerializeField] private float waitToReconnected;

    private bool onChatEnable;
    private IEnumerator reconected;
    private Dictionary<string, User> _users = new Dictionary<string, User>();
    private TcpClient _twitchClient;
    private StreamReader _reader;
    private StreamWriter _writer;
    private GameManager _gameManager;

    public GameManager Manager
    {
        set => _gameManager = value;
    }


    public bool ONChatEnable
    {
        get => onChatEnable;
        set => onChatEnable = value;
    }

    private void Start()
    {
        Connect();
        reconected = ReConnected();
        StartCoroutine(reconected);
    }

    private IEnumerator ReConnected()
    {
        while (true)
        {
            yield return new WaitForSeconds(waitToReconnected);
            Connect();
        }
    }

    private void Update()
    {
        if (!ONChatEnable) return;
        if (_twitchClient != null && !_twitchClient.Connected)
        {
            Connect();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Connect();
        }

        ReadChat();
    }

    private void Connect()
    {
        _twitchClient = new TcpClient("irc.chat.twitch.tv", 6667);
        _reader = new StreamReader(_twitchClient.GetStream());
        _writer = new StreamWriter(_twitchClient.GetStream());

        _writer.WriteLine("PASS " + password);
        _writer.WriteLine("NICK " + username);
        _writer.WriteLine("USER " + username + " 8 * :" + username);
        _writer.WriteLine("JOIN #" + channelName);
        _writer.Flush();
    }


    private void ReadChat()
    {
        if (_twitchClient != null && _twitchClient.Available > 0)
        {
            var message = _reader.ReadLine(); //прочитать сообщение
            //   print(message);
            if (message.Contains("PRIVMSG"))
            {
                //взять имя полльзователя и разбить на строку
                var splitPoint = message.IndexOf("!", 1);
                var chatName = message.Substring(0, splitPoint);
                chatName = chatName.Substring(1);

                //взять сообщение пользователя разбить на строку
                splitPoint = message.IndexOf(":", 1);
                message = message.Substring(splitPoint + 1);
                //print(String.Format("{0}: {1}", chatName, message));

                if (!_users.ContainsKey(chatName))
                {
                    if (_users.Count < maxPlayerOn)
                    {
                        if (message.ToLower() == "start")
                        {
                            var user = _gameManager.CharController.CreateCharacter(chatName);
                            _users.Add(chatName, user);

                            if (_users.Count > 0)
                            {
                                _gameManager.SetState(GameState.game);
                            }
                        }
                    }
                }
                else
                {
                    GameInputs(message, _users[chatName]);
                }

                chatBox.text = chatBox.text + "\n" + String.Format("{0}: {1}", chatName, message);
                //управление в игре
            }
        }
    }

    public void GameInputs(string chatInputs, User user)
    {
        user.Character.TakeCommand(chatInputs.ToLower());
    }
    
}