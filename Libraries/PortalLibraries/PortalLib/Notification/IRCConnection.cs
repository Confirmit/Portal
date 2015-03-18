using System;
using ConfirmIt.PortalLib.Notification;
using Meebey.SmartIrc4net;

using ConfirmIt.PortalLib.Properties;

namespace UlterSystems.PortalLib.Notification
{
    public class IRCConnection
    {
        private IrcClient irc;
        private string server = Settings.Default.IRCServer;
        private int port = Settings.Default.IRCPort;
        private string channel = Settings.Default.IRCChannel;
        private bool send_msg = Settings.Default.SendErrorIRCMsgToAdmin;

        public IRCConnection()
        {
            irc = new IrcClient();
            irc.OnConnected += new Meebey.SmartIrc4net.Delegates.SimpleEventHandler(OnConnected);
            irc.SendDelay = 1000; // time of channel listening in seconds

            try
            {
                irc.Connect(server, port);
            }
            catch (Exception ex)
            {
                if (send_msg)
                {
                    MailItem item = new MailItem
                    {
                        FromAddress = "PortalLib",
                        ToAddress = Settings.Default.ErrorToAddress,
                        Subject = Settings.Default.ErrorSubject,
                        MessageType = ((int)MailTypes.NewsNotification),
                        IsHTML = false,
                        Body = (ex.Message + Environment.NewLine + ex.StackTrace)
                    };
                    item.Save();

                }
            }
        }

        private void OnConnected()
        {
            irc.Login("Portal", "Portal"); // nick and realname
            irc.Join(channel);
        }

        public void SendMessage(string message)
        {
            try
            {
                irc.Message(SendType.Message, channel, message);
            }
            catch{ }
        }

        public void Disconnect()
        {
            try
            {
                irc.Listen();
            }
            catch (Exception)
            {}
        }
    }
}