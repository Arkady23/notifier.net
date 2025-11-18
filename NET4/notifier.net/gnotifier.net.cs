//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
//!!!                                                   !!!
//!!!  gnotifier.net на C#.       Автор: A.Б.Корниенко  !!!
//!!!  v0.2.0.0                             19.11.2025  !!!
//!!!                                                   !!!
//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Reflection;
using System.Net.Sockets;
using System.Net.Security;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;

public class f : Form {
    IContainer conta = new Container();
    ContextMenuStrip menu = new ContextMenuStrip();
    ToolStripMenuItem menuT1 = new ToolStripMenuItem();
    ToolStripMenuItem menuT2 = new ToolStripMenuItem();
    ToolStripMenuItem menuT3 = new ToolStripMenuItem();
    ToolStripMenuItem menuT4 = new ToolStripMenuItem();
    ToolStripMenuItem menuT5 = new ToolStripMenuItem();
    ToolStripMenuItem menuQ = new ToolStripMenuItem();
    ToolStripMenuItem menuR = new ToolStripMenuItem();
    StringBuilder messageData;
    StreamWriter logSW;
    Random rnd1, rnd2;
    Font fBold, fStd;
    TcpClient client;
    SslStream stream;
    NotifyIcon nIcon;
    StreamWriter sw;
    Thread tMon;
    Encoding win1251 = Encoding.GetEncoding(1251);
    const string begin1 = "Connection...", notConnect1 = "The server does not connect",
          unseen_1 = "unread email", unseen_2 = "unread emails", allSeen1 = "No new emails",
          restart1 = "R&estart", authFailed1 = "Authentication failed", quit1 = "Q&uit",
          notifier1 = "gnotifier.net.";
    const int secs1=30, show1=6400, min1=5, port1=993;
    const byte b27 = 27, b53 = 53, b64 = 64, b70 = 70, b91 = 91;
    static string Folder = Thread.GetDomain().BaseDirectory;
    string ini=Folder+notifier1+"ini", imap, email, passw, passs, url, message, min,
           authFailed, notConnect, checkInterval, unseen1, unseen2, unseen5, allSeen,
           begin, restart, quit;
    char[] empty = new Char[] {' ','\t'};
    char[] rasdy = new Char[] {'=',';'};
    int i, j, k, n, m, p, q, r, h, u, v, w, i1, i2, i3, k1, k2, k3, nList,
        port, mins, repeat, t1, t2, t3, t4, t5;
    byte[] bb, bs, b1, b2 = new Byte[2], bytes = new byte[2048];
    bool l, save, notExit = true;
    string[] param, opts, mess, folders, crlf = new string[] { "\r\n" },
             unseen = new string[] { "(UNSEEN" };
    byte answer, key, len;
    DateTime dt;
    static readonly byte[] icobin1 = new byte[] {
        0,0,1,0,1,0,32,32,0,0,1,0,32,0,168,16,0,0,22,0,0,0,40,0,0,0,32,0,0,0,64,0,0,0,1,0,
        32,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,185,176,159,170,185,176,159,255,185,176,159,255,185,176,
        159,255,185,176,159,255,185,176,159,255,185,176,159,255,185,176,159,255,185,176,159,
        255,185,176,159,255,185,176,159,255,185,176,159,255,185,176,159,255,185,176,159,255,
        185,176,159,255,185,176,159,255,185,176,159,255,185,176,159,255,185,176,159,255,185,
        176,159,255,185,176,159,255,185,176,159,255,185,176,159,255,185,176,159,255,185,176,
        159,255,185,176,159,170,0,0,0,0,0,0,0,0,0,0,0,0,246,132,0,56,247,133,0,213,247,133,
        0,255,247,133,0,255,247,133,0,255,247,133,0,255,247,133,0,255,227,201,184,255,220,
        216,207,255,220,216,207,255,220,216,207,255,220,216,207,255,220,216,207,255,220,216,
        207,255,220,216,207,255,220,216,207,255,220,216,207,255,220,216,207,255,220,216,207,
        255,220,216,207,255,220,216,207,255,220,216,207,255,220,216,207,255,220,216,207,255,
        198,206,184,254,75,169,0,255,75,169,0,255,75,169,0,255,75,169,0,255,75,169,0,255,
        75,169,0,213,73,168,0,56,247,133,0,214,247,133,0,255,247,133,0,255,247,133,0,255,
        247,133,0,255,247,133,0,255,247,133,0,255,245,224,214,250,227,224,217,243,224,221,
        213,247,220,216,207,255,220,216,207,255,220,216,207,255,220,216,207,255,220,216,207,
        255,220,216,207,255,220,216,207,255,220,216,207,255,220,216,207,255,220,216,207,255,
        220,216,207,255,220,216,207,255,222,219,211,247,224,221,215,242,210,221,206,249,75,
        169,0,255,75,169,0,255,75,169,0,255,75,169,0,255,75,169,0,255,75,169,0,255,75,169,
        0,213,247,133,0,254,247,133,0,255,247,133,0,255,247,133,0,255,247,133,0,255,247,133,
        0,255,247,133,0,255,249,226,217,254,250,250,250,255,239,237,235,249,220,216,207,235,
        220,216,207,255,220,216,207,255,220,216,207,255,220,216,207,255,220,216,207,255,220,
        216,207,255,220,216,207,255,220,216,207,255,220,216,207,255,220,216,207,255,220,216,
        207,235,232,230,227,249,238,238,238,255,208,221,206,248,75,169,0,255,75,169,0,255,
        75,169,0,255,75,169,0,255,75,169,0,255,75,169,0,255,75,169,0,254,247,133,0,255,247,
        133,0,255,247,133,0,255,247,133,0,255,247,133,0,255,247,133,0,255,247,133,0,255,249,
        225,216,248,250,250,250,255,250,250,250,255,247,247,247,247,231,229,224,252,220,216,
        207,235,220,216,207,255,220,216,207,255,220,216,207,255,220,216,207,255,220,216,207,
        255,220,216,207,255,220,216,207,235,226,224,218,236,237,236,236,247,238,238,238,255,
        238,238,238,255,208,221,206,248,75,169,0,255,75,169,0,255,75,169,0,255,75,169,0,255,
        75,169,0,255,75,169,0,255,75,169,0,255,247,133,0,255,247,133,0,255,247,133,0,255,
        247,133,0,255,247,133,0,255,247,133,0,255,247,133,0,255,249,225,216,248,250,250,250,
        255,250,250,250,255,250,250,250,255,250,250,250,255,243,242,239,248,224,221,213,247,
        220,216,207,255,220,216,207,255,220,216,207,255,220,216,207,255,222,219,211,247,234,
        233,231,248,238,238,238,255,238,238,238,255,238,238,238,255,238,238,238,255,208,221,
        206,248,75,169,0,255,75,169,0,255,75,169,0,255,75,169,0,255,75,169,0,255,75,169,0,
        255,75,169,0,255,247,133,0,255,247,133,0,255,247,133,0,255,247,133,0,255,247,133,
        0,255,247,133,0,255,247,133,0,255,249,225,216,248,250,250,250,255,250,250,250,255,
        250,250,250,255,250,250,250,255,250,250,250,255,249,249,249,246,235,233,230,250,222,
        218,210,249,222,218,210,249,230,228,223,250,238,238,237,246,238,238,238,255,238,238,
        238,255,238,238,238,255,238,238,238,255,238,238,238,255,208,221,206,248,75,169,0,
        255,75,169,0,255,75,169,0,255,75,169,0,255,75,169,0,255,75,169,0,255,75,169,0,255,
        247,133,0,255,247,133,0,255,247,133,0,255,247,133,0,255,247,133,0,255,247,133,0,255,
        247,133,0,255,249,225,216,248,250,250,250,255,250,250,250,255,250,250,250,255,250,
        250,250,255,250,250,250,255,250,250,250,255,236,237,251,247,126,131,254,244,115,121,
        252,245,223,224,241,247,238,238,238,255,238,238,238,255,238,238,238,255,238,238,238,
        255,238,238,238,255,238,238,238,255,208,221,206,248,75,169,0,255,75,169,0,255,75,
        169,0,255,75,169,0,255,75,169,0,255,75,169,0,255,75,169,0,255,247,133,0,255,247,133,
        0,255,247,133,0,255,247,133,0,255,247,133,0,255,247,133,0,255,247,133,0,255,249,225,
        216,248,250,250,250,255,250,250,250,255,250,250,250,255,250,250,250,255,250,250,250,
        255,213,214,252,249,49,65,255,237,49,65,255,255,49,65,255,255,49,65,255,241,199,201,
        244,249,238,238,238,255,238,238,238,255,238,238,238,255,238,238,238,255,238,238,238,
        255,208,221,206,248,75,169,0,255,75,169,0,255,75,169,0,255,75,169,0,255,75,169,0,
        255,75,169,0,255,75,169,0,255,247,133,0,255,247,133,0,255,247,133,0,255,247,133,0,
        255,247,133,0,255,247,133,0,255,247,133,0,255,249,225,216,248,250,250,250,255,250,
        250,250,255,250,250,250,255,248,248,250,208,179,182,253,251,49,65,255,254,49,65,255,
        255,49,65,255,255,49,65,255,255,49,65,255,255,49,65,255,255,153,156,249,236,235,235,
        239,246,238,238,238,255,238,238,238,255,238,238,238,255,208,221,206,248,75,169,0,
        255,75,169,0,255,75,169,0,255,75,169,0,255,75,169,0,255,75,169,0,255,75,169,0,255,
        247,133,0,255,247,133,0,255,247,133,0,255,247,133,0,255,247,133,0,255,247,133,0,255,
        247,133,0,255,249,225,216,248,250,250,250,255,250,250,250,255,236,237,251,247,126,
        131,254,244,49,65,255,255,49,65,255,255,49,65,255,255,49,65,255,255,49,65,255,255,
        49,65,255,255,49,65,255,255,49,65,255,255,115,121,252,245,223,224,241,247,238,238,
        238,255,238,238,238,255,208,221,206,248,75,169,0,255,75,169,0,255,75,169,0,255,75,
        169,0,255,75,169,0,255,75,169,0,255,75,169,0,255,247,133,0,255,247,133,0,255,247,
        133,0,255,247,133,0,255,247,133,0,255,247,133,0,255,247,133,0,255,249,226,217,255,
        250,250,250,255,213,214,252,249,49,65,255,237,49,65,255,255,49,65,255,255,49,65,255,
        255,49,65,255,255,49,65,255,255,49,65,255,255,49,65,255,255,49,65,255,255,49,65,255,
        255,49,65,255,255,49,65,255,241,199,201,244,249,238,238,238,255,208,221,206,248,75,
        169,0,255,75,169,0,255,75,169,0,255,75,169,0,255,75,169,0,255,75,169,0,255,75,169,
        0,255,247,133,0,255,247,133,0,255,247,133,0,255,247,133,0,255,247,133,0,255,247,133,
        0,255,247,133,0,255,246,224,215,249,172,175,253,233,49,65,255,254,49,65,255,255,49,
        65,255,255,49,65,255,255,49,65,255,255,49,65,255,255,49,65,255,255,49,65,255,255,
        49,65,255,255,49,65,255,255,49,65,255,255,49,65,255,255,49,65,255,255,49,65,255,255,
        153,156,249,236,206,219,204,249,75,169,0,255,75,169,0,255,75,169,0,255,75,169,0,255,
        75,169,0,255,75,169,0,255,75,169,0,255,247,133,0,255,247,133,0,255,247,133,0,255,
        247,133,0,255,247,133,0,255,247,133,0,255,219,120,29,253,118,121,241,246,49,65,255,
        255,49,65,255,255,49,65,255,255,49,65,255,255,49,65,255,255,49,65,255,255,49,65,255,
        255,49,65,255,255,49,65,255,255,49,65,255,255,49,65,255,255,49,65,255,255,49,65,255,
        255,49,65,255,255,49,65,255,255,49,65,255,255,115,132,240,245,66,172,32,253,75,169,
        0,255,75,169,0,255,75,169,0,255,75,169,0,255,75,169,0,255,75,169,0,255,247,133,0,
        255,247,133,0,255,247,133,0,255,247,133,0,255,247,133,0,255,179,101,70,255,33,32,
        222,250,43,55,248,255,49,65,255,255,49,65,255,255,49,65,255,255,49,65,255,255,49,
        65,255,255,49,65,255,255,49,65,255,255,49,65,255,255,49,65,255,255,49,65,255,255,
        49,65,255,255,49,65,255,255,49,65,255,255,49,65,255,255,49,65,255,255,49,65,255,255,
        36,98,255,253,3,188,246,250,50,176,85,255,75,169,0,255,75,169,0,255,75,169,0,255,
        75,169,0,255,75,169,0,255,247,133,0,255,247,133,0,255,247,133,0,255,243,131,4,255,
        128,77,122,255,26,28,228,255,25,28,229,255,43,55,248,255,49,65,255,255,49,65,255,
        255,49,65,255,255,49,65,255,255,49,65,255,255,49,65,255,255,49,65,255,255,49,65,255,
        255,49,65,255,255,49,65,255,255,49,65,255,255,49,65,255,255,49,65,255,255,49,65,255,
        255,49,65,255,255,49,65,255,255,36,99,255,255,0,188,255,255,0,188,255,255,30,181,
        153,255,73,169,6,255,75,169,0,255,75,169,0,255,75,169,0,255,247,133,0,255,247,133,
        0,255,221,120,27,255,74,52,178,255,25,28,229,255,25,28,229,255,25,28,229,255,43,55,
        248,255,49,65,255,255,49,65,255,255,49,65,255,255,49,65,255,255,49,65,255,255,49,
        65,255,255,49,65,255,255,152,154,249,246,156,157,249,253,49,65,255,255,49,65,255,
        255,49,65,255,255,49,65,255,255,49,65,255,255,49,65,255,255,49,65,255,255,36,99,255,
        255,0,188,255,255,0,188,255,255,0,188,255,255,15,184,204,255,65,171,34,255,75,169,
        0,255,75,169,0,255,247,133,0,255,179,101,70,255,40,35,213,255,25,28,229,255,25,28,
        229,255,25,28,229,255,25,28,229,255,43,55,248,255,49,65,255,255,49,65,255,255,49,
        65,255,255,49,65,255,255,49,65,255,255,49,65,255,247,188,188,244,250,241,239,236,
        255,241,239,236,255,198,197,243,249,68,79,254,253,49,65,255,255,49,65,255,255,49,
        65,255,255,49,65,255,255,49,65,255,255,36,99,255,255,0,188,255,255,0,188,255,255,
        0,188,255,255,0,188,255,255,5,186,238,255,52,175,79,255,75,169,0,255,115,71,136,240,
        26,28,228,255,25,28,229,255,25,28,229,255,25,28,229,255,25,28,229,255,25,28,229,255,
        43,55,248,255,49,65,255,255,49,65,255,255,49,65,255,255,49,65,255,255,103,109,253,
        248,220,219,239,248,241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,255,
        223,222,239,247,108,114,252,247,49,65,255,255,49,65,255,255,49,65,255,255,49,65,255,
        255,36,99,255,255,0,188,255,255,0,188,255,255,0,188,255,255,0,188,255,255,0,188,255,
        255,0,188,254,255,31,180,152,240,25,28,229,255,25,28,229,255,25,28,229,255,25,28,
        229,255,25,28,229,255,25,28,229,255,25,28,229,255,43,55,248,255,49,65,255,255,49,
        65,255,255,49,65,255,255,148,150,249,238,236,234,237,246,241,239,236,255,241,239,
        236,255,241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,255,236,234,237,
        246,155,157,248,252,49,65,255,255,49,65,255,255,49,65,255,255,36,99,255,255,0,188,
        255,255,0,188,255,255,0,188,255,255,0,188,255,255,0,188,255,255,0,188,255,255,0,188,
        255,255,25,28,229,255,25,28,229,255,25,28,229,255,25,28,229,255,25,28,229,255,25,
        28,229,255,25,28,229,255,43,55,248,255,49,65,255,255,49,65,255,247,188,188,244,250,
        241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,255,241,
        239,236,255,241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,255,198,197,
        243,249,49,65,255,245,49,65,255,255,36,99,255,255,0,188,255,255,0,188,255,255,0,188,
        255,255,0,188,255,255,0,188,255,255,0,188,255,255,0,188,255,255,25,28,229,254,25,
        28,229,255,25,28,229,255,25,28,229,255,25,28,229,255,25,28,229,255,25,28,229,255,
        43,55,248,255,103,109,253,248,220,219,239,248,241,239,236,255,241,239,236,255,241,
        239,236,255,241,239,236,255,241,239,236,255,241,239,236,246,241,239,236,255,241,239,
        236,255,241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,255,223,222,239,
        247,106,113,253,247,37,96,255,249,0,188,255,255,0,188,255,255,0,188,255,255,0,188,
        255,255,0,188,255,255,0,188,255,255,0,188,255,254,25,27,229,223,25,28,229,255,25,
        28,229,255,25,28,229,255,25,28,229,255,25,28,229,255,25,28,229,255,146,148,243,252,
        236,234,237,246,241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,255,241,
        239,236,255,241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,255,241,239,
        236,255,241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,255,236,234,237,
        246,151,171,248,252,0,188,255,255,0,188,255,255,0,188,255,255,0,188,255,255,0,188,
        255,255,0,188,255,255,0,188,255,222,25,27,228,113,25,28,229,255,25,28,229,255,25,
        28,229,255,25,28,229,255,25,28,229,246,192,191,233,250,241,239,236,255,241,239,236,
        255,241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,255,
        241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,255,241,
        239,236,255,241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,255,241,239,
        236,255,191,220,244,250,0,188,255,246,0,188,255,255,0,188,255,255,0,188,255,255,0,
        188,255,255,0,188,255,111,0,0,255,1,25,27,230,112,25,28,229,222,25,28,229,249,135,
        135,231,253,221,219,234,248,241,239,236,255,241,239,236,255,241,239,236,255,241,239,
        236,255,241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,
        255,241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,255,
        241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,255,241,
        239,236,255,220,230,239,248,125,201,251,242,0,188,255,249,0,188,255,221,0,188,255,
        111,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,185,176,159,170,185,176,159,170,185,176,159,170,
        185,176,159,170,185,176,159,170,185,176,159,170,185,176,159,170,185,176,159,170,185,
        176,159,170,185,176,159,170,185,176,159,170,185,176,159,170,185,176,159,170,185,176,
        159,170,185,176,159,170,185,176,159,170,185,176,159,170,185,176,159,170,185,176,159,
        170,185,176,159,170,185,176,159,170,185,176,159,170,185,176,159,170,185,176,159,170,
        185,176,159,170,185,176,159,170,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,185,
        176,159,170,185,176,159,255,185,176,159,255,185,176,159,255,185,176,159,255,185,176,
        159,255,185,176,159,255,185,176,159,255,185,176,159,255,185,176,159,255,185,176,159,
        255,185,176,159,255,185,176,159,255,185,176,159,255,185,176,159,255,185,176,159,255,
        185,176,159,255,185,176,159,255,185,176,159,255,185,176,159,255,185,176,159,255,185,
        176,159,255,185,176,159,255,185,176,159,255,185,176,159,255,185,176,159,170,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,255,255,255,255,255,255,255,255,255,255,255,
        255,224,0,0,7,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,224,0,0,7,224,0,0,7,255,255,255,255,255,
        255,255,255
    };
    Icon ico0 = Icon.ExtractAssociatedIcon(Folder+notifier1+"exe"),
         ico1 = new Icon(new MemoryStream(icobin1));

