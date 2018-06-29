using ConsoleApp2.TwitchLibIRC;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public class TwitchIrc
    {

        private TcpClient _tcpClient;
        private StreamReader _inputStream;
        private StreamWriter _outputStream;

        private readonly string _userName;
        private readonly string _token;

        public TwitchIrc(string userName, string token)
        {
            _userName = userName;
            _token = token;
        }

        public void Connect()
        {
            _tcpClient = new TcpClient("irc.chat.twitch.tv", 6667);
            _inputStream = new StreamReader(_tcpClient.GetStream());
            _outputStream = new StreamWriter(_tcpClient.GetStream()) { AutoFlush = true };
            DoLogin();
        }

        public void ReplyPong(string message)
        {
            _outputStream.WriteLine("PONG");
        }

        public async Task<string> ReadMessageAsync()
        {
            return await _inputStream.ReadLineAsync();
        }

        private void DoLogin()
        {
            _outputStream.WriteLine("PASS oauth:" + _token);
            _outputStream.WriteLine("NICK " + _userName);

            _outputStream.WriteLine("CAP REQ twitch.tv/membership");
            _outputStream.WriteLine("CAP REQ twitch.tv/commands");
            _outputStream.WriteLine("CAP REQ twitch.tv/tags");
            _outputStream.WriteLine(Rfc2812.Join($"#gotaga"));
            _outputStream.WriteLine(Rfc2812.Join($"#gamesdonequick"));
            _outputStream.WriteLine(Rfc2812.Join($"#riot games"));
        }
    }
}