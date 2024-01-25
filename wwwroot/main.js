import { dotnet } from './_framework/dotnet.js'
import * as canvasLib from './canvas.js'

const canvas = document.querySelector('canvas')

const canvasContext = canvas.getContext('2d', { alpha: false, willReadFrequently: false })
window.canvasContext = canvasContext

const { setModuleImports, getAssemblyExports, getConfig } = await dotnet
    .withDiagnosticTracing(false)
    .withApplicationArgumentsFromQuery()
    .create()

setModuleImports('canvas.js', canvasLib.Create(canvas, canvasContext))

const config = getConfig()
const exports = await getAssemblyExports(config.mainAssemblyName)

const exportedProgram = exports.YeahGame?.Web?.Program

function RefreshCanvasSize() {
    canvas.width = window.innerWidth
    canvas.height = window.innerHeight
    if (exportedProgram?.OnResize) {
        exportedProgram.OnResize()
    }
}

RefreshCanvasSize()

window.addEventListener('resize', function (e) { RefreshCanvasSize() })

if (exportedProgram?.OnKeyDown) {
    document.addEventListener('keydown', function (e) {
        exportedProgram.OnKeyDown(e.keyCode)
    })
}

if (exportedProgram?.OnKeyUp) {
    document.addEventListener('keyup', function (e) {
        exportedProgram.OnKeyUp(e.keyCode)
    })
}

if (exportedProgram?.OnMouseMove) {
    canvas.addEventListener('mousemove', function (e) {
        exportedProgram.OnMouseMove(e.offsetX, e.offsetY, e.button, e.ctrlKey)
    })
}

if (exportedProgram?.OnMouseDown) {
    canvas.addEventListener('mousedown', function (e) {
        exportedProgram.OnMouseDown(e.offsetX, e.offsetY, e.button, e.ctrlKey)
    })
}

if (exportedProgram?.OnMouseUp) {
    canvas.addEventListener('mouseup', function (e) {
        exportedProgram.OnMouseUp(e.offsetX, e.offsetY, e.button, e.ctrlKey)
    })
}

if (exportedProgram?.OnMouseLeave) {
    canvas.addEventListener('mouseleave', function (e) {
        exportedProgram.OnMouseLeave(e.offsetX, e.offsetY, e.button, e.ctrlKey)
    })
}

if (exportedProgram?.OnAnimation) {
    /**
     * @param {DOMHighResTimeStamp} time
     */
    function OnAnimation(time) {
        exportedProgram.OnAnimation(time)
        requestAnimationFrame(OnAnimation)
    }

    requestAnimationFrame(OnAnimation)
}

await dotnet.run()
