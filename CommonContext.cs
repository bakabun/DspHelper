using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Bakabun.DspHelper
{
    public class CommonContext
    {
        public DiscordClient Client { get; }
        public DiscordChannel Channel { get; }
        public DiscordGuild Guild { get; }
        public DiscordUser User { get; }
        public DiscordMember Member { get; }
        public IServiceProvider Services { get; }
        public bool IsCommandContext { get; }
        public bool IsInteractionContext { get; }
        public bool IsContextCommandContext { get; }
        public bool IsApplicationCommandContext => this.IsInteractionContext || this.IsContextCommandContext;

        private readonly object _originalContext;

        public CommonContext(CommandContext ctx)
        {
            this.Client = ctx.Client;
            this.Channel = ctx.Channel;
            this.Guild = ctx.Guild;
            this.User = ctx.User;
            this.Member = ctx.Member;
            this.Services = ctx.Services;
            this.IsCommandContext = true;

            this._originalContext = ctx;
        }

        public CommonContext(BaseContext ctx)
        {
            this.Client = ctx.Client;
            this.Channel = ctx.Channel;
            this.Guild = ctx.Guild;
            this.User = ctx.User;
            this.Member = ctx.Member;
            this.Services = ctx.Services;

            if (ctx is InteractionContext)
            {
                this.IsInteractionContext = true;
            }
            else if (ctx is ContextMenuContext)
            {
                this.IsContextCommandContext = true;
            }

            this._originalContext = ctx;
        }

        public async Task<DiscordMessage> RespondAsync(string content, bool ephemeral = false)
        {
            if (this.IsCommandContext)
            {
                return await ((CommandContext)this).RespondAsync(content);
            }
            else
            {
                await ((BaseContext)this).CreateResponseAsync(content, ephemeral);
                return await ((BaseContext)this).GetOriginalResponseAsync();
            }
        }

        public async Task<DiscordMessage> RespondAsync(DiscordEmbed embed, bool ephemeral = false)
        {
            if (this.IsCommandContext)
            {
                return await ((CommandContext)this).RespondAsync(embed);
            }
            else
            {
                await ((BaseContext)this).CreateResponseAsync(embed, ephemeral);
                return await ((BaseContext)this).GetOriginalResponseAsync();
            }
        }

        public async Task<DiscordMessage> RespondAsync(string content, DiscordEmbed embed, bool ephemeral = false)
        {
            if (this.IsCommandContext)
            {
                return await ((CommandContext)this).RespondAsync(content, embed);
            }
            else
            {
                await ((BaseContext)this).CreateResponseAsync(content, embed, ephemeral);
                return await ((BaseContext)this).GetOriginalResponseAsync();
            }
        }

        public async Task<DiscordMessage> RespondAsync(DiscordMessageBuilder builder, bool ephemeral = false)
        {
            if (this.IsCommandContext)
            {
                return await ((CommandContext)this).RespondAsync(builder);
            }
            else
            {
                await ((BaseContext)this).CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder(builder).AsEphemeral(ephemeral));
                return await ((BaseContext)this).GetOriginalResponseAsync();
            }
        }

        public Task SendDeferAsync(bool ephemeral = false) =>
            this.IsCommandContext ? ((CommandContext)this).TriggerTypingAsync() : ((BaseContext)this).DeferAsync(ephemeral);

        public static implicit operator CommonContext(BaseContext ctx) => new CommonContext(ctx);
        public static implicit operator CommonContext(CommandContext ctx) => new CommonContext(ctx);

        public static explicit operator CommandContext(CommonContext ctx) => (CommandContext)ctx._originalContext;
        public static explicit operator BaseContext(CommonContext ctx) => (BaseContext)ctx._originalContext;
        public static explicit operator InteractionContext(CommonContext ctx) => (InteractionContext)ctx._originalContext;
        public static explicit operator ContextMenuContext(CommonContext ctx) => (ContextMenuContext)ctx._originalContext;
    }
}
