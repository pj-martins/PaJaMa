// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Common.ApplicationStateManager
// Assembly: FreeYouTubeDownloader.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 06EDD06F-245A-4D5E-A05A-E271B59D1186
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Common.dll

using FreeYouTubeDownloader.Debug;
using System;
using System.Collections.Generic;

namespace FreeYouTubeDownloader.Common
{
  public class ApplicationStateManager
  {
    private readonly List<IPreservable> _objects;
    private static ApplicationStateManager _stateManager;

    public static ApplicationStateManager Instance
    {
      get
      {
        return ApplicationStateManager._stateManager ?? (ApplicationStateManager._stateManager = new ApplicationStateManager());
      }
    }

    public ApplicationStateManager()
    {
      Log.Trace("CALL ApplicationStateManager.ctor()", (Exception) null);
      this._objects = new List<IPreservable>();
    }

    public void AddObject(IPreservable obj)
    {
      if (this._objects.Contains(obj))
        return;
      this._objects.Add(obj);
    }

    public void RemoveObject(IPreservable obj)
    {
      if (!this._objects.Contains(obj))
        return;
      this._objects.Remove(obj);
    }

    public void Save()
    {
      Log.Trace("CALL ApplicationStateManager.Save()", (Exception) null);
      foreach (IPreservable preservable in this._objects)
      {
        try
        {
          if (preservable.NeedToSerialize)
            preservable.SaveState();
        }
        catch (NotImplementedException ex)
        {
        }
      }
    }

    public void Restore()
    {
      Log.Trace("CALL ApplicationStateManager.Restore(RestoreType)", (Exception) null);
      for (int index = 0; index < this._objects.Count; ++index)
      {
        IPreservable preservable = this._objects[index];
        try
        {
          preservable.RestoreState();
        }
        catch (NotImplementedException ex)
        {
        }
      }
    }
  }
}
