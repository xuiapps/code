// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

import { dotnet } from './_framework/dotnet.js'

const { setModuleImports, getAssemblyExports, getConfig, runMain } = await dotnet
    .withApplicationArguments("start")
    .create();

const xuiCanvas = document.getElementById("xui-canvas");
const xuiCanvasContext = xuiCanvas.getContext("2d");

setModuleImports('main.js', {
    dom: {
        setInnerText: (selector, time) => document.querySelector(selector).innerText = time
    },
    Xui: {
        Runtime: {
            Browser: {
                Actual: {
                    BrowserWindow: {
                        setTitle(title) {
                            document.title = title;
                        }
                    },
                    BrowserDrawingContext: {
                        setFillStyle(fillStyle) {
                            xuiCanvasContext.fillStyle = fillStyle;
                        },
                        fillRect(x, y, width, height) {
                            xuiCanvasContext.fillRect(x, y, width, height);
                        }
                    }
                }
            }
        }
    }
});

const xui = await getAssemblyExports("Xui.Runtime.Browser");
const onAnimationFrame = xui.Xui.Runtime.Browser.Actual.BrowserWindow.OnAnimationFrame;
const onMouseMove = xui.Xui.Runtime.Browser.Actual.BrowserWindow.OnMouseMove;

const config = getConfig();
const exports = await getAssemblyExports(config.mainAssemblyName);

// document.getElementById('reset').addEventListener('click', e => {
//     exports.StopwatchSample.Reset();
//     e.preventDefault();
// });

// const pauseButton = document.getElementById('pause');
// pauseButton.addEventListener('click', e => {
//     const isRunning = exports.StopwatchSample.Toggle();
//     pauseButton.innerText = isRunning ? 'Pause' : 'Start';
//     e.preventDefault();
// });

xuiCanvas.addEventListener("mousemove", event => {
    onMouseMove(event.offsetX, event.offsetY);
});

const onAnimationFrameCallback = (timestamp) => {
    onAnimationFrame(timestamp);
    window.requestAnimationFrame(onAnimationFrameCallback);
};
window.requestAnimationFrame(onAnimationFrameCallback);

await runMain();