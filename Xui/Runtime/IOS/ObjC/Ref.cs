using System;
using System.Diagnostics;
using static Xui.Runtime.IOS.CoreFoundation;

namespace Xui.Runtime.IOS;

public static partial class ObjC
{
    /// <summary>
    /// Abstracts C# interactions with an Objective-C reference counted instance.
    /// </summary>
    public abstract class Ref : IDisposable
    {
        protected nint Id { get; private set; }

        protected Ref(nint id)
        {
            this.Id = id;

            if (this.Id == 0)
            {
                GC.SuppressFinalize(this);
                throw new ArgumentException($"Constructor {this} id null.");
            }

            Marshalling.Set(this.Id, this);
        }

        public void Dispose()
        {
            if (this.Id != 0)
            {
                CFRelease(this.Id);
                Marshalling.Delete(this.Id);
                this.Id = 0;
            }

            GC.SuppressFinalize(this);
        }

        ~Ref()
        {
            if (this.Id != 0)
            {
                Debug.WriteLine($"Instance of {this} is destroyed by GC and holding ObjC ref, track lifetime and dispose instead.");
                Dispose();
            }
        }

        public static implicit operator nint(Ref cls) => cls.Id;
    }
}