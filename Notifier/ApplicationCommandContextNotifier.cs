using System;
using System.Threading.Tasks;
using Bakabun.DspHelper.Extensions;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;

namespace Bakabun.DspHelper.Notifier
{
    public static class ApplicationCommandContextNotifier
    {
        public static Task NotifySuccessAsync(this BaseContext ctx, string message, bool asFollowup = false, bool ephemeral = false)
        {
            var embed = new DiscordEmbedBuilder()
                .WithColor(DiscordColor.Green)
                .WithDescription($"**✅ Success**: {message}")
                .WithFooter(ctx.Guild.Name, ctx.Guild.GetIconUrl())
                .WithCurrentTimestamp()
                .Build();

            if (asFollowup) return ctx.FollowUpAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed));
            return ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder().AddEmbed(embed).AsEphemeral(ephemeral));
        }

        public static Task NotifyErrorAsync(this BaseContext ctx, string message, bool asFollowup = false, bool ephemeral = false)
        {
            var embed = new DiscordEmbedBuilder()
                .WithColor(DiscordColor.Red)
                .WithDescription($"**⛔ Error**: {message}")
                .WithFooter(ctx.Guild.Name, ctx.Guild.GetIconUrl())
                .WithCurrentTimestamp()
                .Build();

            if (asFollowup) return ctx.FollowUpAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed));
            return ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder().AddEmbed(embed).AsEphemeral(ephemeral));
        }

        public static Task NotifyWarningAsync(this BaseContext ctx, string message, bool asFollowup = false, bool ephemeral = false)
        {
            var embed = new DiscordEmbedBuilder()
                .WithColor(DiscordColor.Yellow)
                .WithDescription($"**⚠️ Warning**: {message}")
                .WithFooter(ctx.Guild.Name, ctx.Guild.GetIconUrl())
                .WithCurrentTimestamp()
                .Build();

            if (asFollowup) return ctx.FollowUpAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed));
            return ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder().AddEmbed(embed).AsEphemeral(true));
        }

        public static async Task SendConfirmationAsync(this BaseContext ctx, string question, string proceedMessage, Action onProceedAction, int timeoutOverride = 30)
        {
            var yesButton = new DiscordButtonComponent(ButtonStyle.Danger, "confirm", "Yes");
            var noButton = new DiscordButtonComponent(ButtonStyle.Secondary, "cancel", "No");

            var msgBuilder = new DiscordMessageBuilder()
                .WithEmbed(new DiscordEmbedBuilder()
                    .WithColor(DiscordColor.Gold)
                    .WithTitle("❔ Confirmation")
                    .WithDescription(question)
                    .WithFooter("Press \"yes\" to proceed, \"no\" to cancel", ctx.Guild.GetIconUrl())
                    .WithCurrentTimestamp())
                .AddComponents(yesButton, noButton);

            var proceedEmbed = new DiscordEmbedBuilder()
                .WithColor(DiscordColor.Green)
                .WithTitle("✅ Success")
                .WithDescription(proceedMessage)
                .WithFooter(ctx.Guild.Name, ctx.Guild.GetIconUrl())
                .WithCurrentTimestamp()
                .Build();

            var cancelledEmbed = new DiscordEmbedBuilder()
                .WithColor(DiscordColor.Red)
                .WithTitle("🛑 Cancelled")
                .WithFooter(ctx.Guild.Name, ctx.Guild.GetIconUrl())
                .WithCurrentTimestamp();

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder(msgBuilder));
            var msg = await ctx.GetOriginalResponseAsync();
            var res = await msg.WaitForButtonAsync(ctx.User, TimeSpan.FromSeconds(timeoutOverride));
            if (res.TimedOut)
            {
                msgBuilder.Clear();
                await msg.ModifyAsync(msgBuilder.AddEmbed(cancelledEmbed.WithDescription("Action cancelled due to timeout.")));
            }
            else
            {
                if (res.Result.Id == yesButton.CustomId)
                {
                    onProceedAction.Invoke();
                    await res.Result.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
                    msgBuilder.Clear();
                    await msg.ModifyAsync(msgBuilder.AddEmbed(proceedEmbed));
                }
                else if (res.Result.Id == noButton.CustomId)
                {
                    await res.Result.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
                    msgBuilder.Clear();
                    await msg.ModifyAsync(msgBuilder.AddEmbed(cancelledEmbed.WithDescription("Action cancelled by user.")));
                }
            }
        }
    }
}
