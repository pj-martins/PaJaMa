// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.UI.ToolStripAeroRenderer
// Assembly: YouTubeDownloader, Version=4.2.795.0, Culture=neutral, PublicKeyToken=null
// MVID: E906DA98-D8CA-4CAE-B96B-249F2467E375
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\YouTubeDownloader.exe

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using System.Windows.Forms.VisualStyles;

namespace FreeYouTubeDownloader.UI
{
  public class ToolStripAeroRenderer : ToolStripSystemRenderer
  {
    private static readonly int RebarBackground = 6;
    private VisualStyleRenderer _renderer;

    public ToolbarTheme Theme { get; set; }

    private string RebarClass
    {
      get
      {
        return this.SubclassPrefix + "Rebar";
      }
    }

    private string ToolbarClass
    {
      get
      {
        return this.SubclassPrefix + "ToolBar";
      }
    }

    private string MenuClass
    {
      get
      {
        return this.SubclassPrefix + "Menu";
      }
    }

    private string SubclassPrefix
    {
      get
      {
        switch (this.Theme)
        {
          case ToolbarTheme.MediaToolbar:
            return "Media::";
          case ToolbarTheme.CommunicationsToolbar:
            return "Communications::";
          case ToolbarTheme.BrowserTabBar:
            return "BrowserTabBar::";
          case ToolbarTheme.HelpBar:
            return "Help::";
          default:
            return string.Empty;
        }
      }
    }

    public bool IsSupported
    {
      get
      {
        if (!VisualStyleRenderer.IsSupported)
          return false;
        return VisualStyleRenderer.IsElementDefined(VisualStyleElement.CreateElement("Menu", 7, 1));
      }
    }

    public ToolStripAeroRenderer(ToolbarTheme theme)
    {
      this.Theme = theme;
    }

    private Padding GetThemeMargins(IDeviceContext dc, ToolStripAeroRenderer.MarginTypes marginType)
    {
      try
      {
        ToolStripAeroRenderer.NativeMethods.MARGINS pMargins;
        if (ToolStripAeroRenderer.NativeMethods.GetThemeMargins(this._renderer.Handle, dc.GetHdc(), this._renderer.Part, this._renderer.State, (int) marginType, IntPtr.Zero, out pMargins) == 0)
          return new Padding(pMargins.cxLeftWidth, pMargins.cyTopHeight, pMargins.cxRightWidth, pMargins.cyBottomHeight);
        return new Padding(0);
      }
      finally
      {
        dc.ReleaseHdc();
      }
    }

    private static int GetItemState(ToolStripItem item)
    {
      bool selected = item.Selected;
      if (item.IsOnDropDown)
      {
        if (item.Enabled)
          return !selected ? 1 : 2;
        return !selected ? 3 : 4;
      }
      if (item.Pressed)
        return !item.Enabled ? 6 : 3;
      if (item.Enabled)
        return !selected ? 1 : 2;
      return !selected ? 4 : 5;
    }

    private VisualStyleElement Subclass(VisualStyleElement element)
    {
      return VisualStyleElement.CreateElement(this.SubclassPrefix + element.ClassName, element.Part, element.State);
    }

    private bool EnsureRenderer()
    {
      if (!this.IsSupported)
        return false;
      if (this._renderer == null)
        this._renderer = new VisualStyleRenderer(VisualStyleElement.Button.PushButton.Normal);
      return true;
    }

    protected override void Initialize(ToolStrip toolStrip)
    {
      if (toolStrip.Parent is ToolStripPanel)
        toolStrip.BackColor = Color.Transparent;
      base.Initialize(toolStrip);
    }

    protected override void InitializePanel(ToolStripPanel toolStripPanel)
    {
      foreach (Control control in (ArrangedElementCollection) toolStripPanel.Controls)
      {
        if (control is ToolStrip)
          this.Initialize((ToolStrip) control);
      }
      base.InitializePanel(toolStripPanel);
    }

    protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
    {
      if (this.EnsureRenderer())
      {
        this._renderer.SetParameters(this.MenuClass, 10, 0);
        if (!e.ToolStrip.IsDropDown)
          return;
        Region clip = e.Graphics.Clip;
        Rectangle clientRectangle = e.ToolStrip.ClientRectangle;
        clientRectangle.Inflate(-1, -1);
        e.Graphics.ExcludeClip(clientRectangle);
        this._renderer.DrawBackground((IDeviceContext) e.Graphics, e.ToolStrip.ClientRectangle, e.AffectedBounds);
        e.Graphics.Clip = clip;
      }
      else
        base.OnRenderToolStripBorder(e);
    }

    private Rectangle GetBackgroundRectangle(ToolStripItem item)
    {
      if (!item.IsOnDropDown)
        return new Rectangle(new Point(), item.Bounds.Size);
      Rectangle bounds = item.Bounds;
      bounds.X = item.ContentRectangle.X + 1;
      bounds.Width = item.ContentRectangle.Width - 1;
      bounds.Y = 0;
      return bounds;
    }

    protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
    {
      if (this.EnsureRenderer())
      {
        this._renderer.SetParameters(this.MenuClass, e.Item.IsOnDropDown ? 14 : 8, ToolStripAeroRenderer.GetItemState(e.Item));
        Rectangle backgroundRectangle = this.GetBackgroundRectangle(e.Item);
        this._renderer.DrawBackground((IDeviceContext) e.Graphics, backgroundRectangle, backgroundRectangle);
      }
      else
        base.OnRenderMenuItemBackground(e);
    }

    protected override void OnRenderToolStripPanelBackground(ToolStripPanelRenderEventArgs e)
    {
      if (this.EnsureRenderer())
      {
        if (VisualStyleRenderer.IsElementDefined(VisualStyleElement.CreateElement(this.RebarClass, ToolStripAeroRenderer.RebarBackground, 0)))
          this._renderer.SetParameters(this.RebarClass, ToolStripAeroRenderer.RebarBackground, 0);
        else
          this._renderer.SetParameters(this.RebarClass, 0, 0);
        if (this._renderer.IsBackgroundPartiallyTransparent())
          this._renderer.DrawParentBackground((IDeviceContext) e.Graphics, e.ToolStripPanel.ClientRectangle, (Control) e.ToolStripPanel);
        this._renderer.DrawBackground((IDeviceContext) e.Graphics, e.ToolStripPanel.ClientRectangle);
        e.Handled = true;
      }
      else
        base.OnRenderToolStripPanelBackground(e);
    }

    protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
    {
      if (this.EnsureRenderer())
      {
        if (e.ToolStrip.IsDropDown)
        {
          this._renderer.SetParameters(this.MenuClass, 9, 0);
        }
        else
        {
          if (e.ToolStrip.Parent is ToolStripPanel)
            return;
          if (VisualStyleRenderer.IsElementDefined(VisualStyleElement.CreateElement(this.RebarClass, ToolStripAeroRenderer.RebarBackground, 0)))
            this._renderer.SetParameters(this.RebarClass, ToolStripAeroRenderer.RebarBackground, 0);
          else
            this._renderer.SetParameters(this.RebarClass, 0, 0);
        }
        if (this._renderer.IsBackgroundPartiallyTransparent())
          this._renderer.DrawParentBackground((IDeviceContext) e.Graphics, e.ToolStrip.ClientRectangle, (Control) e.ToolStrip);
        this._renderer.DrawBackground((IDeviceContext) e.Graphics, e.ToolStrip.ClientRectangle, e.AffectedBounds);
      }
      else
        base.OnRenderToolStripBackground(e);
    }

    protected override void OnRenderSplitButtonBackground(ToolStripItemRenderEventArgs e)
    {
      if (this.EnsureRenderer())
      {
        ToolStripSplitButton stripSplitButton = (ToolStripSplitButton) e.Item;
        base.OnRenderSplitButtonBackground(e);
        this.OnRenderArrow(new ToolStripArrowRenderEventArgs(e.Graphics, (ToolStripItem) stripSplitButton, stripSplitButton.DropDownButtonBounds, Color.Red, ArrowDirection.Down));
      }
      else
        base.OnRenderSplitButtonBackground(e);
    }

    private Color GetItemTextColor(ToolStripItem item)
    {
      this._renderer.SetParameters(this.MenuClass, item.IsOnDropDown ? 14 : 8, ToolStripAeroRenderer.GetItemState(item));
      return this._renderer.GetColor(ColorProperty.TextColor);
    }

    protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
    {
      if (this.EnsureRenderer())
        e.TextColor = this.GetItemTextColor(e.Item);
      base.OnRenderItemText(e);
    }

    protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
    {
      if (this.EnsureRenderer())
      {
        if (!e.ToolStrip.IsDropDown)
          return;
        this._renderer.SetParameters(this.MenuClass, 13, 0);
        Padding themeMargins = this.GetThemeMargins((IDeviceContext) e.Graphics, ToolStripAeroRenderer.MarginTypes.Sizing);
        int width1 = e.ToolStrip.Width;
        Rectangle rectangle = e.ToolStrip.DisplayRectangle;
        int width2 = rectangle.Width;
        int num1 = width1 - width2 - themeMargins.Left - themeMargins.Right - 1;
        rectangle = e.AffectedBounds;
        int width3 = rectangle.Width;
        int num2 = num1 - width3;
        Rectangle bounds = e.AffectedBounds;
        bounds.Y += 2;
        bounds.Height -= 4;
        int width4 = this._renderer.GetPartSize((IDeviceContext) e.Graphics, ThemeSizeType.True).Width;
        if (e.ToolStrip.RightToLeft == RightToLeft.Yes)
        {
          bounds = new Rectangle(bounds.X - num2, bounds.Y, width4, bounds.Height);
          bounds.X += width4;
        }
        else
          bounds = new Rectangle(bounds.Width + num2 - width4, bounds.Y, width4, bounds.Height);
        this._renderer.DrawBackground((IDeviceContext) e.Graphics, bounds);
      }
      else
        base.OnRenderImageMargin(e);
    }

    protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
    {
      if (e.ToolStrip.IsDropDown && this.EnsureRenderer())
      {
        this._renderer.SetParameters(this.MenuClass, 15, 0);
        Rectangle rectangle = new Rectangle(e.ToolStrip.DisplayRectangle.Left, 0, e.ToolStrip.DisplayRectangle.Width, e.Item.Height);
        this._renderer.DrawBackground((IDeviceContext) e.Graphics, rectangle, rectangle);
      }
      else
        base.OnRenderSeparator(e);
    }

    protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
    {
      if (this.EnsureRenderer())
      {
        Rectangle bounds = this.GetBackgroundRectangle(e.Item);
        bounds.Width = bounds.Height;
        if (e.Item.RightToLeft == RightToLeft.Yes)
          bounds = new Rectangle(e.ToolStrip.ClientSize.Width - bounds.X - bounds.Width, bounds.Y, bounds.Width, bounds.Height);
        this._renderer.SetParameters(this.MenuClass, 12, e.Item.Enabled ? 2 : 1);
        this._renderer.DrawBackground((IDeviceContext) e.Graphics, bounds);
        Rectangle imageRectangle = e.ImageRectangle;
        imageRectangle.X = bounds.X + bounds.Width / 2 - imageRectangle.Width / 2;
        imageRectangle.Y = bounds.Y + bounds.Height / 2 - imageRectangle.Height / 2;
        this._renderer.SetParameters(this.MenuClass, 11, e.Item.Enabled ? 1 : 2);
        this._renderer.DrawBackground((IDeviceContext) e.Graphics, imageRectangle);
      }
      else
        base.OnRenderItemCheck(e);
    }

    protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
    {
      if (this.EnsureRenderer())
        e.ArrowColor = this.GetItemTextColor(e.Item);
      base.OnRenderArrow(e);
    }

    protected override void OnRenderOverflowButtonBackground(ToolStripItemRenderEventArgs e)
    {
      if (this.EnsureRenderer())
      {
        string className = this.RebarClass;
        if (this.Theme == ToolbarTheme.BrowserTabBar)
          className = "Rebar";
        int state = VisualStyleElement.Rebar.Chevron.Normal.State;
        if (e.Item.Pressed)
          state = VisualStyleElement.Rebar.Chevron.Pressed.State;
        else if (e.Item.Selected)
          state = VisualStyleElement.Rebar.Chevron.Hot.State;
        this._renderer.SetParameters(className, VisualStyleElement.Rebar.Chevron.Normal.Part, state);
        this._renderer.DrawBackground((IDeviceContext) e.Graphics, new Rectangle(Point.Empty, e.Item.Size));
      }
      else
        base.OnRenderOverflowButtonBackground(e);
    }

    internal static class NativeMethods
    {
      [DllImport("uxtheme.dll")]
      public static extern int GetThemeMargins(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, int iPropId, IntPtr rect, out ToolStripAeroRenderer.NativeMethods.MARGINS pMargins);

      public struct MARGINS
      {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;
      }
    }

    private enum MenuParts
    {
      ItemTMSchema = 1,
      DropDownTMSchema = 2,
      BarItemTMSchema = 3,
      BarDropDownTMSchema = 4,
      ChevronTMSchema = 5,
      SeparatorTMSchema = 6,
      BarBackground = 7,
      BarItem = 8,
      PopupBackground = 9,
      PopupBorders = 10,
      PopupCheck = 11,
      PopupCheckBackground = 12,
      PopupGutter = 13,
      PopupItem = 14,
      PopupSeparator = 15,
      PopupSubmenu = 16,
      SystemClose = 17,
      SystemMaximize = 18,
      SystemMinimize = 19,
      SystemRestore = 20,
    }

    private enum MenuBarStates
    {
      Active = 1,
      Inactive = 2,
    }

    private enum MenuBarItemStates
    {
      Normal = 1,
      Hover = 2,
      Pushed = 3,
      Disabled = 4,
      DisabledHover = 5,
      DisabledPushed = 6,
    }

    private enum MenuPopupItemStates
    {
      Normal = 1,
      Hover = 2,
      Disabled = 3,
      DisabledHover = 4,
    }

    private enum MenuPopupCheckStates
    {
      CheckmarkNormal = 1,
      CheckmarkDisabled = 2,
      BulletNormal = 3,
      BulletDisabled = 4,
    }

    private enum MenuPopupCheckBackgroundStates
    {
      Disabled = 1,
      Normal = 2,
      Bitmap = 3,
    }

    private enum MenuPopupSubMenuStates
    {
      Normal = 1,
      Disabled = 2,
    }

    private enum MarginTypes
    {
      Sizing = 3601,
      Content = 3602,
      Caption = 3603,
    }
  }
}
