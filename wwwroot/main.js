import { dotnet } from './_framework/dotnet.js'
import * as canvasLib from './canvas.js'
import * as generalLib from './general.js'
import * as p2pLib from './p2plib.js'
import P2P from './p2p.js'
import * as storageLib from './storage.js'

if (!window) { throw new Error('Expected to be running in a browser') }

console.log('Loading .NET ...')
const { setModuleImports, getAssemblyExports, getConfig } = await dotnet
    .withDiagnosticTracing(false)
    .withApplicationArgumentsFromQuery()
    .create()

console.log('Loading JS exports ...')

const canvas = document.querySelector('canvas')

const canvasContext = canvas.getContext('2d', { alpha: false, willReadFrequently: false })
window.canvasContext = canvasContext

const p2p = new P2P()
window.p2p = p2p

setModuleImports('canvas.js', canvasLib.Create(canvas, canvasContext))
setModuleImports('general.js', generalLib.Create())
setModuleImports('p2plib.js', p2pLib.Create(p2p))
setModuleImports('storage.js', storageLib.Create())

console.log('Loading JS imports ...')

const config = getConfig()
const exports = await getAssemblyExports(config.mainAssemblyName)

const exportedProgram = exports.YeahGame?.Web?.Program

console.log('Setting up listeners ...')

if (exportedProgram.WebRTCAnswer) {
    window.Accept = (answer) => { exportedProgram.WebRTCAnswer(answer) }
}

function RefreshCanvasSize() {
    canvas.width = window.innerWidth
    canvas.height = window.innerHeight
    if (exportedProgram?.OnResize) {
        exportedProgram.OnResize()
    }
}

RefreshCanvasSize()

const KeyCodes =
{
    'ArrowUp': 0x26,
    'ArrowLeft': 0x25,
    'ArrowDown': 0x28,
    'ArrowRight': 0x27,

    'NumpadDecimal': 0x6E,
    'Numpad0': 0x60,
    'Numpad1': 0x61,
    'Numpad2': 0x62,
    'Numpad3': 0x63,
    'Numpad4': 0x64,
    'Numpad5': 0x65,
    'Numpad6': 0x66,
    'Numpad7': 0x67,
    'Numpad8': 0x68,
    'Numpad9': 0x69,
    'NumLock': 0x90,
    // 'NumpadEnter'
    'NumpadAdd': 0x6B,
    'NumpadSubtract': 0x6D,
    'NumpadMultiply': 0x6A,
    'NumpadDivide': 0x6F,

    // 'Minus': 0xBD,
    'Slash': 0xBD,
    'Period': 0xBE,
    'Comma': 0xBC,
    'ShiftRight': 0xA1,
    'ControlRight': 0xA3,
    // 'ContextMenu'
    'MetaRight': 0x5C,
    'AltRight': 0xA5,
    'Space': 0x20,
    'AltLeft': 0xA4,
    'MetaLeft': 0x5B,
    'ControlLeft': 0xA2,
    'ShiftLeft': 0xA0,
    'CapsLock': 0x14,
    'Tab': 0x09,
    'Backspace': 0x08,
    'Enter': 0x0D,
    'Insert': 0x2D,
    'Home': 0x24,
    'PageUp': 0x21,
    'PageDown': 0x22,
    'End': 0x23,
    'Delete': 0x2E,
    'Escape': 0x1B,
    'Pause': 0x13,
    'ScrollLock': 0x91,
    
    'F1': 0x70,
    'F2': 0x71,
    'F3': 0x72,
    'F4': 0x73,
    'F5': 0x74,
    'F6': 0x75,
    'F7': 0x76,
    'F8': 0x77,
    'F9': 0x78,
    'F10': 0x79,
    'F11': 0x7A,
    'F12': 0x7B,

    'Digit0': 48,
    'Digit1': 49,
    'Digit2': 50,
    'Digit3': 51,
    'Digit4': 52,
    'Digit5': 53,
    'Digit6': 54,
    'Digit7': 55,
    'Digit8': 56,
    'Digit9': 57,

    'KeyA': 65,
    'KeyB': 66,
    'KeyC': 67,
    'KeyD': 68,
    'KeyE': 69,
    'KeyF': 70,
    'KeyG': 71,
    'KeyH': 72,
    'KeyI': 73,
    'KeyJ': 74,
    'KeyK': 75,
    'KeyL': 76,
    'KeyM': 77,
    'KeyN': 78,
    'KeyO': 79,
    'KeyP': 80,
    'KeyQ': 81,
    'KeyR': 82,
    'KeyS': 83,
    'KeyT': 84,
    'KeyU': 85,
    'KeyV': 86,
    'KeyW': 87,
    'KeyX': 88,
    'KeyY': 89,
    'KeyZ': 90,
}

