using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DSharpPlus;
using DSharpPlus.Entities;

namespace Bakabun.DspHelper.Extensions
{
    public static class EntitiesExtensions
    {
        public static string GetProperName(this DiscordUser user) => $"{user.Username}#{user.Discriminator}";

        public static IEnumerable<DiscordMember> GetMembers(this DiscordRole role)
        {
            var client = (DiscordClient) typeof(DiscordRole)
                .GetProperty("Discord", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(role);
            var guildId = (ulong?) typeof(DiscordRole)
                .GetField("_guild_id", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(role);

            if (client == null || guildId == null) return null;

            var guild = client.Guilds[guildId.GetValueOrDefault()];
            return guild.Members.Values.Where(m => m.Roles.Contains(role));
        }
    }
}
