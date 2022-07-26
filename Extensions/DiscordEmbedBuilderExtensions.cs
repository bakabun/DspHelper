using System;
using System.Collections.Generic;
using DSharpPlus;
using DSharpPlus.Entities;

namespace Bakabun.DspHelper.Extensions
{
    public static class DiscordEmbedBuilderExtensions
    {
        public static DiscordEmbedBuilder WithAuthor(this DiscordEmbedBuilder builder, DiscordUser user) =>
            builder.WithAuthor(user.GetProperName(), iconUrl: user.GetAvatarUrl(ImageFormat.Auto, 128));

        public static DiscordEmbedBuilder WithCurrentTimestamp(this DiscordEmbedBuilder builder) =>
            builder.WithTimestamp(DateTimeOffset.Now);

        public static DiscordEmbedBuilder AddField(this DiscordEmbedBuilder builder, string name, object value, bool inline = false) =>
            builder.AddField(name, value.ToString(), inline);

        public static DiscordEmbedBuilder WithFields(this DiscordEmbedBuilder builder,
            IEnumerable<KeyValuePair<string, string>> values, bool isInline = false)
        {
            foreach (var (key, value) in values)
                builder.AddField(key, value, isInline);
            return builder;
        }
    }
}
