# TestApp Integration Scenarios

This index lists the TestApp integration scenario baselines and links to each scenario folder.
Use it to quickly navigate expected snapshots and scenario notes.

## Table of contents

- [Navigation flows](#navigation-flows)
- [Layout and overlays](#layout-and-overlays)
- [TextBox interactions](#textbox-interactions)
- [Regression checks](#regression-checks)

## Navigation flows

- [HomePage_Renders](./HomePage_Renders/README.md)  
  Renders the TestApp home page as the baseline startup state.
- [Navigate_To_TextMetrics](./Navigate_To_TextMetrics/README.md)  
  Captures hover/press transitions while navigating from home to TextMetrics.
- [Navigate_Through_All](./Navigate_Through_All/README.md)  
  Walks through all available example pages and snapshots each destination.

## Layout and overlays

- [Grid_Scenarios](./Grid_Scenarios/README.md)  
  Navigates to Grid Layout and captures each documented grid variant.
- [Overlay_OpenAndDismiss](./Overlay_OpenAndDismiss/README.md)  
  Opens the overlay demo popup and verifies outside-click dismissal behavior.

## TextBox interactions

- [TextBox_Focus](./TextBox_Focus/README.md)  
  Focuses NameBox and validates focus visuals.
- [TextBox_Type](./TextBox_Type/README.md)  
  Types incrementally in NameBox and snapshots text growth.
- [TextBox_Backspace](./TextBox_Backspace/README.md)  
  Exercises backspace edits and follow-up typing.
- [TextBox_Password](./TextBox_Password/README.md)  
  Verifies masked rendering while typing in PasswordBox.
- [TextBox_SwitchFocus](./TextBox_SwitchFocus/README.md)  
  Switches focus between NameBox and PasswordBox while preserving values.
- [TextBox_TabNavigation](./TextBox_TabNavigation/README.md)  
  Uses Tab and Shift+Tab to validate forward/backward focus traversal.
- [TextBox_KeyboardSelection](./TextBox_KeyboardSelection/README.md)  
  Uses keyboard range selection and replacement in NameBox.
- [TextBox_MouseSelection](./TextBox_MouseSelection/README.md)  
  Drag-selects text with mouse down/move/up interactions.

## Regression checks

- [Pending_Hover_After_Navigation](./Pending_Hover_After_Navigation/README.md)  
  Reproduces stale hover state after navigating away and returning home.
- [Heartbeat_Stops_After_Mouse_Over_Back](./Heartbeat_Stops_After_Mouse_Over_Back/README.md)  
  Validates that heart animation continues ticking after hovering the Back button.
