//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
//!!!                                                   !!!
//!!!  notifier.net на C#.        Автор: A.Б.Корниенко  !!!
//!!!  v0.3.0.0                             01.12.2025  !!!
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
          notifier1 = "notifier.net.", CRLF = "\r\n";
    const int secs1=30, show1=6100, min1=5, port1=993;
    const byte b27 = 27, b53 = 53, b64 = 64, b70 = 70, b91 = 91;
    static string Folder = Thread.GetDomain().BaseDirectory;
    static string exe=Folder+notifier1+"exe";
    string auto = Environment.GetFolderPath(Environment.SpecialFolder.Startup)+"\\"+
           notifier1+"url", ini=Folder+notifier1+"ini", imap, email, passw, passs, url,
           message, min, authFailed, notConnect, checkInterval, unseen1, unseen2, unseen5,
           allSeen, begin, restart, quit;
    char[] empty = new Char[] {' ','\t'};
    char[] rasdy = new Char[] {'=',';'};
    int i, j, k, n, m, p, q, r, h, u, v, w, i1, i2, i3, k1, k2, k3, nList,
        port, mins, repeat, t1, t2, t3, t4, t5;
    byte[] bb, bs, b1, b2 = new Byte[2], bytes = new byte[2048];
    bool l, save, showErrors, notExit = true;
    string[] param, opts, mess, folders, crlf = new string[] { CRLF },
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
        0,0,0,0,0,0,0,0,0,0,0,0,0,184,176,158,180,184,176,158,255,184,176,158,255,184,176,
        158,255,184,176,158,255,184,176,158,255,184,176,158,255,184,176,158,255,184,176,158,
        255,184,176,158,255,184,176,158,255,184,176,158,255,184,176,158,255,184,176,158,255,
        184,176,158,255,184,176,158,255,184,176,158,255,184,176,158,255,184,176,158,255,184,
        176,158,255,184,176,158,255,184,176,158,255,184,176,158,255,184,176,158,255,184,176,
        158,255,184,176,158,180,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,54,67,244,40,54,67,244,179,
        54,67,244,247,58,70,244,254,184,184,217,255,220,216,207,255,220,216,207,255,220,216,
        207,255,220,216,207,255,220,216,207,255,220,216,207,255,220,216,207,255,220,216,207,
        255,220,216,207,255,220,216,207,255,220,216,207,255,220,216,207,255,220,216,207,255,
        220,216,207,255,220,216,207,255,220,216,207,255,220,216,207,255,220,216,207,255,220,
        216,207,255,220,216,207,255,183,183,216,255,58,70,244,254,54,67,244,247,54,67,244,
        179,54,67,244,40,0,0,0,0,54,67,244,40,54,67,244,243,54,67,244,255,54,67,244,255,54,
        67,244,255,182,185,244,255,226,223,216,255,220,216,207,255,220,216,207,255,220,216,
        207,255,220,216,207,255,220,216,207,255,220,216,207,255,220,216,207,255,220,216,207,
        255,220,216,207,255,220,216,207,255,220,216,207,255,220,216,207,255,220,216,207,255,
        220,216,207,255,220,216,207,255,220,216,207,255,220,216,207,255,220,216,207,255,223,
        220,213,255,175,179,237,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,243,
        54,67,244,40,54,67,244,179,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,
        185,189,248,255,249,249,249,255,235,233,228,255,220,216,208,255,220,216,207,255,220,
        216,207,255,220,216,207,255,220,216,207,255,220,216,207,255,220,216,207,255,220,216,
        207,255,220,216,207,255,220,216,207,255,220,216,207,255,220,216,207,255,220,216,207,
        255,220,216,207,255,220,216,207,255,220,216,207,255,228,226,221,255,237,237,237,255,
        177,181,240,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,
        179,54,67,244,247,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,185,189,
        248,255,250,250,250,255,250,250,250,255,243,242,240,255,224,221,213,255,220,216,207,
        255,220,216,207,255,220,216,207,255,220,216,207,255,220,216,207,255,220,216,207,255,
        220,216,207,255,220,216,207,255,220,216,207,255,220,216,207,255,220,216,207,255,220,
        216,207,255,222,219,211,255,233,233,230,255,238,238,238,255,238,238,238,255,177,181,
        240,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,247,54,
        67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,185,189,248,255,
        250,250,250,255,250,250,250,255,250,250,250,255,248,248,247,255,232,230,224,255,220,
        216,207,255,220,216,207,255,220,216,207,255,220,216,207,255,220,216,207,255,220,216,
        207,255,220,216,207,255,220,216,207,255,220,216,207,255,220,216,207,255,226,223,217,
        255,237,236,236,255,238,238,238,255,238,238,238,255,238,238,238,255,177,181,240,255,
        54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,
        54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,185,189,248,255,250,250,250,
        255,250,250,250,255,250,250,250,255,250,250,250,255,250,250,250,255,240,239,236,255,
        222,219,210,255,220,216,207,255,220,216,207,255,220,216,207,255,220,216,207,255,220,
        216,207,255,220,216,207,255,221,217,209,255,231,230,227,255,238,238,238,255,238,238,
        238,255,238,238,238,255,238,238,238,255,238,238,238,255,177,181,240,255,54,67,244,
        255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,
        255,54,67,244,255,54,67,244,255,54,67,244,255,185,189,248,255,250,250,250,255,250,
        250,250,255,250,250,250,255,250,250,250,255,250,250,250,255,250,250,250,255,247,246,
        245,255,229,226,220,255,220,216,207,255,220,216,207,255,220,216,207,255,220,216,207,
        255,225,222,215,255,236,236,235,255,238,238,238,255,238,238,238,255,238,238,238,255,
        238,238,238,255,238,238,238,255,238,238,238,255,177,181,240,255,54,67,244,255,54,
        67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,
        54,67,244,255,54,67,244,255,54,67,244,255,185,189,248,255,250,250,250,255,250,250,
        250,255,250,250,250,255,250,250,250,255,250,250,250,255,250,250,250,255,250,250,250,
        255,250,250,250,255,237,235,231,255,221,217,208,255,221,217,208,255,229,227,222,255,
        238,238,238,255,238,238,238,255,238,238,238,255,238,238,238,255,238,238,238,255,238,
        238,238,255,238,238,238,255,238,238,238,255,177,181,240,255,54,67,244,255,54,67,244,
        255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,
        255,54,67,244,255,54,67,244,255,185,189,248,255,250,250,250,255,250,250,250,255,250,
        250,250,255,250,250,250,255,250,250,250,255,250,250,250,255,250,250,250,255,250,250,
        250,255,250,250,250,255,217,219,241,255,211,213,235,255,238,238,238,255,238,238,238,
        255,238,238,238,255,238,238,238,255,238,238,238,255,238,238,238,255,238,238,238,255,
        238,238,238,255,238,238,238,255,177,181,240,255,54,67,244,255,54,67,244,255,54,67,
        244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,
        67,244,255,54,67,244,255,185,189,248,255,250,250,250,255,250,250,250,255,250,250,
        250,255,250,250,250,255,250,250,250,255,250,250,250,255,250,250,250,255,249,249,250,
        255,172,177,248,255,63,75,244,255,59,72,243,255,152,158,241,255,236,237,238,255,238,
        238,238,255,238,238,238,255,238,238,238,255,238,238,238,255,238,238,238,255,238,238,
        238,255,238,238,238,255,177,181,240,255,54,67,244,255,54,67,244,255,54,67,244,255,
        54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,
        54,67,244,255,185,189,248,255,250,250,250,255,250,250,250,255,250,250,250,255,250,
        250,250,255,250,250,250,255,250,250,250,255,233,234,249,255,113,122,246,255,54,67,
        244,255,54,67,244,255,54,67,244,255,54,67,244,255,103,113,242,255,220,221,239,255,
        238,238,238,255,238,238,238,255,238,238,238,255,238,238,238,255,238,238,238,255,238,
        238,238,255,177,181,240,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,
        54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,
        185,189,248,255,250,250,250,255,250,250,250,255,250,250,250,255,250,250,250,255,250,
        250,250,255,194,198,248,255,74,85,245,255,54,67,244,255,54,67,244,255,54,67,244,255,
        54,67,244,255,54,67,244,255,54,67,244,255,70,81,244,255,181,185,240,255,238,238,238,
        255,238,238,238,255,238,238,238,255,238,238,238,255,238,238,238,255,177,181,240,255,
        54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,
        54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,185,189,248,255,250,250,250,
        255,250,250,250,255,250,250,250,255,243,243,250,255,139,146,247,255,55,68,244,255,
        54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,
        54,67,244,255,54,67,244,255,54,67,244,255,121,130,242,255,230,230,238,255,238,238,
        238,255,238,238,238,255,238,238,238,255,177,181,240,255,54,67,244,255,54,67,244,255,
        54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,
        54,67,244,255,54,67,244,255,185,189,248,255,250,250,250,255,250,250,250,255,210,213,
        249,255,83,94,245,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,
        67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,
        54,67,244,255,54,67,244,255,78,89,243,255,195,198,239,255,238,238,238,255,238,238,
        238,255,177,181,240,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,
        67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,
        185,189,248,255,248,248,250,255,155,161,247,255,58,71,244,255,54,67,244,255,54,67,
        244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,58,71,244,255,58,
        71,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,
        54,67,244,255,58,71,244,255,149,155,241,255,236,236,238,255,177,181,240,255,54,67,
        244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,
        67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,161,166,247,255,102,112,246,
        255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,
        255,54,67,244,255,76,87,243,255,197,199,237,255,203,204,238,255,80,91,243,255,54,
        67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,
        54,67,244,255,94,104,242,255,151,158,241,255,54,67,244,255,54,67,244,255,54,67,244,
        255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,
        255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,
        255,54,67,244,255,54,67,244,255,54,67,244,255,115,124,242,255,229,228,236,255,241,
        239,236,255,241,239,236,255,229,228,236,255,115,124,242,255,54,67,244,255,54,67,244,
        255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,
        255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,
        255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,
        255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,60,72,243,255,160,164,
        239,255,241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,
        255,241,239,236,255,172,176,239,255,61,73,243,255,54,67,244,255,54,67,244,255,54,
        67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,
        54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,
        54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,
        54,67,244,255,83,94,243,255,207,207,237,255,241,239,236,255,241,239,236,255,241,239,
        236,255,241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,
        255,210,211,237,255,87,97,243,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,
        244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,
        67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,
        54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,122,130,241,255,233,231,236,
        255,241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,255,
        241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,255,234,
        233,237,255,135,141,241,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,
        54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,
        54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,
        63,76,244,255,174,178,239,255,241,239,236,255,241,239,236,255,241,239,236,255,241,
        239,236,255,241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,255,241,239,
        236,255,241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,
        255,174,178,239,255,63,76,244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,
        244,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,244,254,54,67,244,255,54,
        67,244,255,54,67,244,255,54,67,244,255,54,67,244,255,90,101,242,255,214,214,237,255,
        241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,255,241,
        239,236,255,241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,255,241,239,
        236,255,241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,
        255,218,217,237,255,95,104,242,255,54,67,244,255,54,67,244,255,54,67,244,255,54,67,
        244,255,54,67,244,255,54,67,244,254,54,67,244,206,54,67,244,255,54,67,244,255,54,
        67,244,255,54,67,244,255,128,136,240,255,235,234,237,255,241,239,236,255,241,239,
        236,255,241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,
        255,241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,255,
        241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,255,241,
        239,236,255,237,235,236,255,141,148,240,255,55,68,244,255,54,67,244,255,54,67,244,
        255,54,67,244,255,54,67,244,206,54,67,244,8,54,67,244,206,54,67,244,254,72,83,244,
        254,184,186,239,255,241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,255,
        241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,255,241,
        239,236,255,241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,255,241,239,
        236,255,241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,255,241,239,236,
        255,241,239,236,255,241,239,236,255,184,186,239,255,72,83,243,255,54,67,244,254,54,
        67,244,206,54,67,244,8,0,0,0,0,0,0,0,0,0,0,0,0,184,176,158,180,184,176,158,180,184,
        176,158,180,184,176,158,180,184,176,158,180,184,176,158,180,184,176,158,180,184,176,
        158,180,184,176,158,180,184,176,158,180,184,176,158,180,184,176,158,180,184,176,158,
        180,184,176,158,180,184,176,158,180,184,176,158,180,184,176,158,180,184,176,158,180,
        184,176,158,180,184,176,158,180,184,176,158,180,184,176,158,180,184,176,158,180,184,
        176,158,180,184,176,158,180,184,176,158,180,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,184,176,158,180,184,176,158,255,184,176,158,255,184,176,158,255,184,176,
        158,255,184,176,158,255,184,176,158,255,184,176,158,255,184,176,158,255,184,176,158,
        255,184,176,158,255,184,176,158,255,184,176,158,255,184,176,158,255,184,176,158,255,
        184,176,158,255,184,176,158,255,184,176,158,255,184,176,158,255,184,176,158,255,184,
        176,158,255,184,176,158,255,184,176,158,255,184,176,158,255,184,176,158,255,184,176,
        158,180,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,255,255,255,255,255,255,255,255,
        255,255,255,255,224,0,0,7,128,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,224,0,0,7,224,0,0,7,255,255,
        255,255,255,255,255,255
    };
    Icon ico0 = Icon.ExtractAssociatedIcon(exe),
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
      l = autoDelete();
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
                    case "showErrors":
                      showErrors = (val!="0" && 
                        val.IndexOf("No",StringComparison.OrdinalIgnoreCase)!=0);
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
      if(from<to) sw.WriteLine(String.Join(CRLF, opts, from, to-from));
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
       autoRunLnk();
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

    // Если необходимо, то создать ярлык в папке Startup
    void autoRunLnk() {
      l = File.Exists(auto);
      exe = exe.Replace("\\", "/");
      if(l) {
        try {
          l = File.ReadAllText(auto).Contains(exe);
        } catch (Exception) {
          l = true;
        }
      }
      if(!l) if(autoDelete()) {
        File.WriteAllText(auto, "[InternetShortcut]"+CRLF+"URL=file:///"+exe+CRLF+
            "IconFile="+exe+CRLF+"IconIndex=0"+CRLF);
      }
    }

    // Удалить ярлык из папки Startup
    bool autoDelete() {
      try {
        File.Delete(auto);
      } catch (Exception) {
        return false;
      }
      return true;
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
          message = "A001 LOGIN "+email+" "+passw+CRLF;
          if(folders != null) {
            await request("A001 O", "A001 N");
          } else {
            message += "A002 LIST \"\" \"*\""+CRLF;
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
                message += "A"+(w+3).ToString("000")+" STATUS "+folders[w]+" (UNSEEN)"+CRLF;
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
          if(showErrors){
            nIcon.Text = notConnect;
            nIcon.ShowBalloonTip(show1, email, notConnect, ToolTipIcon.Error);
          }
          sleepM();
        }
      }
    }

    void StopMonitoring(){
      notExit = false;
      this.Close();
    }

}
