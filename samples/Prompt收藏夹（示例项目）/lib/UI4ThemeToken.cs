namespace StartUI4Controls
{
    /// <summary>
    /// 主题颜色令牌。每个令牌在 Light / Dark 两份 <see cref="UI4ThemeDefinition"/> 中各对应一个 <see cref="System.Windows.Media.Color"/>。
    /// 令牌名同时决定资源桥生成的资源键：<c>UI4.Color.{令牌名}</c> 与 <c>UI4.Brush.{令牌名}</c>。
    /// </summary>
    public enum UI4ThemeToken
    {
        Accent,
        AccentDark,
        AccentEnd,
        TextForeground,
        TextSecondary,
        Background,
        Surface,
        BorderNormal,
        BorderSecondary,
        BorderHover,
        BorderFocus,
        Placeholder,
        HoverOverlay,
        SelectedOverlay,
        TrackBackground,
        CheckBackground,
        Icon,
        IconHover,
        PanelBorder,
        OffBackground,
        MenuBackground,
        ListSelected,
        HeaderBackground,
        HeaderForeground,
        RowHoverBackground,
        RowSelectedBackground,
        GridLine,
        ProgressStart,
        CheckBoxUnchecked,
        HoverBorderColorLight,
    }
}
