using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SelfhostedServer {
    public interface ILogger {
        void Log(string message);
    }
}
