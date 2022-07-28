using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace DspHelper.Extensions
{
    public static class ApplicationCommandsContextExtensions
    {
        public static Task<DiscordMessage> EditResponseAsync(this BaseContext ctx, string content, IEnumerable<DiscordAttachment> attachments = null) => 
            ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent(content), attachments);

        public static Task<DiscordMessage> EditResponseAsync(this BaseContext ctx, DiscordEmbed embed, IEnumerable<DiscordAttachment> attachments = null) => 
            ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed), attachments);

        public static Task<DiscordMessage> EditResponseAsync(this BaseContext ctx, string content, DiscordEmbed embed, IEnumerable<DiscordAttachment> attachments = null) => 
            ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent(content).AddEmbed(embed), attachments);

        public static Task<DiscordMessage> FollowUpAsync(this BaseContext ctx, string content, bool ephemeral = false) => 
            ctx.FollowUpAsync(new DiscordFollowupMessageBuilder().WithContent(content).AsEphemeral(ephemeral));

        public static Task<DiscordMessage> FollowUpAsync(this BaseContext ctx, DiscordEmbed embed, bool ephemeral = false) => 
            ctx.FollowUpAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed).AsEphemeral(ephemeral));

        public static Task<DiscordMessage> FollowUpAsync(this BaseContext ctx, string content, DiscordEmbed embed, bool ephemeral = false) => 
            ctx.FollowUpAsync(new DiscordFollowupMessageBuilder().WithContent(content).AddEmbed(embed).AsEphemeral(ephemeral));

        public static Task<DiscordMessage> EditFollowupAsync(this BaseContext ctx, ulong followupMessageId, string content, IEnumerable<DiscordAttachment> attachments = null) => 
            ctx.EditFollowupAsync(followupMessageId, new DiscordWebhookBuilder().WithContent(content), attachments);

        public static Task<DiscordMessage> EditFollowupAsync(this BaseContext ctx, ulong followupMessageId, DiscordEmbed embed, IEnumerable<DiscordAttachment> attachments = null) => 
            ctx.EditFollowupAsync(followupMessageId, new DiscordWebhookBuilder().AddEmbed(embed), attachments);

        public static Task<DiscordMessage> EditFollowupAsync(this BaseContext ctx, ulong followupMessageId, string content, DiscordEmbed embed, IEnumerable<DiscordAttachment> attachments = null) => 
            ctx.EditFollowupAsync(followupMessageId, new DiscordWebhookBuilder().WithContent(content).AddEmbed(embed), attachments);
    }
}