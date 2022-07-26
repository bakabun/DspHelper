using System;
using DSharpPlus;
using DSharpPlus.Entities;

namespace Bakabun.DspHelper.Extensions
{
    public static class DiscordGuildExtensions
    {
        public static string GetIconUrl(this DiscordInviteGuild guild, ImageFormat format = ImageFormat.Auto, int size = 256) =>
            GetIconUrl(guild.Id, guild.IconHash, format, size);

        public static string GetIconUrl(this DiscordGuild guild, ImageFormat format = ImageFormat.Auto, int size = 256) =>
            GetIconUrl(guild.Id, guild.IconHash, format, size);

        private static string GetIconUrl(ulong guildId, string iconHash, ImageFormat fmt = ImageFormat.Auto, int size = 256)
        {
            if (iconHash == null) return null;
            var extension = fmt switch
            {
                ImageFormat.Unknown => "",
                ImageFormat.Jpeg => ".jpg",
                ImageFormat.Png => ".png",
                ImageFormat.Gif => ".gif",
                ImageFormat.WebP => ".webp",
                ImageFormat.Auto => iconHash.StartsWith("a_") ? ".gif" : ".jpg",
                _ => throw new ArgumentOutOfRangeException(nameof(fmt), fmt, null)
            };
            return $"https://cdn.discordapp.com/icons/{guildId}/{iconHash}{extension}?size={size}";
        }
    }
}
