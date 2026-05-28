using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab3_SO
{
    public partial class Form1 : Form
    {
        IntPtr hThread1 = IntPtr.Zero;//handle catre thread
        IntPtr hThread2 = IntPtr.Zero;
        IntPtr hThread3 = IntPtr.Zero;
        IntPtr ev1, ev2, ev3;

        class ThreadData//clasa pentru a transmite mai multe date catre thread ce progress bar sa modifice si ce delay sa aiba
        {
            public ProgressBar ProgressBar { get; set; }
            public uint Delay { get; set; }
        }

        public Form1()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;//flag pentru a putea modifica controalele din threaduri diferite de cel principal

            progressBar1.Minimum = 0;
            progressBar1.Maximum = 1000;

            progressBar2.Minimum = 0;
            progressBar2.Maximum = 1000;

            progressBar3.Minimum = 0;
            progressBar3.Maximum = 1000;

            button1.Text = "Start";
            button2.Text = "Start";
            button3.Text = "Start";


        }

        public void Delay(uint ms)
        {
            var start = Environment.TickCount;

            while (Environment.TickCount - start < ms)
            {
            }
        }

        public uint ThreadFunc(IntPtr p)
        {
            GCHandle handle = (GCHandle)p;//convertim inapoi la GCHandle pentru a putea accesa datele transmise catre thread
            ThreadData data = (ThreadData)handle.Target;//accesam datele transmise catre thread

            while (data.ProgressBar.Value < data.ProgressBar.Maximum)
            {
                data.ProgressBar.Value += 1;
                Delay(data.Delay);
            }

            handle.Free();
            return 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            uint id;

            if (hThread1 == IntPtr.Zero)
            {
                ThreadData data1 = new ThreadData
                {
                    ProgressBar = progressBar1,
                    Delay = 60
                };

                GCHandle gch = GCHandle.Alloc(data1);//alocam datele pentru a le putea transmite catre thread

                WinApiClass.LPTHREAD_START_ROUTINE t1 =
                    new WinApiClass.LPTHREAD_START_ROUTINE(ThreadFunc);

                hThread1 = (IntPtr)WinApiClass.CreateThread(
                    IntPtr.Zero,
                    0,
                    t1,
                    (IntPtr)gch,
                    WinApiClass.ThreadState.RUN,
                    out id);

                button1.Text = "Suspend";
                return;
            }

            WinApiClass.SetThreadPriority(hThread1, WinApiClass.ThreadPriority.THREAD_PRIORITY_HIGHEST);

            if (button1.Text == "Suspend")
            {
                WinApiClass.SuspendThread(hThread1);
                button1.Text = "Resume";
            }
            else
            {
                WinApiClass.ResumeThread(hThread1);
                button1.Text = "Suspend";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (hThread1 == IntPtr.Zero)//verificam daca threadul a fost creat inainte de a incerca sa obtinem informatii despre el
            {
                textBox1.Text = "Thread 1 nu a fost creat.";
                return;
            }

            WinApiClass.FILETIME creation, exit, kernel, user;//structuri pentru a stoca informatiile despre thread

            bool ok = WinApiClass.GetThreadTimes(hThread1, out creation, out exit, out kernel, out user);//obtinem informatiile despre thread

            if (!ok)
            {
                textBox1.Text = "Eroare la GetThreadTimes pentru Thread 1.";
                return;
            }

            WinApiClass.SYSTEMTIME stCreation;//structura pentru a stoca timpul de creare al threadului
            WinApiClass.FileTimeToSystemTime(ref creation, out stCreation);//convertim timpul de creare din FILETIME in SYSTEMTIME pentru a-l putea afisa

            long kernelVal = ((long)kernel.DateTimeHigh << 32) | kernel.DateTimeLow;//calculam timpul petrecut in kernel si user in milisecunde
            long userVal = ((long)user.DateTimeHigh << 32) | user.DateTimeLow;

            string exitText;//verificam daca threadul a iesit sau inca ruleaza pentru a afisa informatia corespunzatoare
            if (exit.DateTimeLow == 0 && exit.DateTimeHigh == 0)//daca timpul de exit este 0 inseamna ca threadul inca ruleaza
            {
                exitText = "Thread inca ruleaza";
            }
            else
            {
                WinApiClass.SYSTEMTIME stExit;
                WinApiClass.FileTimeToSystemTime(ref exit, out stExit);
                exitText = $"{stExit.Hour:D2}:{stExit.Minute:D2}:{stExit.Second:D2}.{stExit.Milliseconds:D3}";
            }

            textBox1.Text =
                "Thread 1\r\n" +
                $"Creation: {stCreation.Hour:D2}:{stCreation.Minute:D2}:{stCreation.Second:D2}.{stCreation.Milliseconds:D3}\r\n" +
                $"Exit: {exitText}\r\n" +
                $"Kernel: {kernelVal / 10000.0:F2} ms\r\n" +
                $"User: {userVal / 10000.0:F2} ms";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            uint id;

            if (hThread2 == IntPtr.Zero)
            {
                ThreadData data2 = new ThreadData
                {
                    ProgressBar = progressBar2,
                    Delay = 20
                };

                GCHandle gch = GCHandle.Alloc(data2);

                WinApiClass.LPTHREAD_START_ROUTINE t2 =
                    new WinApiClass.LPTHREAD_START_ROUTINE(ThreadFunc);

                hThread2 = (IntPtr)WinApiClass.CreateThread(
                    IntPtr.Zero,
                    0,
                    t2,
                    (IntPtr)gch,
                    WinApiClass.ThreadState.RUN,
                    out id);

                button2.Text = "Suspend";
                return;
            }

            WinApiClass.SetThreadPriority(
            hThread2,
            WinApiClass.ThreadPriority.THREAD_PRIORITY_NORMAL
            );

            if (button2.Text == "Suspend")
            {
                WinApiClass.SuspendThread(hThread2);
                button2.Text = "Resume";
            }
            else
            {
                WinApiClass.ResumeThread(hThread2);
                button2.Text = "Suspend";
            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            if (hThread2 == IntPtr.Zero)
            {
                textBox1.Text = "Thread 2 nu a fost creat.";
                return;
            }

            WinApiClass.FILETIME creation, exit, kernel, user;

            bool ok = WinApiClass.GetThreadTimes(hThread2, out creation, out exit, out kernel, out user);

            if (!ok)
            {
                textBox1.Text = "Eroare la GetThreadTimes pentru Thread 2.";
                return;
            }

            WinApiClass.SYSTEMTIME stCreation;
            WinApiClass.FileTimeToSystemTime(ref creation, out stCreation);

            long kernelVal = ((long)kernel.DateTimeHigh << 32) | kernel.DateTimeLow;
            long userVal = ((long)user.DateTimeHigh << 32) | user.DateTimeLow;

            string exitText;
            if (exit.DateTimeLow == 0 && exit.DateTimeHigh == 0)
            {
                exitText = "Thread inca ruleaza";
            }
            else
            {
                WinApiClass.SYSTEMTIME stExit;
                WinApiClass.FileTimeToSystemTime(ref exit, out stExit);
                exitText = $"{stExit.Hour:D2}:{stExit.Minute:D2}:{stExit.Second:D2}.{stExit.Milliseconds:D3}";
            }

            textBox1.Text =
                "Thread 2\r\n" +
                $"Creation: {stCreation.Hour:D2}:{stCreation.Minute:D2}:{stCreation.Second:D2}.{stCreation.Milliseconds:D3}\r\n" +
                $"Exit: {exitText}\r\n" +
                $"Kernel: {kernelVal / 10000.0:F2} ms\r\n" +
                $"User: {userVal / 10000.0:F2} ms";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            uint id;

            if (hThread3 == IntPtr.Zero)
            {
                ThreadData data3 = new ThreadData
                {
                    ProgressBar = progressBar3,
                    Delay = 80
                };

                GCHandle gch = GCHandle.Alloc(data3);

                WinApiClass.LPTHREAD_START_ROUTINE t3 =
                    new WinApiClass.LPTHREAD_START_ROUTINE(ThreadFunc);

                hThread3 = (IntPtr)WinApiClass.CreateThread(
                    IntPtr.Zero,
                    0,
                    t3,
                    (IntPtr)gch,
                    WinApiClass.ThreadState.RUN,
                    out id);

                button3.Text = "Suspend";
                return;
            }

            WinApiClass.SetThreadPriority(
            hThread3,
            WinApiClass.ThreadPriority.THREAD_PRIORITY_LOWEST
            );

            if (button3.Text == "Suspend")
            {
                WinApiClass.SuspendThread(hThread3);
                button3.Text = "Resume";
            }
            else
            {
                WinApiClass.ResumeThread(hThread3);
                button3.Text = "Suspend";
            }
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            if (hThread3 == IntPtr.Zero)
            {
                textBox1.Text = "Thread 3 nu a fost creat.";
                return;
            }

            WinApiClass.FILETIME creation, exit, kernel, user;

            bool ok = WinApiClass.GetThreadTimes(hThread3, out creation, out exit, out kernel, out user);

            if (!ok)
            {
                textBox1.Text = "Eroare la GetThreadTimes pentru Thread 3.";
                return;
            }

            WinApiClass.SYSTEMTIME stCreation;
            WinApiClass.FileTimeToSystemTime(ref creation, out stCreation);

            long kernelVal = ((long)kernel.DateTimeHigh << 32) | kernel.DateTimeLow;
            long userVal = ((long)user.DateTimeHigh << 32) | user.DateTimeLow;

            string exitText;
            if (exit.DateTimeLow == 0 && exit.DateTimeHigh == 0)
            {
                exitText = "Thread inca ruleaza";
            }
            else
            {
                WinApiClass.SYSTEMTIME stExit;
                WinApiClass.FileTimeToSystemTime(ref exit, out stExit);
                exitText = $"{stExit.Hour:D2}:{stExit.Minute:D2}:{stExit.Second:D2}.{stExit.Milliseconds:D3}";
            }

            textBox1.Text =
                "Thread 3\r\n" +
                $"Creation: {stCreation.Hour:D2}:{stCreation.Minute:D2}:{stCreation.Second:D2}.{stCreation.Milliseconds:D3}\r\n" +
                $"Exit: {exitText}\r\n" +
                $"Kernel: {kernelVal / 10000.0:F2} ms\r\n" +
                $"User: {userVal / 10000.0:F2} ms";
        }
    }
}
