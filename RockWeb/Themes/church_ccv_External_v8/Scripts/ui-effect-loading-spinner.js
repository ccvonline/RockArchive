//<script type="text/javascript" src="https://localhost:44347/Themes/church_ccv_External_v8/Scripts/effects.js"></script>
 function displayLoader() {
	var loaderBg = $(".loader-bg");
	loaderBg.removeClass("loader-bg-hidden");
	loaderBg.addClass("loader-bg-visible");

	var loader = $(".loader");
	loader.removeClass("loader-hidden");
	loader.addClass("loader-visible");
}

function hideLoader() {
	var loaderBg = $(".loader-bg");
	loaderBg.removeClass("loader-bg-visible");
	loaderBg.addClass("loader-bg-hidden");

	var loader = $(".loader");
	loader.removeClass("loader-visible");
	loader.addClass("loader-hidden");
}
//
