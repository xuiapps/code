using static Xui.Runtime.IOS.CoreFoundation;
using static Xui.Runtime.IOS.UIKit;
using System.Runtime.InteropServices;
using System.Dynamic;

namespace Xui.Runtime.IOS;

public static partial class ObjC
{
    public struct Prop
    {
        public readonly Sel GetSel;
        public readonly Sel SetSel;

        public Prop(string get, string set)
        {
            this.GetSel = new Sel(get);
            this.SetSel = new Sel(set);
        }

        public struct Bool
        {
            private Prop prop;

            public Bool(string get, string set)
            {
                this.prop = new Prop(get, set);
            }

            public bool Get(nint id) => objc_msgSend_retBool(id, this.prop.GetSel);
            public void Set(nint id, bool value) => objc_msgSend(id, this.prop.SetSel, value);
        }

        public struct NInt
        {
            private Prop prop;

            public NInt(string get, string set)
            {
                this.prop = new Prop(get, set);
            }

            public nint Get(nint id) => objc_msgSend_retIntPtr(id, this.prop.GetSel);
            public void Set(nint id, nint value) => objc_msgSend(id, this.prop.SetSel, value);
        }

        public struct NUInt
        {
            private Prop prop;

            public NUInt(string get, string set)
            {
                this.prop = new Prop(get, set);
            }

            public nuint Get(nint id) => objc_msgSend_retNUInt(id, this.prop.GetSel);
            public void Set(nint id, nuint value) => objc_msgSend(id, this.prop.SetSel, value);
        }

        public struct String
        {
            private Prop prop;

            public String(string get, string set)
            {
                this.prop = new Prop(get, set);
            }

            public string? Get(nint id)
            {
                if (id == 0)
                {
                    return null;
                }
                else
                {
                    return objc_msgSend_retCStr(
                        objc_msgSend_retIntPtr(id, this.prop.GetSel),
                        UTF8StringSel);
                }
            } 

            public void Set(nint id, string? value)
            {
                if (value == null)
                {
                    objc_msgSend(id, this.prop.SetSel, nint.Zero);
                }
                else
                {
                    using var cfStringRef = new CFStringRef(value);
                    objc_msgSend(id, this.prop.SetSel, cfStringRef);
                }
            }
        }
    }
}