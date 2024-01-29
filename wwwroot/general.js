export function Create() {
	return {
		window_innerWidth_get: function () { return window.innerWidth },
		window_innerHeight_get: function () { return window.innerHeight },
		
		window_outerWidth_get: function () { return window.outerWidth },
		window_outerHeight_get: function () { return window.outerHeight },

		prompt: function (message, _default ) { return prompt(message, _default) },
	}
}