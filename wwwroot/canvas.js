/**
 * @param {HTMLCanvasElement} canvas
 * @param {CanvasRenderingContext2D} ctx
 */
export function Create(canvas, ctx) {
	return {
		fillStyle_set: function (fillStyle) { ctx.fillStyle = fillStyle },
		fillRect: function (x, y, width, height) { ctx.fillRect(x, y, width, height) },
		strokeRect: function (x, y, width, height) { ctx.strokeRect(x, y, width, height) },
		clearRect: function (x, y, width, height) { ctx.clearRect(x, y, w, h) },
		ellipse1: function (x, y, radiusX, radiusY, rotation, startAngle, endAngle) { ctx.ellipse(x, y, radiusX, radiusY, rotation, startAngle, endAngle) },
		ellipse2: function (x, y, radiusX, radiusY, rotation, startAngle, endAngle, counterclockwise) { ctx.ellipse(x, y, radiusX, radiusY, rotation, startAngle, endAngle, counterclockwise) },
		fillText1: function (text, x, y) { ctx.fillText(text, x, y) },
		fillText2: function (text, x, y, maxWidth) { ctx.fillText(text, x, y, maxWidth) },
		strokeText1: function (text, x, y) { ctx.strokeText(text, x, y) },
		strokeText2: function (text, x, y, maxWidth) { ctx.strokeText(text, x, y, maxWidth) },
		lineWidth_set: function (v) { ctx.lineWidth = v },
		beginPath: function () { ctx.beginPath() },
		moveTo: function (x, y) { ctx.moveTo(x, y) },
		lineTo: function (x, y) { ctx.lineTo(x, y) },
		closePath: function () { ctx.closePath() },
		stroke: function () { ctx.stroke() },
		font_set: function (font) { ctx.font = font },
		fill: function () { ctx.fill() },
		clear: function () { ctx.fillRect(0, 0, canvas.width, canvas.height) },

		width_get: function () { return canvas.width },
		height_get: function () { return canvas.height },

		width_set: function (value) { canvas.width = value },
		height_set: function (value) { canvas.height = value },

		data_get: function () { return ctx.getImageData(0, 0, canvas.width, canvas.height).data },
		data_set: function (data) { ctx.putImageData(new ImageData(data, canvas.width), 0, 0) },
	}
}