    protected override void Dispose( bool disposing ) {
      // Clean up any container being used.
      if( disposing )
          if (conta != null) conta.Dispose();            
      base.Dispose( disposing );
    }

    void nIcon_BalloonTipShown(object Sender, EventArgs e) {
      dt = DateTime.Now;
    }

    void nIcon_BalloonTipClosed(object Sender, EventArgs e) {

      // К сожалению нет свойства, показывающего закрыл ли уведомление пользователь сам.
      // Поэтому просто сравниваем со значением show1, полученным эксперементально.
      if((DateTime.Now-dt).TotalMilliseconds<show1) {
        seen();
        n = 0;
      }
    }

    void nIcon_BalloonTipClicked(object Sender, EventArgs e) {
      n = 0;
      seen();
      if(url.Length>0) {
        try { System.Diagnostics.Process.Start(url); } catch (Exception) { }
      }
    }

    void nIcon_Clicked(object Sender, MouseEventArgs e) {
      if(e.Button != MouseButtons.Right) nIcon_BalloonTipClicked(null, null);
    }

    void menuT1_Click(object Sender, EventArgs e) {
      updT(1, ref t1);
    }

    void menuT2_Click(object Sender, EventArgs e) {
      updT(2,ref t2);
    }

    void menuT3_Click(object Sender, EventArgs e) {
      updT(3, ref t3);
    }

