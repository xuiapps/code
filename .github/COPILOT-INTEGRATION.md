# GitHub Copilot Integration Guide

This guide explains how to use GitHub Copilot with the Xui MCP (Model Context Protocol) server to interact with and test Xui applications.

## Overview

The Xui repository includes an MCP server that allows AI agents like GitHub Copilot to:
- Launch and control Xui demo applications
- Inspect UI hierarchies
- Simulate user interactions
- Capture screenshots
- Debug issues and review logs

## Quick Start for Copilot Users

### Automatic Discovery

GitHub Copilot should automatically discover the MCP server through the `.mcp.json` configuration file in the repository root. When working in this repository, simply ask Copilot to interact with Xui apps.

### Example Requests

Here are some example tasks you can assign to GitHub Copilot:

#### Explore an Application
```
"Start the TestApp and show me the UI hierarchy"
```

#### Test User Interactions
```
"Launch BlankApp, inspect the UI, click the main button, and verify the result"
```

#### Record Test Cases
```
"Create an automated test for the TestApp that:
1. Starts the app
2. Navigates to each example page
3. Validates that each page loads correctly
4. Records any errors"
```

#### Debug Issues
```
"Start the ClockApp and monitor it for 30 seconds. Report any crashes or errors."
```

#### Explore Microinteractions
```
"Analyze the animation behavior when clicking buttons in TestApp"
```

## MCP Server Configuration

### Location
The MCP server is located at `Xui/Utils/MCP/Xui.MCP.csproj`

### Configuration File
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

### Running Manually
If needed, you can run the MCP server manually:

```bash
cd /path/to/sdk
dotnet run --project Xui/Utils/MCP/Xui.MCP.csproj
```

## Available Demo Applications

You can test with these apps:

- **TestApp**: Full demo app with navigation and example pages
  - `Xui/Apps/TestApp/TestApp.Desktop.csproj`
  - `Xui/Apps/TestApp/TestApp.Emulator.csproj`

- **BlankApp**: Minimal playground app
  - `Xui/Apps/BlankApp/BlankApp.Desktop.csproj`

- **ClockApp**: Simple window-only app for testing rendering
  - `Xui/Apps/ClockApp/ClockApp.Desktop.csproj`

- **LoadTestApp**: Performance testing app with infinite scroll
  - `Xui/Apps/LoadTestApp/LoadTestApp.Desktop.csproj`

## Understanding UI Tree Output

The MCP server returns UI hierarchies in XML format:

```xml
<?xml version="1.0" encoding="utf-8"?>
<Canvas id="main-canvas" class="app-root" x="0.00" y="0.00" w="400.00" h="800.00" centerX="200.00" centerY="400.00" visible="true">
  <Button id="btn-submit" class="primary" x="10.00" y="20.00" w="100.00" h="50.00" centerX="60.00" centerY="45.00" visible="true">
    <Text x="15.00" y="30.00" w="80.00" h="20.00" centerX="55.00" centerY="40.00" visible="true" />
  </Button>
</Canvas>
```

### XML Attributes

- **Element Name**: The View class name (Canvas, Button, Text, etc.)
- **id**: Unique identifier for the view (when set)
- **class**: Space-separated class names (when set)
- **x, y**: Position coordinates
- **w, h**: Width and height
- **centerX, centerY**: Center point for safe clicking
- **visible**: Whether the view is visible

### Locating Elements

When asking Copilot to interact with specific UI elements:

1. **By ID**: "Click the button with id='btn-submit'"
2. **By Class**: "Find all elements with class 'primary'"
3. **By Type**: "List all Button elements"
4. **By Position**: "Click the element at center point (60, 45)"

### Safe Clicking

Always use the `centerX` and `centerY` attributes when clicking elements. This prevents issues with clicking at edges or margins:

```
✅ Good: Click(centerX, centerY)
❌ Bad:  Click(x, y)  // This clicks the top-left corner
```

## Common Workflows

### 1. Exploratory Testing

```
Task: Explore the TestApp UI and describe the layout

Steps Copilot will take:
1. Call StartApp("Xui/Apps/TestApp/TestApp.Desktop.csproj")
2. Call InspectUi() to get the UI tree
3. Parse the XML and describe the structure
4. Take a Screenshot() for visual reference
```

