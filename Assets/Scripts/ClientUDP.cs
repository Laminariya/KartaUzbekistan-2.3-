using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ClientUDP : MonoBehaviour
{
    
    private UdpClient udpClient = new UdpClient();
    private GameManager _manager;
    private string _ip = "10.10.40.11";
    private int _port = 4023;
    private bool _isComplete;
    
    private Queue<string> _messages = new Queue<string>();
    
    // Start is called before the first frame update
    public void Init()
    {
        StartConnect();
    }

    private void StartConnect()
    {
        udpClient?.Dispose();
        udpClient = new UdpClient();
        udpClient.Connect(_ip, _port);
        _isComplete = true;
    }

    private void FixedUpdate()
    {
        if (_messages.Count > 0 && _isComplete)
        {
            Send(_messages.Dequeue());
        }
    }
    
    public void AddMessage(string str)
    {
        //Debug.Log(nameMess);
        _messages.Enqueue(str);
    }

    private async void Send(string message)
    {
        _isComplete = false;
        if (udpClient.Client.Connected) //Проверяем есть ли подключение
        {
            // отправляемые данные
            // преобразуем в массив байтов
            //Debug.Log("UDp");
            byte[] data = Encoding.UTF8.GetBytes(message);
            // отправляем данные
            try
            {
                int bytes = await udpClient.SendAsync(data, data.Length);
            }
            catch (Exception e)
            {
                Debug.LogError("XXX "+e);
                //StartConnect();
                //Send(message);
                throw;
            }
            
        }
        else //Если нет, то пробуем переподключиться
        {
            udpClient.Connect(_ip, _port);
            await Task.Delay(500);
            if (udpClient.Client.Connected)
            {
                // преобразуем в массив байтов
                byte[] data = Encoding.UTF8.GetBytes(message);
                // отправляем данные
                int bytes = await udpClient.SendAsync(data, data.Length);
            }
        }

        await Task.Delay(100);
        _isComplete = true;
    }
    
}
