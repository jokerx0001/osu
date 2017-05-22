// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;

namespace osu.Game.Users
{
    public class UserPanel : Container
    {
        private const float height = 100;
        private const float content_padding = 10;
        private const float status_height = 30;

        private readonly User user;
        private OsuColour colours;

        private readonly Container cover;
        private readonly Box statusBg;
        private readonly OsuSpriteText statusMessage;

        public readonly Bindable<UserStatus> Status = new Bindable<UserStatus>();

        public UserPanel(User user)
        {
            this.user = user;

            Width = 300;
            Height = height;
            Masking = true;
            CornerRadius = 5;
            EdgeEffect = new EdgeEffect
            {
                Type = EdgeEffectType.Shadow,
                Colour = Color4.Black.Opacity(0.25f),
                Radius = 4,
            };

            Children = new Drawable[]
            {
                cover = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                },
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black.Opacity(0.7f),
                },
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Padding = new MarginPadding { Top = content_padding, Bottom = status_height + content_padding, Left = content_padding, Right = content_padding },
                    Children = new Drawable[]
                    {
                        new UpdateableAvatar
                        {
                            Size = new Vector2(height - status_height - content_padding * 2),
                            User = user,
                            Masking = true,
                            CornerRadius = 5,
                            EdgeEffect = new EdgeEffect
                            {
                                Type = EdgeEffectType.Shadow,
                                Colour = Color4.Black.Opacity(0.25f),
                                Radius = 4,
                            },
                        },
                        new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Padding = new MarginPadding { Left = height - status_height - content_padding },
                            Children = new Drawable[]
                            {
                                new OsuSpriteText
                                {
                                    Text = user.Username,
                                    TextSize = 18,
                                    Font = @"Exo2.0-SemiBoldItalic",
                                },
                                new FillFlowContainer
                                {
                                    Anchor = Anchor.BottomLeft,
                                    Origin = Anchor.BottomLeft,
                                    AutoSizeAxes = Axes.X,
                                    Height = 20f,
                                    Direction = FillDirection.Horizontal,
                                    Spacing = new Vector2(5f, 0f),
                                    Children = new Drawable[]
                                    {
                                        new DrawableFlag(user.Country?.FlagName ?? @"__")
                                        {
                                            Width = 30f,
                                            RelativeSizeAxes = Axes.Y,
                                        },
                                        new Container
                                        {
                                            Width = 40f,
                                            RelativeSizeAxes = Axes.Y,
                                        },
                                        new CircularContainer
                                        {
                                            Width = 20f,
                                            RelativeSizeAxes = Axes.Y,
                                        },
                                    },
                                },
                            },
                        },
                    },
                },
                new Container
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    RelativeSizeAxes = Axes.X,
                    Height = status_height,
                    Children = new Drawable[]
                    {
                        statusBg = new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Alpha = 0.5f,
                        },
                        new FillFlowContainer
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            AutoSizeAxes = Axes.Both,
                            Spacing = new Vector2(5f, 0f),
                            Children = new[]
                            {
                                new TextAwesome
                                {
                                    Anchor = Anchor.CentreLeft,
                                    Origin = Anchor.CentreLeft,
                                    Icon = FontAwesome.fa_circle_o,
                                    Shadow = true,
                                    TextSize = 14,
                                },
                                statusMessage = new OsuSpriteText
                                {
                                    Anchor = Anchor.CentreLeft,
                                    Origin = Anchor.CentreLeft,
                                    Font = @"Exo2.0-Semibold",
                                },
                            },
                        },
                    },
                },
            };

            Status.ValueChanged += displayStatus;
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours, TextureStore textures)
        {
            this.colours = colours;

            cover.Add(new AsyncLoadWrapper(new Sprite
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Texture = textures.Get(user.CoverUrl),
                FillMode = FillMode.Fill,
                OnLoadComplete = d => d.FadeInFromZero(200),
            }) { RelativeSizeAxes = Axes.Both });
        }

        private void displayStatus(UserStatus status)
        {
            statusBg.FadeColour(status.GetAppropriateColour(colours), 500, EasingTypes.OutQuint);
            statusMessage.Text = status.Message;
        }
    }
}
