using static Xui.Runtime.IOS.ObjC;
using static Xui.Runtime.IOS.UIKit;
using static Xui.Runtime.IOS.CoreFoundation;

namespace Xui.Runtime.IOS.Actual;

public class IOSSoftKeyboard : UIResponder
{
    public static new readonly Class Class = UIResponder.Class
        .Extend("XUIIOSSoftKeyboard")
        
        .AddProtocol(UITextInput)
        .AddMethod("canBecomeFirstResponder", CanBecomeFirstResponderStatic)
        .AddMethod("nextResponder", NextResponderStatic)

        // .AddProtocol(UITextInputTraits)
        .AddMethod("keyboardType", KeyboardTypeStatic)
        .AddMethod("keyboardAppearance", KeyboardAppearanceStatic)
        .AddMethod("returnKeyType", ReturnKeyTypeStatic)
        .AddMethod("textContentType", TextContentTypeStatic)

        // .AddProtocol(UIKeyInput)
        .AddMethod("insertText:", InsertTextStatic)
        .AddMethod("deleteBackward", DeleteBackwardsStatic)
        .AddMethod("hasText", HasTextStatic)

        // .AddProtocol(UITextInput)
        .AddMethod("inputDelegate", InputDelegateStatic)
        .AddMethod("markedTextRange", MarkedTextRangeStatic)
        .AddMethod("selectedTextRange", SelectedTextRangeStatic)
        .AddMethod("beginningOfDocument", BeginningOfDocumentStatic)
        .AddMethod("endOfDocument", EndOfDocumentStatic)
        .AddMethod("comparePosition:toPosition:", ComparePositionToPositionStatic)

        .AddProtocol(UITextInputDelegate)
        
        .AddMethod("textWillChange:", TextWillChangeStatic)
        .AddMethod("textDidChange:", TextDidChangeStatic)
        .AddMethod("selectionWillChange:", SelectionWillChangeStatic)
        .AddMethod("selectionDidChange:", SelectionDidChangeStatic)
        .AddMethod("conversationContext:didChange:", ConversationContextDidChangeStatic)

        .Register();
    
    private static bool CanBecomeFirstResponderStatic(nint self, nint sel) =>
        Marshalling.Get<IOSSoftKeyboard>(self).CanBecomeFirstResponder;

    private static nint NextResponderStatic(nint self, nint sel) =>
        Marshalling.Get<IOSSoftKeyboard>(self).NextResponder;
    
    private static nint KeyboardTypeStatic(nint self, nint sel) => 0;
    private static nint KeyboardAppearanceStatic(nint self, nint sel) => 0;
    private static nint ReturnKeyTypeStatic(nint self, nint sel) => 0;
    private static nint TextContentTypeStatic(nint self, nint sel) => 0;

    private static void InsertTextStatic(nint self, nint sel, nint cfStringRef) =>
        Marshalling.Get<IOSSoftKeyboard>(self).InsertText(CFStringRef.Marshal(cfStringRef)!);

    private static void DeleteBackwardsStatic(nint self, nint sel) =>
        Marshalling.Get<IOSSoftKeyboard>(self).DeleteBackwards();

    private static bool HasTextStatic(nint self, nint sel) => false;

    private static nint InputDelegateStatic(nint self, nint sel) => self;

    private static nint MarkedTextRangeStatic(nint self, nint sel)
    {
        // TODO: Return UITextRange
        return 0;
    }

    private static nint SelectedTextRangeStatic(nint self, nint sel)
    {
        // TODO: Return UITextRange
        return 0;
    }

    private static nint BeginningOfDocumentStatic(nint self, nint sel)
    {
        // TODO: Return UITextPosition
        return 0;
    }

    private static nint EndOfDocumentStatic(nint self, nint sel)
    {
        // TODO: Return UITextPosition
        return 0;
    }

    private static nint ComparePositionToPositionStatic(nint self, nint sel, nint position, nint other)
    {
        // -1 The left operand is smaller than the right operand.
        // 0 The two operands are equal.
        // 1 The left operand is greater than the right operand.
        return 0;
    }

    private static void TextWillChangeStatic(nint self, nint sel, nint textInput)
    {
    }

    private static void TextDidChangeStatic(nint self, nint sel, nint textInput)
    {
    }

    private static void SelectionWillChangeStatic(nint self, nint sel, nint textInput)
    {   
    }

    private static void SelectionDidChangeStatic(nint self, nint sel, nint textInput)
    {   
    }
    
    private static void ConversationContextDidChangeStatic(nint self, nint sel, nint conversationCOntext, nint textInput)
    {   
    }
    
    public IOSSoftKeyboard(IOSWindow window) : base(Class.New())
    {
        this.Window = window;
    }

    public bool CanBecomeFirstResponder => true;

    // TODO: Use some sort of ref-counting gere...
    public nint NextResponder { get; set; }
    
    protected internal IOSWindow Window { get; }

    private void InsertText(string text) =>
        this.Window.InsertText(text);

    private void DeleteBackwards() =>
        this.Window.DeleteBackwards();
}