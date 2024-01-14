using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAO.DAO;
using DAO.Models;
using Microsoft.AspNet.SignalR;
using PlanetX2012.Models.DAO;
using System.Linq;
using PlanetX2012.Topic;
using Newtonsoft.Json;
using System.Diagnostics;
using DAO.DAO.Repository;
namespace PlanetX2012.UserStatusManager
{
    public class SignalRHub : Hub
    {
        private SignalRConnectionRepository connectionController = new SignalRConnectionRepository();
        private UserStatusManager userStatusController = new UserStatusManager();

        private readonly static int _trendingTopicLimit = 10;


        public void Send(string message)
        {
            string clientid = Context.QueryString["clientid"];

            Clients.All.send(clientid + ": " + message);
        }

        public void SendUserTyping(string sendTo, bool isTyping)
        {
            string fromclientid = Context.QueryString["clientid"];

            foreach (var connectionId in connectionController.GetSignalRConnection(sendTo))
            {
                Clients.Client(connectionId).sendUserTyping(fromclientid, isTyping);

                //   LogMessage(fromclientid, connectionId.ToString(), isTyping.ToString());
            }
        }

        public void Send(string sendTo, string message, string from)
        {
            string fromclientid = Context.QueryString["clientid"];

            foreach (var connectionId in connectionController.GetSignalRConnection(sendTo))
            {
                Clients.Client(connectionId).send(fromclientid, from + ": " + message);

                //    LogMessage(fromclientid, connectionId.ToString(), from + ": " + message);
            }

            Clients.Caller.send(sendTo, "me: " + message);

        }

        public void UserStatusChange(EnumClass.UserStatus userStatus)
        {
            int clientid = Convert.ToInt32(Context.QueryString["clientid"]);
            userStatusController.UpdateStatus(clientid, userStatus);
            foreach (var connectionId in connectionController.GetAllFriendsSignalRConnection(Context.QueryString["clientid"]))
            {
                Clients.Client(connectionId).friendStatusChange(Context.QueryString["clientid"], userStatus);
            }


        }

        public void BroadCastPost(string postContent, int postId, string postWebContent)
        {



            foreach (var connectionId in connectionController.GetAllFriendsSignalRConnection(Context.QueryString["clientid"]))
            {
                Clients.Client(connectionId).recievePost(Context.QueryString["clientid"], postContent, postId, postWebContent);
            }

            Clients.Caller.recievePost(Context.QueryString["clientid"], postContent, postId, postWebContent);

        }

        public void BroadCastComment(long postCommentId, int parentCommentId, string postComment, int postId)
        {
            foreach (var connectionId in connectionController.GetAllFriendsSignalRConnection(Context.QueryString["clientid"]))
            {
                Clients.Client(connectionId).recieveComment(Context.QueryString["clientid"], postCommentId, parentCommentId, postComment, postId);
            }

            Clients.Caller.recieveComment(Context.QueryString["clientid"], postCommentId, parentCommentId, postComment, postId);

        }

        public override Task OnConnected()
        {
            int clientid = Convert.ToInt32(Context.QueryString["clientid"]);
            userStatusController.InsertConnection(clientid, EnumClass.UserStatus.Online, Context.ConnectionId);

            UserStatusChange(EnumClass.UserStatus.Online);
            connectionController.AddSignalRConnection(Context.QueryString["clientid"], Context.ConnectionId);
            foreach (var connectionId in connectionController.GetAllFriendsSignalRConnection(Context.QueryString["clientid"]))
            {
                Clients.Client(connectionId).friendConnected(Context.QueryString["clientid"]);
            }



            return base.OnConnected();
        }

        public override Task OnDisconnected()
        {

            connectionController.DeleteConnection(Context.QueryString["clientid"], Context.ConnectionId);

            if (connectionController.GetSignalRConnection(Context.QueryString["clientid"]).Count() == 0)
                UserStatusChange(EnumClass.UserStatus.Offline);

            return base.OnDisconnected();
        }

        public override Task OnReconnected()
        { return base.OnReconnected(); }

        public void SendTrendingTopics()
        {

            TopicManager topicController = new TopicManager();
            var trendTopics = (topicController.GetTrendingTopcis(_trendingTopicLimit)
                .Select(t =>
                    new { tag = t.Tag, tagCount = t.TagCount, tagId = t.TopicTagId }));

            Clients.Caller.recieveTrendingTopic(JsonConvert.SerializeObject(trendTopics));
        }

        private void LogMessage(string fromclientid, string toconnectionId, string message)
        {
            PlanetXContext db = new PlanetXContext();
            GeneralLog glog = new GeneralLog();
            Process currentProcess = Process.GetCurrentProcess();
            glog.LogText = String.Format("ConnectionId: {0} FromClientId {1} Message {2} ProcessId {3}", toconnectionId, fromclientid, message, currentProcess.Id.ToString());
            db.GeneralLogs.Add(glog);
            db.SaveChanges();

        }

        private void LogMessage(string toconnectionId, string clientid)
        {
            PlanetXContext db = new PlanetXContext();
            Process currentProcess = Process.GetCurrentProcess();
            GeneralLog glog = new GeneralLog();
            glog.LogText = String.Format("ConnectionId: {0} clientid {1} ProcessId {2} ", toconnectionId, clientid, currentProcess.Id.ToString());
            db.GeneralLogs.Add(glog);
            db.SaveChanges();

        }
    }
}