    void menuT4_Click(object Sender, EventArgs e) {
      updT(4, ref t4);
    }

    void menuT5_Click(object Sender, EventArgs e) {
      updT(5, ref t5);
    }

    void menuR_Click(object Sender, EventArgs e) {
      iniMonitoring();
      set_fStd(i2);
      m = 0;
    }

    void menuQ_Click(object Sender, EventArgs e) {
      StopMonitoring();
    }

    void set_fStd(int i) {
      for (i2=1; i2<6; i2++) if(i2 != i) menu.Items[i2].Font = fStd;
    }

    void updT(int i, ref int t) {
      if (mins != t) {
        mins = t;
        updMins();
        set_fStd(i);
        m = 0;
      }
    }

    void updMins() {
      t5 = 2;
      t4 = 5;
      t3 = 10;
      t2 = 20;
      t1 = 30;
      if(mins<3) {
        t5 = mins;
        i2 = 5;
      } else if(mins<10) {
        t4 = mins;
        i2 = 4;
      } else if(mins<20) {
        t3 = mins;
        i2 = 3;
      } else if(mins<30) {
        t2 = mins;
        i2 = 2;
      } else {
        t1 = mins;
        i2 = 1;
      }
      menu.Items[i2].Font = fBold;
      menuT1.Text = t1+" "+min;
      menuT2.Text = t2+" "+min;
      menuT3.Text = t3+" "+min;
      menuT4.Text = t4+" "+min;
      menuT5.Text = t5+" "+min;
    }

