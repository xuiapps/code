using static Xui.Runtime.IOS.CoreFoundation;
using static Xui.Runtime.IOS.UIKit;

namespace Xui.Runtime.IOS;

public static partial class ObjC
{
    /// <summary>
    /// Abstracts C# interactions with an Objective-C selector.
    /// </summary>
    public class Sel
    {
        private bool init = false;

        private nint selector;

        private string name;

        public Sel(nint lib, string name)
        {
            this.name = name;
            this.init = false;
            this.selector = 0;
        }

        public Sel(string name)
        {
            this.name = name;
            this.init = false;
            this.selector = 0;
        }

        public Sel(string name, nint id)
        {
            if (selector == 0)
            {
                throw new ObjCException($"Sel '{name}' created with 0.");
            }

            this.name = name;
            this.init = true;
            this.selector = id;
        }

        public nint Selector
        {
            get
            {
                if (!this.init)
                {
                    using var cfStringRef = new CFStringRef(name);
                    this.selector = NSSelectorFromString(cfStringRef);
                    this.init = true;
                }

                return this.selector;
            }
        }

        public static Sel RegisterName(string name)
        {
            nint id = sel_registerName(name);
            if (id == 0)
            {
                throw new ObjCException($"Objective-C sel_registerName for '{name}' returned 0.");
            }
        
            return new Sel(name, id);
        }

        public static implicit operator nint(Sel sel)
        {
            return sel.Selector;
        }
    }
}