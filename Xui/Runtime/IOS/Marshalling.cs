
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Xui.Runtime.IOS;

public static class Marshalling
{
    private static Dictionary<nint, object> ObjCToCSharpMap = new Dictionary<nint, object>();
    private static Dictionary<nint, Func<nint, object>> ClassFactory = new Dictionary<nint, Func<nint, object>>();

    public static void Set(nint objCInstance, object cSharpInstance)
    {
        if (objCInstance == 0)
        {
            throw new ObjCException("Trying to set C# instance responsible for 0 ptr.");
        }

        if (ObjCToCSharpMap.TryGetValue(objCInstance, out var value) && value != cSharpInstance)
        {
            throw new ObjCException($"Trying to set C# instance {cSharpInstance} as responsible for Objective-C {objCInstance}, but responsible instance already exist.");
        }

        ObjCToCSharpMap[objCInstance] = cSharpInstance;
    }

    /// <summary>
    /// Used to get the C# instance responsible for an Objective-C instance.
    /// </summary>
    public static T Get<T>(nint objCInstance)
    {
        if (objCInstance == 0)
        {
            throw new ObjCException("Can't get C# instance responsible for 0 ptr.");
        }
        else if (ObjCToCSharpMap.TryGetValue(objCInstance, out var cSharpInstance))
        {
            if (cSharpInstance is T t)
            {
                return t;
            }

            throw new ObjCException($"Tried to get C# {typeof(T).FullName} but got {cSharpInstance.GetType().FullName} for Objective-C instance {objCInstance} of class {ObjC.object_getClassName(objCInstance)}.");
        }

        throw new ObjCException($"No C# instance exists, responsible for {objCInstance}, of class {ObjC.object_getClassName(objCInstance)}.");
    }

    public static void Delete(nint id)
    {
        ObjCToCSharpMap.Remove(id);
    }

    public static T? ObjCToCSharpNullable<T>(nint objCInstance) where T : class => objCInstance == 0 ? null : ObjCToCSharp<T>(objCInstance);

    public static T ObjCToCSharp<T>(nint objCInstance)
    {
        nint objCClass = 0;
        if (ObjCToCSharpMap.TryGetValue(objCInstance, out var instance))
        {
            if (instance is T cSharpInstance)
            {
                return cSharpInstance;
            }

            throw new ObjCException($"Failed wrapping Objective-C instance {objCInstance}, got C# {instance.GetType().FullName}, expected {typeof(T).FullName}.");
        }
        else
        {
            objCClass = ObjC.object_getClass(objCInstance);
            if (ClassFactory.TryGetValue(objCClass, out var cSharpClass))
            {
                var cSharpInstance = cSharpClass(objCInstance);
                if (cSharpInstance is T cSharpInstanceAsT)
                {
                    Set(objCInstance, cSharpInstance);
                    return cSharpInstanceAsT;
                }

                throw new ObjCException($"Failed wrapping Objective-C instance {objCInstance}, constructed C# wrapper {cSharpInstance.GetType().FullName}, expected {typeof(T).FullName}.");
            }
        }

        throw new ObjCException($"Failed wrapping Objective-C instance {objCInstance} of class {ObjC.object_getClassName(objCInstance)} as C# {typeof(T).FullName}.");
    }

    /// <summary>
    /// Registers a class wrapper for Objective-C instances.
    /// It is recommended you construct the complete object graph in C#,
    /// and have C# instances responsible for the Objecitve-C counterparts,
    /// with complete ownership including lifespan.
    /// 
    /// In some cases, like dealing with singletons however, you will get Objective-C
    /// instances constructed by the runtime, and the method here will allow registering factories.
    /// 
    /// However wrapped instances will often retain the native one,
    /// and you will need to device an per-case plan how to deal with the lifetime of these.
    /// </summary>
    public static void SetClassWrapper(nint objCClass, Func<nint, object> factory)
    {
        if (ClassFactory.ContainsKey(objCClass))
        {
            throw new ObjCException($"C# class factory for Objective-C class {ObjC.class_getName(objCClass)}");
        }

        ClassFactory[objCClass] = factory;
    }
}