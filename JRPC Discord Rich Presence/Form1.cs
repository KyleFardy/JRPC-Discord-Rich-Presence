using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JRPC_Client;
using XDevkit;
using System.Net;

namespace JRPC_Discord_Rich_Presence
{
    public partial class Form1 : Form
    {
        private DiscordRPC.EventHandlers handlers;
        private DiscordRPC.RichPresence presence;
        public static IXboxConsole console;
        public static bool connected_to_console { get; set; }
        public static string current_game { get; set; }
        public static uint GTAV_ADDRESS = 0xc89A2ADB;
        public static uint GTAV_NAME_POINTER = 0x61;
        public static uint GTAV_IP_POINTER = 0x19;
        public Form1()
        {
            InitializeComponent();
        }
        public void load_names()
        {
            switch (console.XamGetCurrentTitleId().ToString("X"))
            {
                case "545408A7":
                    for (int i = 0; i < 18; i++)
                    {
                        string players_name = Encoding.ASCII.GetString(console.GetMemory((GTAV_ADDRESS + GTAV_NAME_POINTER) + ((uint)(120 * i)), 15)).TrimEnd(new char[1]);
                        string ip_address = new IPAddress(console.GetMemory((GTAV_ADDRESS + GTAV_IP_POINTER) + ((uint)(120 * i)), 4)).ToString();
                        if (!string.IsNullOrEmpty(players_name))
                        {
                            dataGridView1.Rows.Add(players_name, ip_address);
                        }
                    }
                break;
                case "454109BA":
                case "41560914": 
                case "415608FC": 
                case "415608CB":
                case "4156081C":
                case "415608C3":
                case "41560817": 
                case "415607E6":
                case "41560855":
                default:
                    MessageBox.Show("This Will Only Work On Grand Theft Auto V", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                if (console.Connect(out console))
                {
                    connected_to_console = true;
                    switch (console.XamGetCurrentTitleId().ToString("X"))
                    {
                        case "454109BA": current_game = "Battlefield 4"; break;
                        case "41560914": current_game = "Call Of Duty : Advanced Warfare"; break;
                        case "415608FC": current_game = "Call Of Duty : Ghosts"; break;
                        case "415608CB": current_game = "Call Of Duty : Modern Warfare 3"; break;
                        case "4156081C": current_game = "Call Of Duty : World At War"; break;
                        case "415608C3": current_game = "Call Of Duty : Black Ops 2"; break;
                        case "41560817": current_game = "Call Of Duty : Modern Warfare 2"; break;
                        case "415607E6": current_game = "Call Of Duty : Modern Warfare"; break;
                        case "545408A7": current_game = "Grand Theft Auto V"; break;
                        case "41560855": current_game = "Call Of Duty : Black Ops"; break;
                        default: current_game = "Unknown"; break;
                    }
                    label1.Text = "Current Game : " + current_game;
                    backgroundWorker1.RunWorkerAsync();
                    load_names();
                }
                else
                {
                    MessageBox.Show("Failed To Connect To Default Console", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {
                MessageBox.Show("Failed To Connect To Default Console", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            StartPosition:
            handlers = default(DiscordRPC.EventHandlers);
            DiscordRPC.Initialize("603916193899872286", ref handlers, true, null);
            presence.details = "Playing " + current_game;
            presence.state = "Created By Kyle Fardy";
            presence.largeImageKey = "smacker_solutions_main";
            if (connected_to_console)
            {
                switch (console.XamGetCurrentTitleId().ToString("X"))
                {
                    case "454109BA": presence.smallImageKey = "bf4"; break;
                    case "41560914": presence.smallImageKey = "callofduty"; break;
                    case "415608FC": presence.smallImageKey = "callofduty"; break;
                    case "415608CB": presence.smallImageKey = "callofduty"; break;
                    case "4156081C": presence.smallImageKey = "callofduty"; break;
                    case "415608C3": presence.smallImageKey = "callofduty"; break;
                    case "41560817": presence.smallImageKey = "callofduty"; break;
                    case "415607E6": presence.smallImageKey = "callofduty"; break;
                    case "545408A7": presence.smallImageKey = "smacker_solutions_gtav"; break;
                    case "41560855": presence.smallImageKey = "callofduty"; break;
                    default: presence.smallImageKey = "smacker_solutions_xbox"; break;
                }
            }
            else
            {
                presence.smallImageKey = "smacker_solutions_unknown";
            }
            presence.largeImageText = "JRPC Disccord Rich Presence";
            presence.smallImageText = "A Simple JRPC Discord Rich Presence!";
            DiscordRPC.UpdatePresence(ref presence);
            goto StartPosition;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            load_names();
        }
    }
}
