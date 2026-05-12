using System;
using System.Collections.Generic;
using System.Text;

namespace FormValidationEngine.Core.Validation.Logging
{
    public interface ILogging
    {
        void Info(string message);
        void Warning(string message);
        void Error(string message);
        void Error(Exception exception, string message);
    }
}
