using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace UDP_test
{
    public partial class Form1 : Form
    {
        Dictionary<char, string> charTokoi2xxx = new Dictionary<char, string>()
        {
            {'ю', "C0"}, {'а', "C1"}, {'б', "C2"}, {'ц', "C3"}, {'д', "C4"}, {'е', "C5"}, {'ф', "C6"}, {'г', "C7"},
            {'х', "C8"}, {'и', "C9"}, {'й', "CA"}, {'к', "CB"}, {'л', "CC"}, {'м', "CD"}, {'н', "CE"}, {'о', "CF"},
            {'п', "D0"}, {'я', "D1"}, {'р', "D2"}, {'с', "D3"}, {'т', "D4"}, {'у', "D5"}, {'ж', "D6"}, {'в', "D7"},
            {'ь', "D8"}, {'ы', "D9"}, {'з', "DA"}, {'ш', "DB"}, {'э', "DC"}, {'щ', "DD"}, {'ч', "DE"}, {'ъ', "DF"}, {'ё', "A3"},

            {'Ю', "E0"}, {'А', "E1"}, {'Б', "E2"}, {'Ц', "E3"}, {'Д', "E4"}, {'Е', "E5"}, {'Ф', "E6"}, {'Г', "E7"},
            {'Х', "E8"}, {'И', "E9"}, {'Й', "EA"}, {'К', "EB"}, {'Л', "EC"}, {'М', "ED"}, {'Н', "EE"}, {'О', "EF"},
            {'П', "F0"}, {'Я', "F1"}, {'Р', "F2"}, {'С', "F3"}, {'Т', "F4"}, {'У', "F5"}, {'Ж', "F6"}, {'В', "F7"},
            {'Ь', "F8"}, {'Ы', "F9"}, {'З', "FA"}, {'Ш', "FB"}, {'Э', "FC"}, {'Щ', "FD"}, {'Ч', "FE"}, {'Ъ', "FF"}, {'Ё', "B3"},

        };

        Dictionary<string, char> koi2xxxToChar = new Dictionary<string, char>()
        {
            {"C0", 'ю'}, {"C1", 'а'}, {"C2", 'б'}, {"C3", 'ц'}, {"C4", 'д'}, {"C5", 'е'}, {"C6", 'ф'}, {"C7", 'г'},
            {"C8", 'х'}, {"C9", 'и'}, {"CA", 'й'}, {"CB", 'к'}, {"CC", 'л'}, {"CD", 'м'}, {"CE", 'н'}, {"CF", 'о'},
            {"D0", 'п'}, {"D1", 'я'}, {"D2", 'р'}, {"D3", 'с'}, {"D4", 'т'}, {"D5", 'у'}, {"D6", 'ж'}, {"D7", 'в'},
            {"D8", 'ь'}, {"D9", 'ы'}, {"DA", 'з'}, {"DB", 'ш'}, {"DC", 'э'}, {"DD", 'щ'}, {"DE", 'ч'}, {"DF", 'ъ'}, {"A3", 'ё'},


            {"E0", 'Ю'}, {"E1", 'А'}, {"E2", 'Б'}, {"E3", 'Ц'}, {"E4", 'Д'}, {"E5", 'Е'}, {"E6", 'Ф'}, {"E7", 'Г'},
            {"E8", 'Х'}, {"E9", 'И'}, {"EA", 'Й'}, {"EB", 'К'}, {"EC", 'Л'}, {"ED", 'М'}, {"EE", 'Н'}, {"EF", 'О'},
            {"F0", 'П'}, {"F1", 'Я'}, {"F2", 'Р'}, {"F3", 'С'}, {"F4", 'Т'}, {"F5", 'У'}, {"F6", 'Ж'}, {"F7", 'В'},
            {"F8", 'Ь'}, {"F9", 'Ы'}, {"FA", 'З'}, {"FB", 'Ш'}, {"FC", 'Э'}, {"FD", 'Щ'}, {"FE", 'Ч'}, {"FF", 'Ъ'}, {"B3", 'Ё'},


        };
        public string[] encode(string text)
        {
            string[] res = { "", "" };

            int strLength = text.Length;

            for (int i = 0; i < strLength; i++)
            {
                if (charTokoi2xxx.ContainsKey(text[i]))
                {
                    res[0] += charTokoi2xxx[text[i]];
                    res[1] += Convert.ToString(text[i], 2);
                    res[1] += " ";
                }
            }

            return res;
        }

        public string decode(string text)
        {
            string spltd = "";
            int strLength = text.Length;

            for (int i = 0; i < strLength; i++)
            {
                if (i % 2 == 0)
                {
                    spltd += " ";
                }
                spltd += text[i];
            }

            string res = "";
            string[] test = spltd.Split(' ');

            

            foreach (string i in test)
            {
                if (koi2xxxToChar.ContainsKey(i))
                {
                    res += koi2xxxToChar[i];
                }
            }

            return res;
        }





        string localIP;
        int localPort = 12345;

        string serverIp;
        int serverPort;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address.ToString();
            }

            this.Text = localIP;

            textBox5.Text = localPort.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TcpClient client = new TcpClient();
            try
            {
               
                client.Connect(IPAddress.Parse(serverIp), serverPort);
                NetworkStream stream = client.GetStream();

                
                string message = textBox4.Text;
                byte[] sendBytes = encode(message)[0].Select(c => (byte)c).ToArray();
                stream.Write(sendBytes, 0, sendBytes.Length);

                
                Invoke((MethodInvoker)(() => textBox3.Text = textBox3.Text + Environment.NewLine + localPort.ToString() + ": " + message));
                Invoke((MethodInvoker)(() => textBox6.Text = textBox6.Text + Environment.NewLine + localPort.ToString() + ": " + encode(message)[0]));
                Invoke((MethodInvoker)(() => textBox6.Text = textBox6.Text + Environment.NewLine + localPort.ToString() + ": " + encode(message)[1]));

                stream.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
            finally
            {
                client.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Thread server = new Thread(new ThreadStart(serverThread));

            serverIp = textBox1.Text;
            serverPort = Convert.ToInt32(textBox2.Text);

            localPort = Convert.ToInt32(textBox5.Text);
            textBox5.Enabled = false;
            button2.Enabled = false;

            server.Start();
        }

        public void serverThread()
        {
            TcpListener tcpServer = new TcpListener(IPAddress.Any, localPort);
            tcpServer.Start();

            while (true)
            {
                try
                {
                    
                    TcpClient client = tcpServer.AcceptTcpClient();
                    NetworkStream stream = client.GetStream();

                    
                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string message = new string(buffer.Take(bytesRead).Select(b => (char)b).ToArray());

                    
                    Invoke((MethodInvoker)(() => textBox3.Text = textBox3.Text + Environment.NewLine + serverPort.ToString() + ": " + decode(message)));
                    Invoke((MethodInvoker)(() => textBox6.Text = textBox6.Text + Environment.NewLine + serverPort.ToString() + ": " + message));

                    stream.Close();
                    client.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка на сервере: " + ex.Message);
                }
            }
        }

        
    }
}
