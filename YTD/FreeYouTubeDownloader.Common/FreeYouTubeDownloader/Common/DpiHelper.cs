// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Common.DpiHelper
// Assembly: FreeYouTubeDownloader.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 06EDD06F-245A-4D5E-A05A-E271B59D1186
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Common.dll

using System;
using System.Windows.Media;

namespace FreeYouTubeDownloader.Common
{
  public class DpiHelper
  {
    private static readonly DpiHelper Instance = new DpiHelper(96.0);
    private const double DefaultLogicalDpi = 96.0;
    private readonly MatrixTransform _transformFromDevice;
    private readonly MatrixTransform _transformToDevice;

    public double LogicalDpiX { get; }

    public double LogicalDpiY { get; }

    public double DeviceDpiX { get; }

    public double DeviceDpiY { get; }

    public double LogicalToDeviceUnitsScalingFactorX
    {
      get
      {
        return this._transformToDevice.Matrix.M11;
      }
    }

    public double LogicalToDeviceUnitsScalingFactorY
    {
      get
      {
        return this._transformToDevice.Matrix.M22;
      }
    }

    public double DeviceToLogicalUnitsScalingFactorX
    {
      get
      {
        return this._transformFromDevice.Matrix.M11;
      }
    }

    public double DeviceToLogicalUnitsScalingFactorY
    {
      get
      {
        return this._transformFromDevice.Matrix.M22;
      }
    }

    public DpiHelper(double logicalDpi)
    {
      this.LogicalDpiX = logicalDpi;
      this.LogicalDpiY = logicalDpi;
      IntPtr dc = NativeMethods.User32.GetDC(IntPtr.Zero);
      if (dc != IntPtr.Zero)
      {
        this.DeviceDpiX = (double) NativeMethods.Gdi32.GetDeviceCaps(dc, 88);
        this.DeviceDpiY = (double) NativeMethods.Gdi32.GetDeviceCaps(dc, 90);
        NativeMethods.User32.ReleaseDC(IntPtr.Zero, dc);
      }
      else
      {
        this.DeviceDpiX = this.LogicalDpiX;
        this.DeviceDpiY = this.LogicalDpiY;
      }
      Matrix identity1 = Matrix.Identity;
      Matrix identity2 = Matrix.Identity;
      identity1.Scale(this.DeviceDpiX / this.LogicalDpiX, this.DeviceDpiY / this.LogicalDpiY);
      identity2.Scale(this.LogicalDpiX / this.DeviceDpiX, this.LogicalDpiY / this.DeviceDpiY);
      this._transformFromDevice = new MatrixTransform(identity2);
      this._transformFromDevice.Freeze();
      this._transformToDevice = new MatrixTransform(identity1);
      this._transformToDevice.Freeze();
    }

    public static double DeviceToLogicalUnitsX(double value)
    {
      return value * DpiHelper.Instance.DeviceToLogicalUnitsScalingFactorX;
    }

    public static double DeviceToLogicalUnitsY(double value)
    {
      return value * DpiHelper.Instance.DeviceToLogicalUnitsScalingFactorY;
    }

    public static double LogicalToDeviceUnitsX(double value)
    {
      return value * DpiHelper.Instance.LogicalToDeviceUnitsScalingFactorX;
    }

    public static double LogicalToDeviceUnitsY(double value)
    {
      return value * DpiHelper.Instance.LogicalToDeviceUnitsScalingFactorY;
    }

    public static int DeviceToLogicalUnitsX(int value)
    {
      return (int) DpiHelper.DeviceToLogicalUnitsX((double) value);
    }

    public static int DeviceToLogicalUnitsY(int value)
    {
      return (int) DpiHelper.DeviceToLogicalUnitsY((double) value);
    }

    public static int LogicalToDeviceUnitsX(int value)
    {
      return (int) DpiHelper.LogicalToDeviceUnitsX((double) value);
    }

    public static int LogicalToDeviceUnitsY(int value)
    {
      return (int) DpiHelper.LogicalToDeviceUnitsY((double) value);
    }
  }
}
