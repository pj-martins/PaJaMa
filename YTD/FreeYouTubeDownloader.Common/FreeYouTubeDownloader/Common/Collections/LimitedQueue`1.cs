// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Common.Collections.LimitedQueue`1
// Assembly: FreeYouTubeDownloader.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 06EDD06F-245A-4D5E-A05A-E271B59D1186
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Common.dll

using System.Collections.Generic;
using System.Text;

namespace FreeYouTubeDownloader.Common.Collections
{
  public sealed class LimitedQueue<T> : LinkedList<T>
  {
    public int MaxSize { get; }

    public LimitedQueue(int maxSize)
    {
      this.MaxSize = maxSize;
    }

    public void Enqueue(T item)
    {
      lock (this)
      {
        while (this.Count >= this.MaxSize)
          this.RemoveFirst();
      }
      this.AddLast(item);
    }

    public T Dequeue()
    {
      T obj = default (T);
      if (this.First != null)
      {
        obj = this.First.Value;
        this.RemoveFirst();
      }
      return obj;
    }

    public T Lookup()
    {
      if (this.First == null)
        return default (T);
      return this.First.Value;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder(this.MaxSize);
      foreach (T obj in (LinkedList<T>) this)
        stringBuilder.AppendLine(obj.ToString());
      return stringBuilder.ToString();
    }
  }
}
