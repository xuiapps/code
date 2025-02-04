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
                        setLinearGradientFillStyle(x0, y0, x1, y1, offsets, colors) {
                            let gradient = xuiCanvasContext.createLinearGradient(x0, y0, x1, y1);
                            for (let i = 0; i < colors.length; i++) {
                                gradient.addColorStop(offsets[i], colors[i]);
                            }
                            xuiCanvasContext.fillStyle = gradient;
                        },
                        setRadialGradientFillStyle(x0, y0, r0, x1, y1, r1, offsets, colors) {
                            let gradient = xuiCanvasContext.createRadialGradient(x0, y0, r0, x1, y1, r1);
                            for (let i = 0; i < colors.length; i++) {
                                const offset = offsets[i];
                                const color = colors[i];
                                gradient.addColorStop(offset, color);
                            }
                            xuiCanvasContext.fillStyle = gradient;
                        },
                        setStrokeStyle(strokeStyle) {
                            xuiCanvasContext.strokeStyle = strokeStyle;
                        },
                        setLinearGradientStrokeStyle(x0, y0, x1, y1, offsets, colors) {
                            let gradient = xuiCanvasContext.createLinearGradient(x0, y0, x1, y1);
                            for (let i = 0; i < colors.length; i++) {
                                gradient.addColorStop(offsets[i], colors[i]);
                            }
                            xuiCanvasContext.strokeStyle = gradient;
                        },
                        setRadialGradientStrokeStyle(x0, y0, r0, x1, y1, r1, offsets, colors) {
                            let gradient = xuiCanvasContext.createRadialGradient(x0, y0, r0, x1, y1, r1);
                            for (let i = 0; i < colors.length; i++) {
                                gradient.addColorStop(offsets[i], colors[i]);
                            }
                            xuiCanvasContext.strokeStyle = gradient;
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
                            xuiCanvasContext.roundRect(x, y, width, height, radii);
                        },
                        roundRect4(x, y, width, height, topLeft, topRight, bottomRight, bottomLeft) {
                            xuiCanvasContext.roundRect(x, y, width, height, [topLeft, topRight, bottomRight, bottomLeft]);
                        },
                        save() {
                            xuiCanvasContext.save();
                        },
                        scale(x, y) {
                            xuiCanvasContext.scale(x, y);
                        },
                        translate(x, y) {
                            xuiCanvasContext.translate(x, y);
                        },
                        transform(a, b, c, d, e, f) {
                            xuiCanvasContext.transform(a, b, c, d, e, f);
                        },
                        setTransform(a, b, c, d, e, f) {
                            xuiCanvasContext.setTransform(a, b, c, d, e, f);
                        },
                        stroke() {
                            xuiCanvasContext.stroke();
                        },
                        setLineDash(segments) {
                            xuiCanvasContext.setLineDash(segments);
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
const onWheel = xui.Xui.Runtime.Browser.Actual.BrowserWindow.OnWheel;

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
    event.preventDefault();
});
xuiCanvas.addEventListener("wheel", event => {
    onWheel(event.offsetX, event.offsetY, -event.deltaX, -event.deltaY);
    event.preventDefault();
});

const onAnimationFrameCallback = (timestamp) => {
    const w = xuiCanvas.offsetWidth;
    const h = xuiCanvas.offsetHeight;
    if (xuiCanvas.width != w) {
        xuiCanvas.width = w;
    }
    if (xuiCanvas.height != h) {
        xuiCanvas.height = h;
    }
    onAnimationFrame(xuiCanvas.width, xuiCanvas.height, timestamp);
    window.requestAnimationFrame(onAnimationFrameCallback);
};
window.requestAnimationFrame(onAnimationFrameCallback);

await runMain();