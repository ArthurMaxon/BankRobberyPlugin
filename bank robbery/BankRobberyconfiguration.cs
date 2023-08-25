using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_robbery
{
    public class BankRobberyConfig : IRocketPluginConfiguration
    {
        public int RobberyCooldownMinutes { get; set; }
        public bool PoliceAlertEnabled { get; set; }

        public void LoadDefaults()
        {
            RobberyCooldownMinutes = 60; // Default cooldown time in minutes
            PoliceAlertEnabled = true;   // Default police alert setting
        }
    }
}