### 2. Interaction Testing

```
Task: Click all buttons and verify they respond

Steps Copilot will take:
1. Call InspectUi() to find all Button elements
2. For each button:
   a. Call Click(centerX, centerY)
   b. Call InspectUi() to check for changes
   c. Call GetAppLog() to check for errors
```

### 3. Recording Test Cases

```
Task: Create a test that validates the navigation flow

Steps Copilot will take:
1. Start the app and inspect initial state
2. For each navigation action:
   a. Find and click navigation elements
   b. Verify the new page loads
   c. Record the expected UI state
3. Generate test code based on observations
```

### 4. Crash Detection

```
Task: Monitor the app for crashes

Steps Copilot will take:
1. Call StartApp()
2. Periodically call GetAppLog() to check for errors
3. Monitor for unexpected app exits
4. Report any crashes with full logs
```

## Best Practices

### For Users
1. **Be Specific**: Provide clear app paths and element identifiers
2. **Use IDs**: Encourage developers to set `Id` and `ClassName` on Views
3. **Request Screenshots**: Visual verification helps validate behavior
4. **Check Logs**: Always review logs when something goes wrong

### For Developers
1. **Set View IDs**: Assign meaningful IDs to interactive elements
2. **Use ClassNames**: Group related elements with class names
3. **Enable DevTools**: Always build in Debug configuration for testing
4. **Handle Errors**: Log errors to stdout/stderr for MCP visibility

## Troubleshooting

### Copilot Can't Find MCP Server

**Symptom**: Copilot doesn't recognize MCP tools

**Solutions**:
1. Verify `.mcp.json` exists in repository root
2. Manually mention: "Use the Xui MCP server to interact with the app"
3. Check that dotnet SDK is available: `dotnet --version`

### App Fails to Start

**Symptom**: StartApp returns timeout or error

**Solutions**:
1. Build the app first: `dotnet build <project-path>`
2. Check GetAppLog() for error details
3. Try running the app manually to verify it works
4. Ensure the path is relative to the solution root

### No UI Tree Returned

**Symptom**: InspectUi returns null or empty

**Solutions**:
1. Wait for app to fully initialize
2. Call Invalidate() to force a redraw
3. Check that the app has a RootView with UI elements
4. Verify app is still running with GetAppLog()

### Click Doesn't Work

**Symptom**: Click doesn't trigger expected behavior

**Solutions**:
1. Use `centerX` and `centerY` instead of `x` and `y`
2. Verify element is visible: `visible="true"`
3. Check element bounds are reasonable (not 0x0)
4. Try Tap() instead of Click() for touch-based interactions

## Advanced Usage

### Multi-Step Test Recording

```
Task: Record a complete user flow as a test

Process:
1. Map out the user flow steps
2. For each step:
   - Inspect UI to find target elements
   - Simulate user action (click, tap, etc.)
   - Wait for UI to update
   - Verify expected state change
   - Capture screenshot
3. Generate C# test code with all interactions
4. Include assertions based on observed behavior
```

### Performance Testing

```
Task: Measure app responsiveness

Process:
1. Start app and record initial state
2. Perform series of interactions
3. Monitor logs for timing information
4. Take screenshots at key moments
5. Analyze response times
```

### UI Exploration Report

```
Task: Generate a comprehensive UI report

Process:
1. Inspect UI tree
2. Count elements by type
3. Identify elements with/without IDs
4. Map navigation structure
5. Generate markdown report with screenshots
```

## Feedback and Contributions

If you have suggestions for improving the MCP integration or GitHub Copilot experience:

1. Open an issue describing the improvement
2. Include example scenarios and expected behavior
3. Contribute enhancements via pull request

## Resources

- [MCP Server README](../Xui/Utils/MCP/README.md)
- [Model Context Protocol Specification](https://spec.modelcontextprotocol.io/)
- [GitHub Copilot Documentation](https://docs.github.com/copilot)
- [Xui Architecture](../AGENTS.md)

## Support

For questions or issues:
1. Check the [MCP README](../Xui/Utils/MCP/README.md)
2. Review [CONTRIBUTING.md](../CONTRIBUTING.md)
3. Open an issue with the `mcp` label
