var dialog = document.querySelector('dialog');
document.querySelector('#show-pop-up-window').onclick = function () {
	dialog.showModal();
};
document.querySelector('#close-pop-up-window').onclick = function () {
	dialog.close();
};

