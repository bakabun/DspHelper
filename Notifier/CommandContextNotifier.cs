using System;
using System.Threading.Tasks;
using Bakabun.DspHelper.Extensions;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;

namespace Bakabun.DspHelper.Notifier
{
    public static class CommandContextNotifier
    {
        public static Task NotifySuccessAsync(this CommandContext ctx, string message)
        {
            var embed = new DiscordEmbedBuilder()
                .WithColor(DiscordColor.Green)
                .WithDescription($"**✅ Success**: {message}")
                .WithFooter(ctx.Guild.Name, ctx.Guild.GetIconUrl())
                .WithCurrentTimestamp()
                .Build();

            return ctx.RespondAsync(embed);
        }

        public static Task NotifyErrorAsync(this CommandContext ctx, string message)
        {
            var embed = new DiscordEmbedBuilder()
                .WithColor(DiscordColor.Red)
                .WithDescription($"**⛔ Error**: {message}")
                .WithFooter(ctx.Guild.Name, ctx.Guild.GetIconUrl())
                .WithCurrentTimestamp()
                .Build();

            return ctx.RespondAsync(embed);
        }

        public static Task NotifyWarningAsync(this CommandContext ctx, string message)
        {
            var embed = new DiscordEmbedBuilder()
                .WithColor(DiscordColor.Yellow)
                .WithDescription($"**⚠️ Warning**: {message}")
                .WithFooter(ctx.Guild.Name, ctx.Guild.GetIconUrl())
                .WithCurrentTimestamp()
                .Build();

            return ctx.RespondAsync(embed);
        }

        public static async Task SendConfirmationAsync(this CommandContext ctx, string question, string proceedMessage, Action onProceedAction, int timeoutOverride = 30)
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

            var msg = await ctx.RespondAsync(msgBuilder);
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
