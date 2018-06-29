using ConsoleApp2.TwitchLibIRC;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static TwitchIrc _twitch;

        static void Main(string[] args)
        {
            _twitch = new TwitchIrc("justinfan1676217621", "");
            var parser = new IrcParser();
            _twitch.Connect();

            Task.Run(async () =>
            {
                while (true)
                {
                    HandleIrcMessage(parser.ParseIrcMessage(await _twitch.ReadMessageAsync()));
                }
            });

            Console.ReadKey();
        }

        #region Ripped out of Twitch Lib

        private static void HandleIrcMessage(IrcMessage ircMessage)
        {
            if (ircMessage.Message.Contains("Login authentication failed"))
            {

            }

            switch (ircMessage.Command)
            {
                case IrcCommand.PrivMsg:
                    HandlePrivMsg(ircMessage);
                    return;
                case IrcCommand.Notice:
                    HandleNotice(ircMessage);
                    break;
                case IrcCommand.Ping:
                    _twitch.ReplyPong("PONG");
                    return;
                case IrcCommand.Pong:
                    return;
                case IrcCommand.Join:
                    HandleJoin(ircMessage);
                    break;
                case IrcCommand.Part:
                    HandlePart(ircMessage);
                    break;
                case IrcCommand.HostTarget:
                    break;
                case IrcCommand.ClearChat:
                    break;
                case IrcCommand.UserState:
                    HandleUserState(ircMessage);
                    break;
                case IrcCommand.GlobalUserState:
                    break;
                case IrcCommand.RPL_001:
                    break;
                case IrcCommand.RPL_002:
                    break;
                case IrcCommand.RPL_003:
                    break;
                case IrcCommand.RPL_004:
                    Handle004();
                    break;
                case IrcCommand.RPL_353:
                    Handle353(ircMessage);
                    break;
                case IrcCommand.RPL_366:
                    break;
                case IrcCommand.RPL_372:
                    break;
                case IrcCommand.RPL_375:
                    break;
                case IrcCommand.RPL_376:
                    break;
                case IrcCommand.Whisper:
                    break;
                case IrcCommand.RoomState:
                    HandleRoomState(ircMessage);
                    break;
                case IrcCommand.Reconnect:
                    break;
                case IrcCommand.UserNotice:
                    HandleUserNotice(ircMessage);
                    break;
                case IrcCommand.Mode:
                    HandleMode(ircMessage);
                    break;
                case IrcCommand.Unknown:
                    Console.WriteLine($"Unaccounted for: {ircMessage.ToString()}");
                    break;
                default:
                    Console.WriteLine($"Unaccounted for: {ircMessage.ToString()}");
                    break;
            }
        }

        #region IrcCommand Handling

        private static void HandlePrivMsg(IrcMessage ircMessage)
        {
            if (ircMessage.Hostmask.Equals("jtv!jtv@jtv.tmi.twitch.tv"))
            {
                return;
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[{DateTime.Now}] {ircMessage.User}: {ircMessage.Message}");
        }

        private static void HandleNotice(IrcMessage ircMessage)
        {
            if (ircMessage.Message.Contains("Improperly formatted auth"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Incorrect Login Details");
                return;
            }

            var success = ircMessage.Tags.TryGetValue(Tags.MsgId, out var msgId);
            if (!success)
            {
                Console.WriteLine($"Unaccounted for: {ircMessage.ToString()}");
            }

            switch (msgId)
            {
                case MsgIds.ColorChanged:
                    break;
                case MsgIds.HostOn:
                    break;
                case MsgIds.HostOff:
                    break;
                case MsgIds.ModeratorsReceived:
                    break;
                case MsgIds.NoPermission:
                    break;
                case MsgIds.RaidErrorSelf:
                    break;
                case MsgIds.RaidNoticeMature:
                    break;
                case MsgIds.MsgChannelSuspended:
                    break;
                default:
                    Console.WriteLine($"Unaccounted for: {ircMessage.ToString()}");
                    break;
            }
        }

        private static void HandleJoin(IrcMessage ircMessage)
        {
            Console.WriteLine($"User {ircMessage.User} Joined Channel {ircMessage.Channel}");
        }

        private static void HandlePart(IrcMessage ircMessage)
        {
            Console.WriteLine($"User {ircMessage.User} Left Channel {ircMessage.Channel}");
        }
        
        private static void HandleUserState(IrcMessage ircMessage)
        {
            var userState = new UserState(ircMessage);
            Console.WriteLine(userState.UserType);
        }

        private static void Handle004()
        {
            Console.WriteLine("Connected!");
        }

        private static void Handle353(IrcMessage ircMessage)
        {
            Console.WriteLine($"Users Deteched for Channel {ircMessage.Channel}");
            foreach (var user in ircMessage.Message.Split(' ').ToList())
            {
                Console.WriteLine(user);
            }
        }

        private static void HandleRoomState(IrcMessage ircMessage)
        {
            var channelstate = new ChannelState(ircMessage);
            Console.WriteLine($"Broadcaster Language: {channelstate.BroadcasterLanguage}");
        }

        private static void HandleUserNotice(IrcMessage ircMessage)
        {
            var successMsgId = ircMessage.Tags.TryGetValue(Tags.MsgId, out var msgId);
            if (!successMsgId)
            {
                Console.WriteLine($"Unaccounted for: {ircMessage.ToString()}");
                return;
            }

            switch (msgId)
            {
                case MsgIds.Raid:
                    break;
                case MsgIds.ReSubscription:
                    break;
                case MsgIds.Ritual:
                    break;
                case MsgIds.SubGift:
                    break;
                case MsgIds.Subscription:
                    break;
                default:
                    Console.WriteLine($"Unaccounted for: {ircMessage.ToString()}");
                    break;
            }
        }

        private static void HandleMode(IrcMessage ircMessage)
        {
            if (ircMessage.Message.StartsWith("+o"))
            {
                Console.WriteLine($"Moderator {ircMessage.Message.Split(' ')[1]} Joined Channel {ircMessage.Channel}");
            }

            if (ircMessage.Message.StartsWith("-o"))
            {
                Console.WriteLine($"Moderator {ircMessage.Message.Split(' ')[1]} Left Channel {ircMessage.Channel}");
            }
        }

        #endregion

        #endregion
    }
}
