// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.RadioButtonRender
// Assembly: YouTubeDownloader, Version=4.2.795.0, Culture=neutral, PublicKeyToken=null
// MVID: E906DA98-D8CA-4CAE-B96B-249F2467E375
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\YouTubeDownloader.exe

using BrightIdeasSoftware;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace FreeYouTubeDownloader
{
  public sealed class RadioButtonRender : BaseRenderer
  {
    public override void Render(Graphics g, Rectangle r)
    {
      this.DrawBackground(g, r);
      if (this.Column == null)
        return;
      r = this.ApplyCellPadding(r);
      CheckState checkState = this.GetCheckState(this.Aspect);
      r = this.CalculateRadioButtonBounds(g, r);
      RadioButtonRenderer.DrawRadioButton(g, r.Location, this.GetRadioButtonState(checkState));
    }

    protected override Rectangle HandleGetEditRectangle(Graphics g, Rectangle cellBounds, OLVListItem item, int subItemIndex, Size preferredSize)
    {
      return this.CalculatePaddedAlignedBounds(g, cellBounds, preferredSize);
    }

    public CheckState GetCheckState(object rowObject)
    {
      bool? nullable = rowObject as bool?;
      if (!nullable.HasValue)
        return CheckState.Indeterminate;
      return !nullable.Value ? CheckState.Unchecked : CheckState.Checked;
    }

    private Rectangle CalculateRadioButtonBounds(Graphics g, Rectangle cellBounds)
    {
      Size glyphSize = RadioButtonRenderer.GetGlyphSize(g, RadioButtonState.CheckedNormal);
      return this.AlignRectangle(cellBounds, new Rectangle(0, 0, glyphSize.Width, glyphSize.Height));
    }

    private RadioButtonState GetRadioButtonState(CheckState checkState)
    {
      if (this.IsCheckBoxDisabled)
        return checkState != CheckState.Unchecked && checkState == CheckState.Checked ? RadioButtonState.CheckedDisabled : RadioButtonState.UncheckedDisabled;
      if (this.IsCheckboxHot)
        return checkState != CheckState.Unchecked && checkState == CheckState.Checked ? RadioButtonState.CheckedHot : RadioButtonState.UncheckedHot;
      return checkState != CheckState.Unchecked && checkState == CheckState.Checked ? RadioButtonState.CheckedNormal : RadioButtonState.UncheckedNormal;
    }
  }
}
