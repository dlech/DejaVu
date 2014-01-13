// Siarhei Arkhipenka (c) 2006-2007. email: sbs-arhipenko@yandex.ru

using System;
using System.Windows.Forms;

namespace UndoRedoSample
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new DemoForm());
        }
    }
} 