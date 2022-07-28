using System.Collections.Generic;
using System.IO;
using System.Linq;
using DSharpPlus.Entities;

namespace DspHelper.Extensions
{
    public static class ConverterExtensions
    {
        public static DiscordInteractionResponseBuilder ToInteractionResponseBuilder(this DiscordMessageBuilder builder, bool ephemeral = false) =>
            new DiscordInteractionResponseBuilder(builder).AsEphemeral(ephemeral);

        public static DiscordWebhookBuilder ToWebhookBuilder(this DiscordMessageBuilder builder)
        {
            var webhook = new DiscordWebhookBuilder()
                .WithContent(builder.Content)
                .WithTTS(builder.IsTTS)
                .AddFiles(builder.Files.Select(f => new KeyValuePair<string, Stream>(f.FileName, f.Stream)).ToDictionary(k => k.Key, v => v.Value));

            if (builder.Mentions != null) webhook.AddMentions(builder.Mentions);
            if (builder.Embeds != null) webhook.AddEmbeds(builder.Embeds);
            if (builder.Components != null) webhook.AddComponents(builder.Components);

            return webhook;
        }

        public static DiscordFollowupMessageBuilder ToFollowupMessageBuilder(this DiscordMessageBuilder builder, bool ephemeral = false)
        {
            var fub = new DiscordFollowupMessageBuilder()
                .WithContent(builder.Content)
                .WithTTS(builder.IsTTS)
                .AsEphemeral(ephemeral)
                .AddFiles(builder.Files.Select(f => new KeyValuePair<string, Stream>(f.FileName, f.Stream)).ToDictionary(k => k.Key, v => v.Value));

            if (builder.Mentions != null) fub.AddMentions(builder.Mentions);
            if (builder.Embeds != null) fub.AddEmbeds(builder.Embeds);
            if (builder.Components != null) fub.AddComponents(builder.Components);

            return fub;
        }
    }
}
