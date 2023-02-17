﻿namespace MobileBasedCashFlowAPI.Settings
{
    public class MailSettings
    {
        public string From { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; } = int.MaxValue;
    }
}
