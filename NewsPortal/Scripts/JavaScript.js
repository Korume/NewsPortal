var dialog = document.querySelector('dialog');
document.querySelector('#show-pop-up-window').onclick = function () {
	dialog.showModal();
};
document.querySelector('#close-pop-up-window').onclick = function () {
﻿function Open(text) {
	var dialog = document.getElementById(text);
	dialog.showModal();
}
function Close(text) {
	var dialog = document.querySelector(text);
	dialog.close();
};

