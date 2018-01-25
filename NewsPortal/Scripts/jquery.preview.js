﻿var articleShowCheck = false;
var mainNewsCheck = false;

var article = document.getElementById('previewArticle');
var mainNews = document.getElementById('previewMainNews');

document.getElementById('Title').onfocus = function () {
	disablePreview();
	console.log("Okay");
};
CKEDITOR.on('instanceReady', function (evt) {
	var editor = evt.editor;
	editor.on('focus', function () {
		disablePreview();
	});
});	

function disablePreview() {
	if (articleShowCheck) {
		articleShowCheck = false;
		article.style.display = "none";
	}

	if (mainNews) {
		mainNewsCheck = false;
		mainNews.style.display = "none";
	}			
};

document.getElementById('ArticleShow').onclick = function () {

	if (!articleShowCheck) {
		articleShowCheck = true;
		article.style.display = "block";
		document.getElementById('TitlePreview').innerHTML = document.getElementById('Title').value;
	}
	else {
		articleShowCheck = false;
		article.style.display = "none";
	}
};

document.getElementById('MainNewsShow').onclick = function () {
	if (!mainNewsCheck) {
		mainNewsCheck = true;
		mainNews.style.display = "block";
		document.getElementById('TitlePreview-MainNews').innerHTML = document.getElementById('Title').value;
		var Context = CKEDITOR.instances.Content.getData();
		document.getElementById('textPreview').innerHTML = Context;
	}
	else {
		mainNewsCheck = false;
		mainNews.style.display = "none";
	}
};