## The Long and Fast Path
Implement a bit of native code, that follows the rabbit down the rabbit hole:

https://cs.android.com/android/platform/superproject/+/master:frameworks/base/libs/hwui/jni/android_graphics_Canvas.cpp;l=692

Android uses JNI to call these native methods from BaseCanvas:
https://android.googlesource.com/platform/frameworks/base/+/master/graphics/java/android/graphics/BaseCanvas.java

So within onDraw of an Android View, it is possible to obtain the mNativeCanvasWrapper, pass it down as long to native method.

In the C++ JNI implementation that wrapper is simply reinterpret_cast as Canvas*:
https://cs.android.com/android/platform/superproject/+/master:frameworks/base/libs/hwui/jni/android_graphics_Canvas.cpp;drc=a21bbb027a3eff72f94063e59c894020abd73c3c;l=49

Eventually it should be possible to rewrite the JNI as similar C flat-api callable with PInvoke from C#.