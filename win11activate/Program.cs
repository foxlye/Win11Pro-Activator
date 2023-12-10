using Microsoft.Win32;
using System;
using System.Diagnostics;

namespace win11activate
{
    class Program
    {
        static int get_state()
        {
            try
            {
                return Convert.ToInt32(Registry.CurrentUser.CreateSubKey("Control Panel\\Desktop").GetValue("Activator_state"));
            }
            catch(Exception e)
            {
                return 0;
            }
           
        }
        
        
        static void Main(string[] args)
        {
            logger("Welcome to Shlindows 11 Activator");

            int current_state = get_state();

            switch (current_state)
            {
                case 0:
                    StepOne();
                    break;

                case 1:
                    StepSecond();
                    break;

                default: logger("Error state"); break;
            }

            Console.ReadKey();
        }




        private static void StepOne()
        {
            logger("FIRST ACTION");
            logger("Step 1");
            string text = cmdController("cscript /nologo C:\\Windows\\System32\\slmgr.vbs -rearm");
            bool flag = !text.Contains("Access denied") && !text.Contains("0xC004D302");
            if (flag)
            {
                logger("Step 2");
                Registry.CurrentUser.CreateSubKey("Control Panel\\Desktop").SetValue("PaintDesktopVersion", 4);
                logger("Step 3");
                cmdController("gpupdate");
                logger("Step 4");
                Registry.LocalMachine.CreateSubKey("SYSTEM\\CurrentControlSet\\Services\\svsvc\\KMS").SetValue("", "kms_4");
                logger("Step 5");
                Registry.LocalMachine.CreateSubKey("SYSTEM\\CurrentControlSet\\Services\\svsvc").SetValue("Start", 4);
                logger("Step 6");
                cmdController(string.Format("reg add {0}HKLM\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\SoftwareProtectionPlatform{1} /v SkipRearm /t REG_DWORD /d 1 /reg:64 /f", '"', '"'));
                logger("Step 7");
                cmdController("gpupdate");

                Registry.CurrentUser.CreateSubKey("Control Panel\\Desktop").SetValue("Activator_state", 1);
                logger("First action completed. Restart PC and run Activator.");
            }
            else
            {
                bool flag2 = text.Contains("0xC004D302");
                if (flag2)
                {
                    logger("Failed activation shlindows Code: 0xC004D302");
                }
                bool flag3 = text.Contains("Access denied");
                if (flag3)
                {
                    logger("Restart with admin privilegies");
                }
            }
        }

        private static void StepSecond()
        {
            logger("SECOND ACTION");
            logger("Step 1");
            string text = cmdController("cscript /nologo C:\\Windows\\System32\\slmgr.vbs -rearm");
            bool flag = !text.Contains("Access denied") && !text.Contains("0xC004D302");
            if (flag)
            {
                logger("Step 2");
                logger("Activating Shlindows 11 ...");
                cmdController("cscript /nologo C:\\Windows\\System32\\slmgr.vbs /ipk W269N-WFGWX-YVC9B-4J6C9-T83GX");
                logger("Step 3");
                cmdController("cscript /nologo C:\\Windows\\System32\\slmgr.vbs /skms kms8.msguides.com");
                cmdController("cscript /nologo C:\\Windows\\System32\\slmgr.vbs /ato");


                Registry.CurrentUser.CreateSubKey("Control Panel\\Desktop").SetValue("PaintDesktopVersion", 0);
                Registry.CurrentUser.CreateSubKey("Control Panel\\Desktop").SetValue("Activator_state", 0);
                logger("Shlindows 11 successfully activated. Restart PC.");
            }
            else
            {
                bool flag2 = text.Contains("0xC004D302");
                if (flag2)
                {
                    logger("Failed activation shlindows Code: 0xC004D302");
                }
                bool flag3 = text.Contains("Access denied");
                if (flag3)
                {
                    logger("Restart with admin privilegies");
                }
            }
        }

        private static string cmdController(string command)
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.Start();
            process.StandardInput.WriteLine(command);
            process.StandardInput.Flush();
            process.StandardInput.Close();
            return process.StandardOutput.ReadToEnd();
        }

        private static void logger(string _log)
        {
            Console.WriteLine($"[github.com/foxlye] {_log}");
        }
    }
}