const DisableyKeys = [
    'F12',
    'F5',
]

/**
 * @param {TouchList} touches
 */
function GetTouches(touches) {
    const xs = []
    const ys = []
    const ids = []
    for (var i = 0; i < touches.length; i++) {
        xs.push(touches[i].clientX)
        ys.push(touches[i].clientY)
        ids.push(touches[i].identifier)
    }
    return { xs, ys, ids }
}

window.addEventListener('resize', function (e) { RefreshCanvasSize() })

if (exportedProgram.OnTouch) {
    canvas.addEventListener('touchstart', function (e) {
        if (!e.touches) { return }
        const data = GetTouches(e.touches)
        exportedProgram.OnTouch(data.xs, data.ys, data.ids)
    })

    canvas.addEventListener('touchend', function (e) {
        if (!e.touches) { return }
        const data = GetTouches(e.touches)
        exportedProgram.OnTouch(data.xs, data.ys, data.ids)
    })

    canvas.addEventListener('touchmove', function (e) {
        if (!e.touches) { return }
        const data = GetTouches(e.touches)
        exportedProgram.OnTouch(data.xs, data.ys, data.ids)
    })

    canvas.addEventListener('touchcancel', function (e) {
        if (!e.touches) { return }
        const data = GetTouches(e.touches)
        exportedProgram.OnTouch(data.xs, data.ys, data.ids)
    })
}

if (exportedProgram?.OnKeyDown) {
    document.addEventListener('keydown', function (e) {
        if (DisableyKeys.includes(e.key)) { return }
        // const vk = KeyCodes[e.code]
        // if (!vk) {
        //     console.warn(`Unknown key ${e.code}`)
        //     return
        // }
        exportedProgram.OnKeyDown(e.keyCode, e.key, e.location, e.altKey, e.ctrlKey, e.shiftKey)
        e.preventDefault()
    })
}

if (exportedProgram?.OnKeyUp) {
    document.addEventListener('keyup', function (e) {
        if (DisableyKeys.includes(e.key)) { return }
        // const vk = KeyCodes[e.code]
        // if (!vk) {
        //     console.warn(`Unknown key ${e.code}`)
        //     return
        // }
        exportedProgram.OnKeyUp(e.keyCode, e.key, e.location, e.altKey, e.ctrlKey, e.shiftKey)
        e.preventDefault()
    })
}

if (exportedProgram?.OnWheel) {
    canvas.addEventListener('wheel', function (e) {
        exportedProgram.OnWheel(e.offsetX, e.offsetY, e.deltaY, e.altKey, e.ctrlKey, e.shiftKey)
    })
}

if (exportedProgram?.OnMouseMove) {
    canvas.addEventListener('mousemove', function (e) {
        exportedProgram.OnMouseMove(e.offsetX, e.offsetY, e.button, e.altKey, e.ctrlKey, e.shiftKey)
    })
}

if (exportedProgram?.OnMouseDown) {
    canvas.addEventListener('mousedown', function (e) {
        exportedProgram.OnMouseDown(e.offsetX, e.offsetY, e.button, e.altKey, e.ctrlKey, e.shiftKey)
    })
}

if (exportedProgram?.OnMouseUp) {
    canvas.addEventListener('mouseup', function (e) {
        exportedProgram.OnMouseUp(e.offsetX, e.offsetY, e.button, e.altKey, e.ctrlKey, e.shiftKey)
    })
}

if (exportedProgram?.OnMouseLeave) {
    canvas.addEventListener('mouseleave', function (e) {
        exportedProgram.OnMouseLeave(e.offsetX, e.offsetY, e.button, e.altKey, e.ctrlKey, e.shiftKey)
    })
}

if (exportedProgram?.OnAnimation) {
    /**
     * @param {DOMHighResTimeStamp} time
     */
    function OnAnimation(time) {
        exportedProgram.OnAnimation()
        requestAnimationFrame(OnAnimation)
    }

    requestAnimationFrame(OnAnimation)
}

console.log('Starting .NET app ...')

await dotnet.run()