    void log(string x) {
      if(!(logSW != null)){
        logSW = new StreamWriter(new FileStream(Folder+notifier1+"log", FileMode.Create,
                                     FileAccess.Write, FileShare.ReadWrite));
        Console.SetOut(logSW);
      }
      Console.WriteLine(x);
      logSW.Flush();
    }

    int next1() {
      int k1 = rnd1.Next(65,117);
      if(k1>90) k1 += 6;
      return k1;
    }

    // Запутать пароль, чтобы никто не украл
    void getPasss() {
      q = passw.Length>255? 255:passw.Length;
      len = (byte)q;
      if(len<b27) {
        len += b64;
      } else if(len<b53){
        len += b70;
      } else {
        len = b53;
      }
      rnd1 = new Random();
      key = (byte)next1();
      rnd2 = new Random(key);
      r = q;
      if(len > b53) {
        r += 20;
        r -= r%10+2;
      }
      bb = new Byte[r];
      rnd2.NextBytes(bb);
      passs = passw;
      for(p = q; p<r; p++) passs += (char)next1();
      b1 = win1251.GetBytes(passs);
      Array.Sort(bb,b1);
      b2[0] = key;
      b2[1] = len;
      save = notExit;
      passs = win1251.GetString(b2)+win1251.GetString(b1);
    }

