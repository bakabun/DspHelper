using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.SlashCommands;

namespace Bakabun.DspHelper.Notifier
{
    public static class CommonContextNotifier
    {
        public static Task NotifySuccessAsync(this CommonContext ctx, string message, bool asFollowup = false, bool ephemeral = false) =>
            ctx.IsCommandContext ? ((CommandContext)ctx).NotifySuccessAsync(message) : ((BaseContext)ctx).NotifySuccessAsync(message, asFollowup, ephemeral);

        public static Task NotifyErrorAsync(this CommonContext ctx, string message, bool asFollowup = false, bool ephemeral = false) =>
            ctx.IsCommandContext ? ((CommandContext)ctx).NotifyErrorAsync(message) : ((BaseContext)ctx).NotifyErrorAsync(message, asFollowup, ephemeral);

        public static Task NotifyWarningAsync(this CommonContext ctx, string message, bool asFollowup = false, bool ephemeral = false) =>
            ctx.IsCommandContext ? ((CommandContext)ctx).NotifyWarningAsync(message) : ((BaseContext)ctx).NotifyWarningAsync(message, asFollowup, ephemeral);

        public static Task SendConfirmationAsync(this CommonContext ctx, string question, string proceedMessage, Action onProceedAction, int timeoutOverride = 30) =>
            ctx.IsCommandContext ? ((CommandContext)ctx).SendConfirmationAsync(question, proceedMessage, onProceedAction, timeoutOverride) : ((BaseContext)ctx).SendConfirmationAsync(question, proceedMessage, onProceedAction, timeoutOverride);
    }
}
