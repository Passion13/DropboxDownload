using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestProjectDemo.Controllers
{
    public class BatchController : Controller
    {
        //
        // GET: /Batch/

        public ActionResult Execution()
        {
            int exitCode;
            ProcessStartInfo processInfo;
            Process process;
            string command = @"C:\Users\anantpithadiya\Documents\Visual Studio 2012\Projects\TestProjectDemo\TestProjectDemo\Content\Module.bat";
            processInfo = new ProcessStartInfo("cmd.exe",command);
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            // *** Redirect the output ***
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;

            process = Process.Start(processInfo);
            process.WaitForExit();

            // *** Read the streams ***
            // Warning: This approach can lead to deadlocks, see Edit #2
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            exitCode = process.ExitCode;

            Console.WriteLine("output>>" + (String.IsNullOrEmpty(output) ? "(none)" : output));
            Console.WriteLine("error>>" + (String.IsNullOrEmpty(error) ? "(none)" : error));
            Console.WriteLine("ExitCode: " + exitCode.ToString(), "ExecuteCommand");
            process.Close();
            return View();
        }
       
    }
}