    void getPassw() {
      if(passs.Length>2) {
        key = (byte)passs[0];
        len = (byte)passs[1];
        b1 = new Byte[passs.Length-2];
        win1251.GetBytes(passs,2,b1.Length,b1,0);
        if(len>b53) {
          len -= len<b91? b64:b70;
        } else {
          len = (byte)b1.Length;
        }
        if(passs.Length>=len) {
          rnd2 = new Random(key);
          bs = new Byte[b1.Length];
          bb = new Byte[b1.Length];
          for(p=0; p<b1.Length; p++) bs[p] = (byte)p;
          rnd2.NextBytes(bb);
          Array.Sort(bb,bs);
          Array.Sort(bs,b1);
          passw = win1251.GetString(b1,0,len);
        }
      }
    }

    [STAThread]
    static void Main (string[] args) {
      Directory.SetCurrentDirectory(Folder);
      Application.Run( new f(args));
    }

    public f(string[] args) {

      // Никаких окон не надо
      this.WindowState = FormWindowState.Minimized;
      this.ShowInTaskbar = false;

      // Анонимная функция перехвата и вывода ошибки
      AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) =>
      {
        log(" "+((Exception)eventArgs.ExceptionObject).ToString());
        StopMonitoring();
      };

      AppDomain.CurrentDomain.ProcessExit += (sender, eventArgs) =>
      {
          StopMonitoring();
      };

