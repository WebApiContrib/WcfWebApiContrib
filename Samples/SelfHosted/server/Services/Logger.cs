using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OperationHandlers;

namespace SelfhostedServer.Services {
    public class Logger : ILogger {
        public void Log(string message) {
            Console.WriteLine(message);
        }
    }
}
