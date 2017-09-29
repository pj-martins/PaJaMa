// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Common.Collections.CircularStack`1
// Assembly: FreeYouTubeDownloader.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 06EDD06F-245A-4D5E-A05A-E271B59D1186
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Common.dll

using System.Collections.Generic;

namespace FreeYouTubeDownloader.Common.Collections
{
  public sealed class CircularStack<T> : LinkedList<T>
  {
    public int Size { get; }

    public CircularStack(int size)
    {
      this.Size = size;
    }

    public void Push(T item)
    {
      lock (this)
      {
        while (this.Count >= this.Size)
          this.PopBack();
      }
      this.AddFirst(item);
    }

    public T Pop()
    {
      lock (this)
      {
        if (this.First == null)
          return default (T);
        T obj = this.First.Value;
        this.Remove(obj);
        return obj;
      }
    }

    public T PopBack()
    {
      lock (this)
      {
        if (this.Last == null)
          return default (T);
        T obj = this.Last.Value;
        this.Remove(obj);
        return obj;
      }
    }

    public T Peek()
    {
      if (this.First == null)
        return default (T);
      return this.First.Value;
    }

    public T PeekBack()
    {
      if (this.Last == null)
        return default (T);
      return this.Last.Value;
    }
  }
}
