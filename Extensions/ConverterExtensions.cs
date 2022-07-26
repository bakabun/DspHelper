using System.Collections.Generic;
using System.IO;
using System.Linq;
using DSharpPlus.Entities;

namespace DspHelper.Extensions
{
    public static class ConverterExtensions
    {
        public static DiscordInteractionResponseBuilder ToInteractionResponseBuilder(this DiscordMessageBuilder message, bool ephemeral = false) =>
            new DiscordInteractionResponseBuilder(message).AsEphemeral(ephemeral);

        public static DiscordWebhookBuilder ToWebhookBuilder(this DiscordMessageBuilder message)
        {
            var builder = new DiscordWebhookBuilder()
                .WithContent(message.Content)
                .WithTTS(message.IsTTS)
                .AddFiles(message.Files.Select(f => new KeyValuePair<string, Stream>(f.FileName, f.Stream)).ToDictionary(k => k.Key, v => v.Value));

            if (message.Mentions != null) builder.AddMentions(message.Mentions);
            if (message.Embeds != null) builder.AddEmbeds(message.Embeds);
            if (message.Components != null) builder.AddComponents(message.Components);

            return builder;
        }

        public static DiscordFollowupMessageBuilder ToFollowupMessageBuilder(this DiscordMessageBuilder message, bool ephemeral = false)
        {
            var builder = new DiscordFollowupMessageBuilder()
                .WithContent(message.Content)
                .WithTTS(message.IsTTS)
                .AsEphemeral(ephemeral)
                .AddFiles(message.Files.Select(f => new KeyValuePair<string, Stream>(f.FileName, f.Stream)).ToDictionary(k => k.Key, v => v.Value));

            if (message.Mentions != null) builder.AddMentions(message.Mentions);
            if (message.Embeds != null) builder.AddEmbeds(message.Embeds);
            if (message.Components != null) builder.AddComponents(message.Components);

            return builder;
        }
    }
}
