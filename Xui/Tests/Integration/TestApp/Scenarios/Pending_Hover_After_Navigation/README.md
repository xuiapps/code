# Integration Test Run

## Timeline

# TestApp

Open the test app to see the menu with SDK examples.

## Scenario

Reproduce stale hover state after navigating to NestedStacks and back.

### 01. HomePage

<img src="./01.HomePage.Render.svg" alt="01. HomePage expected" />

### 02. HoverNestedStacks

<img src="./02.HoverNestedStacks.Render.svg" alt="02. HoverNestedStacks expected" />

### 03. PressedNestedStacks

<img src="./03.PressedNestedStacks.Render.svg" alt="03. PressedNestedStacks expected" />

### 04. NestedStacks

<img src="./04.NestedStacks.Render.svg" alt="04. NestedStacks expected" />

### 05. HoverBack

<img src="./05.HoverBack.Render.svg" alt="05. HoverBack expected" />

### 06. PressHoverBack

<img src="./06.PressHoverBack.Render.svg" alt="06. PressHoverBack expected" />

### 07. ClickedHomePage

<img src="./07.ClickedHomePage.Render.svg" alt="07. ClickedHomePage expected" />

```
Expected: returning home should clear hover state from removed views.
Validation: move mouse to a neutral position and snapshot visual state.
```

### 08. MouseMovedAway

<img src="./08.MouseMovedAway.Render.svg" alt="08. MouseMovedAway expected" />
