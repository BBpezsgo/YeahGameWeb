import { dotnet } from './_framework/dotnet.js'
import * as canvasLib from './canvas.js'

const canvas = document.querySelector('canvas')
// const labelFps = document.getElementById('label-fps')

const canvasContext = canvas.getContext('2d', { alpha: false, willReadFrequently: false })
window.canvasContext = canvasContext

const { setModuleImports, getAssemblyExports, getConfig } = await dotnet
    .withDiagnosticTracing(false)
    .withApplicationArgumentsFromQuery()
    .create()

setModuleImports('canvas.js', canvasLib.Create(canvas, canvasContext))

const config = getConfig()
const exports = await getAssemblyExports(config.mainAssemblyName)

function RefreshCanvasSize() {
    canvas.width = window.innerWidth
    canvas.height = window.innerHeight
    if (exports.WasmExp?.Program?.OnResize) {
        exports.WasmExp.Program.OnResize()
    }
}

RefreshCanvasSize()

window.addEventListener('resize', function (e) { RefreshCanvasSize() })

if (exports.WasmExp?.Program?.OnKeyDown) {
    document.addEventListener('keydown', function (e) {
        exports.WasmExp.Program.OnKeyDown(e.keyCode)
    })
}

if (exports.WasmExp?.Program?.OnKeyUp) {
    document.addEventListener('keyup', function (e) {
        exports.WasmExp.Program.OnKeyUp(e.keyCode)
    })
}

if (exports.WasmExp?.Program?.OnMouseMove) {
    canvas.addEventListener('mousemove', function (e) {
        exports.WasmExp.Program.OnMouseMove(e.offsetX, e.offsetY, e.button, e.ctrlKey)
    })
}

if (exports.WasmExp?.Program?.OnMouseDown) {
    canvas.addEventListener('mousedown', function (e) {
        exports.WasmExp.Program.OnMouseDown(e.offsetX, e.offsetY, e.button, e.ctrlKey)
    })
}

if (exports.WasmExp?.Program?.OnMouseUp) {
    canvas.addEventListener('mouseup', function (e) {
        exports.WasmExp.Program.OnMouseUp(e.offsetX, e.offsetY, e.button, e.ctrlKey)
    })
}

if (exports.WasmExp?.Program?.OnMouseLeave) {
    canvas.addEventListener('mouseleave', function (e) {
        exports.WasmExp.Program.OnMouseLeave(e.offsetX, e.offsetY, e.button, e.ctrlKey)
    })
}

if (exports.WasmExp?.Program?.OnAnimation) {
    // let lastTime = 0

    /**
     * @param {DOMHighResTimeStamp} time
     */
    function OnAnimation(time) {
        exports.WasmExp.Program.OnAnimation(time)

        // const deltaTime = time - lastTime
        // lastTime = time
        // labelFps.textContent = Math.round(1 / (deltaTime / 1000)).toString()

        requestAnimationFrame(OnAnimation)
    }

    requestAnimationFrame(OnAnimation)
}

if (exports.WasmExp?.Program?.OnTick) {
    const ticker = setInterval(function() {
        exports.WasmExp.Program.OnTick()
    }, 10)
}

await dotnet.run()
