# Xui MCP (Model Context Protocol) Server

The Xui MCP server provides AI agents (like GitHub Copilot) with tools to interact with running Xui applications for testing, exploration, and documentation purposes.

## Features

- **App Lifecycle Management**: Start, stop, and connect to Xui applications
- **UI Inspection**: Retrieve the visual UI tree in XML format
- **Screenshot Capture**: Take SVG screenshots of the running app
- **Input Simulation**: Click, tap, and send pointer events
- **App Logging**: Access stdout/stderr from running apps
- **Knowledge Base**: Access Xui API documentation and guides

## Quick Start

### Running the MCP Server

The MCP server uses stdio transport and can be run directly:

```bash
dotnet run --project Xui/Utils/MCP/Xui.MCP.csproj
```

### Installing as a Tool

You can install the MCP server as a .NET tool:

```bash
dotnet tool install --global Xui.MCP
xui-mcp
```

## GitHub Copilot Integration

GitHub Copilot can use this MCP server to interact with Xui applications. To enable this integration:

### 1. Configure the MCP Server

The repository includes a `.mcp.json` configuration file at the root:

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

### 2. Using with GitHub Copilot Agents

GitHub Copilot agents can discover and use MCP servers through the following methods:

#### Option A: Repository Configuration (Recommended)
The `.mcp.json` file in the repository root allows GitHub Copilot to automatically discover and use the MCP server when working in this repository.

#### Option B: Organization/User Settings
Contact your GitHub organization administrator to add the Xui MCP server to your organization's approved MCP servers list.

#### Option C: Manual Connection
When GitHub Copilot asks for tools or capabilities, mention:
> "Use the Xui MCP server at `dotnet run --project Xui/Utils/MCP/Xui.MCP.csproj` to interact with Xui apps"

### 3. Example Usage

Once configured, you can ask GitHub Copilot to:

- **Explore UI**: "Inspect the TestApp UI tree and describe the layout"
- **Test Interactions**: "Click the submit button and verify the response"
- **Record Tests**: "Create a test that navigates through the app and validates each screen"
- **Debug Issues**: "Start the BlankApp and check if there are any crashes in the logs"

## MCP Tools

### App Lifecycle

- **StartApp**: Launch a Xui app with DevTools enabled
  ```
  StartApp(projectPath: "Xui/Apps/TestApp/TestApp.Desktop.csproj")
  ```

- **ConnectApp**: Connect to an already-running app by pipe name
  ```
  ConnectApp(pipeName: "xui-devtools-12345")
  ```

- **StopApp**: Stop the running app

### UI Inspection

- **InspectUi**: Get the visual UI tree in XML format
  
  Returns an XML structure like:
  ```xml
  <?xml version="1.0" encoding="utf-8"?>
  <Canvas id="main-canvas" class="app-root" x="0.00" y="0.00" w="400.00" h="800.00" centerX="200.00" centerY="400.00" visible="true">
    <Button id="btn-submit" class="primary" x="10.00" y="20.00" w="100.00" h="50.00" centerX="60.00" centerY="45.00" visible="true" />
    <Text x="10.00" y="80.00" w="200.00" h="30.00" centerX="110.00" centerY="95.00" visible="true" />
  </Canvas>
  ```

  Key attributes:
  - **Element name**: View type (Canvas, Button, Text, etc.)
  - **id**: Unique identifier (if set)
  - **class**: Space-separated class names (if set)
  - **x, y, w, h**: Position and size
  - **centerX, centerY**: Center point for clicking (prevents edge clicks)
  - **visible**: Visibility state

- **Screenshot**: Capture an SVG screenshot

### Input Simulation

- **Click**: Simulate a mouse click at coordinates
  ```
  Click(x: 60.0, y: 45.0)
  ```

- **Tap**: Simulate a touch tap at coordinates
  ```
  Tap(x: 60.0, y: 45.0)
  ```

- **Pointer**: Send synthetic pointer events (start, move, end, cancel)
  ```
  Pointer(phase: "start", x: 60.0, y: 45.0, index: 0)
  ```

### App Monitoring

- **GetAppLog**: Retrieve recent stdout/stderr output
- **Identify**: Set an AI identity label for the pointer overlay
- **Invalidate**: Force a redraw of the app

## XML Format Benefits

The XML format provides several advantages over JSON for LLM interaction:

1. **Human Readable**: Easier for both humans and LLMs to parse
2. **Semantic Structure**: Element names directly represent View types
3. **Center Points**: `centerX` and `centerY` attributes provide safe click targets
4. **Easy Selection**: `id` and `class` attributes enable CSS-like selection
5. **Hierarchical**: Nested structure clearly shows parent-child relationships

### Locating Elements

Use the `id` or `class` attributes to locate elements:

```xml
<!-- Find by ID -->
<Button id="btn-submit" ... />

<!-- Find by class -->
<Button class="primary large" ... />
<Text class="primary" ... />
```

### Clicking Elements

Always use the center point coordinates for clicking:

```xml
<Button id="btn-submit" centerX="60.00" centerY="45.00" ... />
```

Then: `Click(x: 60.0, y: 45.0)`

This prevents issues with clicking at element edges or margins.

## Development

### Building

```bash
dotnet build Xui/Utils/MCP/Xui.MCP.csproj
```

### Testing

Start a demo app with DevTools:
```bash
dotnet run --project Xui/Apps/TestApp/TestApp.Desktop.csproj -c Debug
```

In another terminal, start the MCP server and use the tools to interact with the app.

## Architecture

The MCP server communicates with Xui apps through:

1. **Process Management**: Launches apps with `dotnet run`
2. **Named Pipes**: Connects to DevTools via JSON-RPC over named pipes
3. **DevTools Protocol**: Uses the Xui DevTools protocol for UI inspection and input

```
GitHub Copilot
    ↓ (MCP stdio)
MCP Server (AppTools)
    ↓ (named pipe JSON-RPC)
DevTools Window
    ↓
Xui Application
```

## Troubleshooting

### App Won't Start
- Check that the project path is correct and relative to the solution root
- Ensure the app builds successfully: `dotnet build <project-path>`
- Check logs with `GetAppLog()`

### Connection Timeout
- Increase the 90-second timeout if the app takes longer to start
- Check that DevTools is enabled (Debug configuration)

### No UI Tree
- Ensure the app has a RootView with UI elements
- Check that the app is fully initialized before calling InspectUi

## Contributing

See [CONTRIBUTING.md](../../../CONTRIBUTING.md) for development guidelines.

## License

See [LICENSE.md](../../../LICENSE.md) for license information.
