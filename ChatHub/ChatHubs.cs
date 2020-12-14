﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace ChatHub
{
    public class ChatHubs : Hub
    {
        private static Dictionary<string, string> users = new Dictionary<string, string>();
        public static List<string> groups = new List<string> { "C# Programmers", "R43 Trainees", "NET Core Developers" };
        public override Task OnConnected()
        {
            
            Clients.All.message("New", "Hi there!");
            return base.OnConnected();
        }
        public void CreateGroup(string groupName)
        {
            string username = users[Context.ConnectionId];
            if (!groups.Contains(groupName))
            {
                groups.Add(groupName);
                Clients.All.message("SERVER", $"new group created by {username}");
                Clients.All.updateGroups(groups);
            }
        }
        public void JoinToGroup(string groupName)
        {
            Groups.Add(Context.ConnectionId, groupName);
            string username = users[Context.ConnectionId];
            Clients.Client(Context.ConnectionId).message("SERVER", $"Joined group {groupName}");
            Clients.Client(Context.ConnectionId).joinedGroup(groupName);

        }
        public void LeaveFromGroup(string groupName)
        {
            Groups.Remove(Context.ConnectionId, groupName);
            string username = users[Context.ConnectionId];
            Clients.Client(Context.ConnectionId).message("SERVER", $"left group {groupName}");
            Clients.Client(Context.ConnectionId).leftGroup(groupName);
        }
        public void MessageToGroup(string groupName, string message)
        {
            string username = users[Context.ConnectionId];
            Clients.Group(groupName).message(username, message);
        }
        public void AddMe(string username)
        {
            users.Add(Context.ConnectionId, username);
            Clients.All.updateList(users.Values.ToList());
            Clients.All.updateGroups(groups);

        }
        public void Send(string username, string message)
        {
            Clients.All.message(username, message);
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            string username = users[Context.ConnectionId];
            Clients.All.message("goodbye!", $"{username} left");
            users.Remove(Context.ConnectionId);
            Clients.All.updateList(users.Values.ToList());
            return base.OnDisconnected(stopCalled);
        }
        public void uploadImage(string username, ImageData data)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
            path = Path.Combine(path, data.Filename);
            data.Image = data.Image.Substring(data.Image.LastIndexOf(',') + 1);
            string converted = data.Image.Replace('-', '+');
            converted = converted.Replace('_', '/');
            byte[] buffer = Convert.FromBase64String(converted);
            FileStream fs = new FileStream($"{path}", FileMode.Create);
            fs.Write(buffer, 0, buffer.Length);
            fs.Close();
            //Debug.WriteLine(data);
            Clients.All.message(username, $"<img src='/Images/{data.Filename}' class='materialboxed' width='30' /> <a target='_blank' href='/Images/{data.Filename}'>download</a>");
        }
    }
    public class ImageData
    {
        public string Filename { get; set; }
        public string Image { get; set; }
    }
}