      // Initialize menu
      menu.ShowImageMargin = false;
      menu.Items.AddRange(new ToolStripMenuItem[] {menuT1,menuT2,menuT3,menuT4,menuT5,menuR,menuQ});
      menu.Items.Insert(0, new ToolStripSeparator());
      menu.Items.Insert(6, new ToolStripSeparator());

      menuT1.Click += new EventHandler(menuT1_Click);
      menuT2.Click += new EventHandler(menuT2_Click);
      menuT3.Click += new EventHandler(menuT3_Click);
      menuT4.Click += new EventHandler(menuT4_Click);
      menuT5.Click += new EventHandler(menuT5_Click);

      menuR.Text = restart1;
      menuR.Click += new EventHandler(menuR_Click);

      menuQ.Text = quit1;
      menuQ.Click += new EventHandler(menuQ_Click);

      // Create the NotifyIcon.
      nIcon = new NotifyIcon(conta);

      // The Icon property sets the icon that will appear
      // in the systray for this application.
      nIcon.Icon = ico0;

      // The ContextMenu property sets the menu that will
      // appear when the systray icon is right clicked.
      nIcon.ContextMenuStrip = menu;

      // The Text property sets the text that will be displayed,
      // in a tooltip, when the mouse hovers over the systray icon.
      nIcon.Text = begin1;
      nIcon.Visible = true;

      nIcon.BalloonTipShown += new EventHandler(nIcon_BalloonTipShown);
      nIcon.BalloonTipClosed += new EventHandler(nIcon_BalloonTipClosed);
      nIcon.BalloonTipClicked += new EventHandler(nIcon_BalloonTipClicked);
      nIcon.MouseClick += new MouseEventHandler(nIcon_Clicked);

      fBold = new Font(menuT1.Font, menuT1.Font.Style | FontStyle.Bold);
      fStd = new Font(menuT1.Font, menuT1.Font.Style);

