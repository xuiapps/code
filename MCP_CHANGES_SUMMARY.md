# MCP XML Format Update - Summary

## Changes Made

This PR implements the requested enhancements to the Xui MCP (Model Context Protocol) server to improve GitHub Copilot integration and LLM interaction with Xui applications.

## 1. JSON → XML Transition

**Before (JSON format):**
```json
{
  "root": {
    "type": "Button",
    "x": 20,
    "y": 20,
    "w": 120,
    "h": 50,
    "visible": true,
    "children": []
  }
}
```

**After (XML format):**
```xml
<Button id="btn-submit" class="primary" x="20.00" y="20.00" w="120.00" h="50.00" centerX="80.00" centerY="45.00" visible="true" />
```

### Benefits:
- ✅ **Human-readable**: Easier to understand at a glance
- ✅ **Semantic structure**: Element names directly represent View types
- ✅ **Familiar format**: Uses XML conventions similar to HTML/XAML
- ✅ **Better for LLMs**: More natural for AI agents to parse and understand

## 2. Center Point for Clicking

**Problem Solved:**
LLMs were clicking at element edges (x, y coordinates), often missing interactive areas by clicking on margins.

**Solution:**
Added `centerX` and `centerY` attributes that provide the center point of each View element.

**Example:**
```xml
<Button x="20.00" y="20.00" w="120.00" h="50.00" centerX="80.00" centerY="45.00" />
```
- Old: Click(20, 20) → clicks top-left corner (often margin)
- New: Click(80, 45) → clicks center of button (reliable)

## 3. ID and Class Attributes

**Added Support:**
- `id` attribute: Unique identifier from `View.Id`
- `class` attribute: Space-separated class names from `View.ClassName`

**Example:**
```xml
<Button id="btn-submit" class="primary large" ... />
<Text id="status-label" class="info-text" ... />
```

### Benefits:
- ✅ **CSS-like selection**: Find elements by ID or class
- ✅ **Clear identification**: Easy to reference specific elements
- ✅ **Better organization**: Group related elements with classes

## 4. Files Modified

### Core Protocol Changes:
1. **`Xui/Middleware/DevTools/IO/Protocol.cs`**
   - Updated `ViewNode` record to include:
     - `float CenterX, float CenterY`
     - `string? Id`
     - `string? ClassName`

2. **`Xui/Middleware/DevTools/Actual/DevToolsWindow.cs`**
   - Modified `WalkView()` to:
     - Calculate center points: `(X + W/2, Y + H/2)`
     - Extract `Id` from `view.Id`
     - Extract `ClassName` as space-separated string from `view.ClassName`

3. **`Xui/Middleware/DevTools/DevToolsPlatform.cs`**
   - Updated empty `ViewNode` constructor to match new signature

### MCP Server Changes:
4. **`Xui/Utils/MCP/AppTools.cs`**
   - Changed `InspectUi()` return format from JSON to XML
   - Added `ViewNodeToXml()` method for XML serialization
   - Added `WriteViewNode()` recursive helper for tree traversal
   - Made `JsonOptions` public in `DevToolsClient`
   - Added local `ViewNode` and `InspectResult` records

### Build Fixes:
5. **`Xui/Utils/MCP/Xui.MCP.csproj`**
   - Fixed cross-platform path separators (backslash → forward slash)
   - Now builds correctly on Linux/macOS

## 5. Documentation Added

### MCP Server Documentation:
**`Xui/Utils/MCP/README.md`** (6.5 KB)
- Complete MCP server documentation
- Tool descriptions and usage examples
- XML format explanation with examples
- Architecture overview
- Troubleshooting guide

### GitHub Copilot Integration Guide:
**`.github/COPILOT-INTEGRATION.md`** (8.4 KB)
- How to use GitHub Copilot with Xui MCP
- Example workflows and tasks
- Best practices for users and developers
- Common troubleshooting scenarios
- Advanced usage patterns

## 6. GitHub Copilot Integration Setup

### Automatic Discovery:
The repository includes `.mcp.json` at the root:
```json
{
  "mcpServers": {
    "xui": {
      "type": "stdio",
      "command": "dotnet",
      "args": ["run", "--project", "Xui/Utils/MCP/Xui.MCP.csproj"]
    }
  }
}
```

### How It Works:
1. GitHub Copilot discovers the MCP server via `.mcp.json`
2. When you ask Copilot to interact with Xui apps, it uses the MCP tools
3. Copilot can now:
   - Start/stop apps
   - Inspect UI in XML format
   - Click elements using center points
   - Locate elements by ID or class
   - Take screenshots
   - Monitor logs

## 7. Example Usage

### Asking GitHub Copilot:
```
"Start the TestApp and click the submit button"
```

### What Copilot Does:
1. Calls `StartApp("Xui/Apps/TestApp/TestApp.Desktop.csproj")`
2. Calls `InspectUi()` → receives XML:
   ```xml
   <Button id="btn-submit" centerX="80.00" centerY="45.00" ... />
   ```
3. Parses XML to find the button
4. Extracts center point: (80, 45)
5. Calls `Click(80.0, 45.0)`
6. ✅ Button clicked successfully!

## 8. Testing

Built and verified:
- ✅ `Xui.Middleware.DevTools` project builds successfully
- ✅ `Xui.MCP` project builds successfully
- ✅ XML output format verified with sample data
- ✅ All attributes present in output (id, class, center points)
- ✅ Cross-platform build works on Linux

## 9. Backwards Compatibility

### Breaking Changes:
⚠️ The `InspectUi()` tool now returns XML instead of JSON.

### Migration:
If you have existing code that parses the JSON output:
- Update to parse XML instead
- Use XPath or XML parsing libraries
- Center points are now available for reliable clicking

### For LLM Agents:
No migration needed - the XML format is more intuitive and easier to work with.

## 10. Future Enhancements

Potential improvements for future PRs:
- [ ] Add `data-*` attributes for custom metadata
- [ ] Support filtering by visibility in InspectUi
- [ ] Add `XPath` or `CSS selector` query tool
- [ ] Cache UI tree for performance
- [ ] Add diff/comparison between UI states
- [ ] Support for accessibility attributes

## 11. Verification Steps

To verify these changes work:

1. **Build the MCP project:**
   ```bash
   dotnet build Xui/Utils/MCP/Xui.MCP.csproj
   ```

2. **Start a demo app:**
   ```bash
   dotnet run --project Xui/Apps/TestApp/TestApp.Desktop.csproj -c Debug
   ```

3. **Start the MCP server:**
   ```bash
   dotnet run --project Xui/Utils/MCP/Xui.MCP.csproj
   ```

4. **Use GitHub Copilot to interact:**
   - Ask: "Inspect the TestApp UI and describe the layout"
   - Copilot will use the MCP server to retrieve the XML tree

## Summary

This PR successfully addresses all requirements from the problem statement:

✅ **JSON → XML transition**: InspectUi now returns XML format  
✅ **Center points added**: centerX and centerY prevent edge-clicking  
✅ **ID and class support**: Elements can be located by id or class  
✅ **GitHub Copilot integration**: Documented and ready to use  
✅ **Cross-platform builds**: Fixed path separator issues  
✅ **Comprehensive documentation**: Added guides for users and developers  

The changes make it significantly easier for LLM agents like GitHub Copilot to:
- Understand UI structure
- Locate specific elements
- Interact reliably with UI elements
- Record and replay test scenarios
- Explore and document applications
