using Rocket.API;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Commands;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using UnityEngine;

namespace BankRobberyPlugin
{
    public class BankRobbery : RocketPlugin
    {
        protected override void Load()
        {
            U.Events.OnPlayerConnected += OnPlayerConnected;
        }

        protected override void Unload()
        {
            U.Events.OnPlayerConnected -= OnPlayerConnected;
        }

        private void OnPlayerConnected(UnturnedPlayer player)
        {
            player.Events.OnPlayerChatted += OnPlayerChat;
        }

        private void OnPlayerChat(UnturnedPlayer player, ref Color color, string message, EChatMode chatMode, ref bool cancel)
        {
            string[] args = message.ToLower().Split(' ');

            if (args[0] == "/rob")
            {
                if (IsNearBank(player))
                {
                    StartCoroutine(RobberyTimer(player));
                }
                else
                {
                    UnturnedChat.Say(player, "You are not near a bank to rob.");
                }
            }
            else if (args[0] == "/setbank")
            {
                if (IsNearBank(player))
                {
                    SetBank(player);
                }
                else
                {
                    UnturnedChat.Say(player, "You are not near a bank to set up.");
                }
            }
        }

        private bool IsNearBank(UnturnedPlayer player)
        {
            Vector3 bankLocation = new Vector3(100, 0, 100); // Example bank location
            return Vector3.Distance(player.Position, bankLocation) <= 10; // Player is within 10 meters of the bank
        }

        private System.Collections.IEnumerator RobberyTimer(UnturnedPlayer player)
        {
            UnturnedChat.Say(player, "You started the robbery. You have 60 seconds to complete it.");

            yield return new WaitForSeconds(60); // 60 seconds for example

            AlertPolice(player);
            TransferExperience(player);
        }

        private void AlertPolice(UnturnedPlayer player)
        {
            foreach (var onlinePlayer in Provider.clients)
            {
                var targetPlayer = UnturnedPlayer.FromSteamPlayer(onlinePlayer);

                if (targetPlayer.HasPermission("police"))
                {
                    UnturnedChat.Say(targetPlayer, "Attention all police units: A bank robbery is in progress!");
                }
            }
        }

        private void TransferExperience(UnturnedPlayer player)
        {
            int stolenExperience = UnityEngine.Random.Range(100, 500); // Example stolen experience range
            player.Experience += stolenExperience;
            UnturnedChat.Say(player, $"You successfully stole {stolenExperience} experience!");
        }

        private void SetBank(UnturnedPlayer player)
        {
            int initialBankExperience = 1000; // Example initial bank experience
            player.Experience -= initialBankExperience;
            UnturnedChat.Say(player, $"You set up the bank with {initialBankExperience} experience.");
        }
    }
}



