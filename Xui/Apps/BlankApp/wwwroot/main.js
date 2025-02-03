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
                        reset() {
                            xuiCanvasContext.reset();
                        },
                        setGlobalAlpha(globalAlpha) {
                            xuiCanvasContext.globalAlpha = globalAlpha;
                        },
                        setFillStyle(fillStyle) {
                            xuiCanvasContext.fillStyle = fillStyle;
                        },
                        setStrokeStyle(strokeStyle) {
                            xuiCanvasContext.strokeStyle = strokeStyle;
                        },
                        fillRect(x, y, width, height) {
                            xuiCanvasContext.fillRect(x, y, width, height);
                        },
                        strokeRect(x, y, width, height) {
                            xuiCanvasContext.strokeRect(x, y, width, height);
                        },
                        setFont(font) {
                            xuiCanvasContext.font = font;
                        },
                        setLineCap(lineCap) {
                            xuiCanvasContext.lineCap = lineCap;
                        },
                        setLineJoin(lineJoin) {
                            xuiCanvasContext.lineJoin = lineJoin;
                        },
                        setLineWidth(lineWidth) {
                            xuiCanvasContext.lineWidth = lineWidth;
                        },
                        setLineMiterLimit(miterLimit) {
                            xuiCanvasContext.miterLimit = miterLimit;
                        },
                        setLineWidth(lineDashOffset) {
                            xuiCanvasContext.lineDashOffset = lineDashOffset;
                        },
                        arc(x, y, radius, startAngle, endAngle, counterclockwise) {
                            xuiCanvasContext.arc(x, y, radius, startAngle, endAngle, counterclockwise);
                        },
                        arcTo(x1, y1, x2, y2, radius) {
                            xuiCanvasContext.arcTo(x1, y1, x2, y2, radius);
                        },
                        beginPath() {
                            xuiCanvasContext.beginPath();
                        },
                        clip() {
                            xuiCanvasContext.clip();
                        },
                        closePath() {
                            xuiCanvasContext.closePath();
                        },
                        quadraticCurveTo(cpx, cpy, x, y) {
                            xuiCanvasContext.quadraticCurveTo(cpx, cpy, x, y);
                        },
                        bezierCurveTo(cp1x, cp1y, cp2x, cp2y, x, y) {
                            xuiCanvasContext.bezierCurveTo(cp1x, cp1y, cp2x, cp2y, x, y)
                        },
                        ellipse(x, y, radiusX, radiusY, rotation, startAngle, endAngle, counterclockwise) {
                            xuiCanvasContext.ellipse(x, y, radiusX, radiusY, rotation, startAngle, endAngle, counterclockwise);
                        },
                        fill(fillRule) {
                            xuiCanvasContext.fill(fillRule);
                        },
                        fillText(text, x, y) {
                            xuiCanvasContext.fillText(text, x, y);
                        },
                        lineTo(x, y) {
                            xuiCanvasContext.lineTo(x, y);
                        },
                        measureText(text) {
                            const metrics = xuiCanvasContext.measureText(text);
                            return { width: metrics.width, height: metrics.emHeightAscent + metrics.emHeightDescent }
                        },
                        moveTo(x, y) {
                            xuiCanvasContext.moveTo(x, y);
                        },
                        rect(x, y, width, height) {
                            xuiCanvasContext.rect(x, y, width, height);
                        },
                        restore() {
                            xuiCanvasContext.restore();
                        },
                        rotate(angle) {
                            xuiCanvasContext.rotate(angle);
                        },
                        roundRect(x, y, width, height, radii) {
                            xuiCanvasContext.rotate(x, y, width, height, radii);
                        },
                        roundRect4(x, y, width, height, topLeft, topRight, bottomRight, bottomLeft) {
                            xuiCanvasContext.rotate(x, y, width, height, [topLeft, topRight, bottomRight, bottomLeft]);
                        },
                        save() {
                            xuiCanvasContext.save();
                        },
                        scale(x, y) {
                            xuiCanvasContext.scale(x, y);
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