      param = (string[])args.Clone();
      RunMonitoring();
    }

    // Считать ini-файл
    void ReadIni() {
      if(File.Exists(ini)){
        string[] subs;
        string nam,val;
        i1 = i3 = k1 = k2 = k3 = -1;
        opts = File.ReadAllText(ini).Split(crlf, StringSplitOptions.None);
        for (h=0; h<opts.Length; h++){
          if(opts[h].Length>1) {
            if((byte)opts[h][0]==59) {
              if(k1>=0 && k2<0) k1 = h;     // Всё ещё начало секции Folders
            } else {
              if(i3<0){
                if((byte)opts[h][0]==91){
                  if(opts[h].Length>8) if(opts[h].Substring(1,8)=="Folders]") {
                    k1 = h;
                    i1 = i3 = 0;
                  }
                }else{

                  // Разбор параметров
                  subs = opts[h].Split(rasdy,3,StringSplitOptions.RemoveEmptyEntries);
                  if(subs.Length>1) {
                    nam=subs[0].Trim(empty);
                    val=subs[1].Trim(empty);
                    switch(nam){
                    case "server":
                      imap=val;
                      break;
                    case "port":
                      if(!int.TryParse(val, out port)) port=port1;
                      if(port<1) port = port1;
                      break;
                    case "email":
                      email = val;
                      break;
                    case "passw":
                      k3 = h;
                      passw = val;
                      getPasss();
                      break;
                    case "passs":
                      k3 = h;
                      passs = val;
                      getPassw();
                      break;
                    case "url":
                      url=val;
                      break;
                    case "mins":
                      if(!int.TryParse(val, out mins)) mins=min1;
                      if(mins<1) mins = 1;
                      break;
                    case "begin":
                      begin = val;
                      break;
                    case "checkInterval":
                      checkInterval = val;
                      break;
                    case "notConnect":
                      notConnect = val;
                      break;
                    case "authFailed":
                      authFailed = val;
                      break;
                    case "unseen1":
                      unseen1 = val;
                      break;
                    case "unseen2":
                      unseen2 = val;
                      break;
                    case "unseen5":
                      unseen5 = val;
                      break;
                    case "allSeen":
                      allSeen = val;
                      break;
                    case "min":
                      min = val;
                      break;
                    case "quit":
                      quit = val;
                      break;
                    case "restart":
                      restart = val;
                      break;
                    }
                  }
                }
              }else{
                if((byte)opts[h][0]==91){
                  if(k2<1) k2 = h;          // Начало первой строки после секции Folders
                  i3 = -1;                  // Пошла другая секция
                } else {
                  if(k2<0) k2 = 0;          // Уже не начало секции Folders
                  i1++;                     // количество папок
                }
              }
            }
          } else {
            if(k1>=0 && k2<=0) k2 = h;      // Начало первой строки после секции Folders
          }
        }
        if(k2<k1) k2 = opts.Length;         // Конец секции Folders
      }
    }

    // Раскодировка папок:
    // https://datatracker.ietf.org/doc/html/rfc3501#section-5.1.3
    // за исключением того, что вместо UTF-7 используется UTF-16BE.
    // В Итоге в закодированной строке с именами папок:
    // 1. Выделяем участки начинающиеся с & и заканчивающиеся -
    //    за исключением последовательности &-, которая представляет
    //    собой один символ &.
    // 2. В этих участках заменяем символ , на /.
    // 3. Полученные участки декодируем из формата Base64.
    // 4. Полученные участки декодируем из UTF-16BE в UTF-8.
    // 5. Все другие участки остаются без изменения.
    string ImapToUtf8(string x) {
      k = j = 0;
      string y, z = String.Empty;
      while (j>=0 && k>=0) {
        j = x.IndexOf("&", k);
        if(j>=0){
          z += x.Substring(k, j-k);
          k = x.IndexOf("-", j);
          if(k>j){
            n = j-k;
            if(n==1) {
              z += "&";
            } else {
              j++;
              n = k-j;
              switch(n % 4){
              case 1:
                y="===";
                break;
              case 2:
                y="==";
                break;
              case 3:
                y="=";
                break;
              default:
                y=String.Empty;
                break;
              }
              z += Encoding.GetEncoding(1201).GetString(Convert.FromBase64String(
                   x.Substring(j, n).Replace(",","/")+y));
              k++;
            }
          } else {
            z += x.Substring(j);
          }
        } else {
          z += x.Substring(k);
        }
      }
      return z;
    }

    void writePassw() {
      sw.WriteLine("passs = "+passs);
    }

    void writeOpts(int from, int to) {
      if(from<to) sw.WriteLine(String.Join("\r\n", opts, from, to-from));
    }

    void writeFolders() {
      if(i1>0) for(int i=0; i<i1; i++) sw.WriteLine(ImapToUtf8(folders[i]));
    }

    // Сохранить все изменения в ini-файле, папки в кодировке UTF-8
    void saveIni() {
      if(k1>=0 || k3>=0){
        string bak = ini+".bak";
        try {
          File.Delete(bak);
          File.Move(ini,bak);
        } catch (Exception) {
          save = false;
        }
        if(save) {
          k1++;                             // Надо включить наименование секции
          sw = new StreamWriter(ini, save, Encoding.UTF8);
          int k4 = opts[opts.Length-1].Length==0? opts.Length-1 : opts.Length;
          if(k1>0 && k3>=0) {
            if(k3>k2){
              writeOpts(0, k1);             // Записываем всё от начала до k1
              writeFolders();               // Записываем все папки
              writeOpts(k2, k3);            // Записываем от k2 до k3
              writePassw();                 // Записываем passs
              writeOpts(k3+1, k4);          // Записываем всё от passs до конца
            } else {
              writeOpts(0, k3);             // Записываем всё от начала до k3
              writePassw();                 // Записываем passs
              writeOpts(k3+1, k1);          // Записываем от passs до k1
              writeFolders();               // Записываем все папки
              writeOpts(k2, k4);            // Записываем всё от k2 до конца
            }
          } else if(k1>0) {
            writeOpts(0, k1);               // Записываем всё от начала до k1
            writeFolders();                 // Записываем все папки
            writeOpts(k2, k4);              // Записываем всё от k2 до конца
          } else {
            writeOpts(0, k3);               // Записываем всё от начала до k3
            writePassw();                   // Записываем passs
            writeOpts(k3+1, k4);            // Записываем всё от passs до конца
          }
          sw.Close();
          sw.Dispose();
          save = false;
          File.Delete(bak);
        }
      }
    }

    bool findFolder(string x) {
      for(int w = k1; w<k2; w++) {
        if(opts[w].Length>0)
        if((byte)opts[w][0]!=59)
        if(opts[w].TrimStart(empty).PadRight(x.Length)==x) return true;
      }
      return false;
    }

    void clientClose() {
      if(stream != null) stream.Close();
      if(client != null) client.Close();
    }

    void RunMonitoring() {
       iniMonitoring();
       tMon = new Thread(startMonitoring);
       tMon.Start();
    }

    async Task request(string expect1, string expect2) {
      stream.Write(Encoding.UTF8.GetBytes(message));
      messageData = new StringBuilder();
      answer = 0;
      do {
        try {
          k = await stream.ReadAsync(bytes, 0, bytes.Length);
        } catch (Exception) {
          k = 0;
        }
        message = Encoding.UTF8.GetString(bytes, 0, k);
        messageData.Append(message);
        j = message.LastIndexOf(expect1);
        if(j<0) {
          if(expect2.Length>0) {
            j = message.LastIndexOf(expect2);
            if(j>=0) {
              answer = 2;
              k = 0;
            }
          }
        } else {
          answer = 1;
          k = 0;
        }
      } while (k>0);
    }

    void iniMonitoring() {

      // Присвоение значений по умолчанию
      checkInterval = "Check interval";
      notConnect = notConnect1;
      authFailed = authFailed1;
      unseen5 = String.Empty;
      imap = "imap.mail.ru";
      email = "me@mail.ru";
      restart = restart1;
      unseen1 = unseen_1;
      unseen2 = unseen_2;
      allSeen = allSeen1;
      begin = begin1;
      passw = "123";
      save = false;
      quit = quit1;
      port = port1;
      mins = min1;
      min = "min";

      ReadIni();
      menu.Items.RemoveAt(0);
      menu.Items.Insert(0, new ToolStripLabel(checkInterval));
      menu.Items[0].Font = fBold;
      if (nIcon.Text == begin1) {
        nIcon.Text = begin;
      } else if (nIcon.Text == notConnect1) {
        nIcon.Text = notConnect;
      } else if (nIcon.Text == authFailed1) {
        nIcon.Text = authFailed;
      }
      menuR.Text = restart;
      menuQ.Text = quit;
      updMins();
      i = secs1*1000;
      repeat = mins*2-2;
      if(repeat<1) repeat = 1;
    }

    void seen() {
      nIcon.Icon = ico0;
      nIcon.Text = email+"\n"+allSeen;
    }

    void sleepM() {
      while (notExit && m>i){
        Thread.Sleep(i);
        m -= i;
      }
    }

    async void startMonitoring() {
      while (notExit){
        m = mins*60000;
        client = null;
        stream = null;
        try {
          // Создаем соединение с почтовым сервером
          client = new TcpClient(imap,port);
          stream = new SslStream(client.GetStream(),false);
          stream.AuthenticateAsClient(imap);
          l = notExit;
        } catch (Exception) {
          l = false;
        }
        if(l) {

          // Прочитать информацию
          // Описание альтернативной регистрации через AUTHENTICATE
          // https://new2.intuit.ru/en/studies/courses/116/116/lecture/3367?page=1
          // https://intuit.ru/en/studies/courses/116/116/lecture/3367
          message = "A001 LOGIN "+email+" "+passw+"\r\n";
          if(folders != null) {
            await request("A001 O", "A001 N");
          } else {
            message += "A002 LIST \"\" \"*\"\r\n";
            await request("A002", "A001 N");
          }

          if(answer==1) {
            if(!(folders != null)) {
              Queue<int> iList = new Queue<int>();
              mess = messageData.ToString().Split(crlf, StringSplitOptions.None);
              for(k=1; k<mess.Length; k++) {
                if(mess[k].Length>16) if(mess[k].Substring(0,4)=="* LI") iList.Enqueue(k);
              }

              // Сформировать массив папок в исходной кодировке IMAP.
              nList = iList.Count;
              if(i1<1) {
                i1 = nList;
                save = notExit;
              }
              if(i1>0) {
                u = 0;
                folders = new string[i1];
                while (u<i1 && iList.Count>0) {
                  w = iList.Dequeue();
                  message = mess[w].Substring(mess[w].IndexOf("\"",
                            mess[w].IndexOf("\"")+1)+1).Trim(empty);

                  if(i1 < nList) {
                    if(findFolder(ImapToUtf8(message))) folders[u++] = message;
                  } else {
                    folders[u++] = message;
                  }
                }
              }
            }
            if(save) saveIni();

            // Получить число непрочитанных писем
            message = String.Empty;
            for(w=0; w<folders.Length; w++)
                message += "A"+(w+3).ToString("000")+" STATUS "+folders[w]+" (UNSEEN)\r\n";
            await request("A"+(folders.Length+2).ToString("000"), String.Empty);
            u = 0;
            foreach (string v in messageData.ToString().Split(unseen, StringSplitOptions.None))
            {
              k = v.IndexOf(")");
              if(k>0) if(int.TryParse(v.Substring(0,k), out k)) u += k;
            }
            clientClose();
            n = repeat;
            if(u>0) {
              if(unseen5.Length>0) {
                v = u%10;
                w = u%100;
                if(w<10 || w>20) {
                  switch(v) {
                  case 1:
                    message = unseen1;
                    break;
                  case 2:
                    message = unseen2;
                    break;
                  default:
                    message = unseen5;
                    break;
                  }
                } else {
                  message = unseen5;
                }
              } else {
                if(u>1) {
                  message = unseen2;
                } else {
                  message = unseen1;
                }
              }
              nIcon.Icon = ico1;
              nIcon.Text = email+"\n"+u+" "+message;
            } else {
              seen();
            }

            while (notExit && m>i){
              if(n>0 && u>0) {
                nIcon.ShowBalloonTip(show1, email, u+" "+message, ToolTipIcon.None);
                n--;
              }
              if(notExit) {
                Thread.Sleep(i);
                m -= i;
              }
            }
          } else {
            clientClose();
            nIcon.Text = authFailed;
            nIcon.ShowBalloonTip(show1, email, authFailed, ToolTipIcon.Warning);
            sleepM();
          }
        } else {
          clientClose();
          nIcon.Text = notConnect;
          nIcon.ShowBalloonTip(show1, email, notConnect, ToolTipIcon.Error);
          sleepM();
        }
      }
    }

    void StopMonitoring(){
      notExit = false;
      this.Close();
    }

}
