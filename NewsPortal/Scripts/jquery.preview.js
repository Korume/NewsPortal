var target = document.getElementById('Title');

var observer = new MutationObserver(function (mutations) {
	mutations.forEach(function (mutation) {
		alert("1");
		var title = document.getElementById('Title').value;
		document.getElementById('TitlePreview').innerHTML = title; 
		document.getElementById('TitlePreview-MainNews').innerHTML = title;
	});
});

var config = { attributes: true, childList: true, characterData: true }

observer.observe(target, config);

//document.getElementById('Title').addEventListener("DOMSubtreeModified", function () {
//	var title = document.getElementById('Title').value;
//	document.getElementById('TitlePreview').innerHTML = title; 
//	document.getElementById('TitlePreview-MainNews').innerHTML = title;
//});

function handleFileSelect() {
	var files = document.getElementById('fileImage').target.files; // FileList object

		// Only process image files.
		//if (!files.type.match('image.*')) {
		//	continue;
		//}

		var reader = new FileReader();

		// Closure to capture the file information.
		reader.onload = (function (theFile) {
			return function (e) {
				// Render thumbnail.
				var classImage = document.getElementsByClassName('.image');
				classImage.innerHTML = ['<img class="imagePreview" src="', e.target.result,
					'" title="', theFile.name, '"/>'].join('');
				//document.getElementById('list').insertBefore(span, null);
			};
		})(files);
		// Read in the image file as a data URL.
		reader.readAsDataURL(f);
}
