$(function () {
	var chat = $.connection.commentHub;
	chat.client.addNewCommentToPage = function (idComment, name, message, dateTime) { 
		$('#comment-list').append(
			'<li class="content-list-item" id="' + htmlEncode(idComment) + '" >' +
			'<div class="comment">' +
			'<div class="comment-head">' +
			'<a href="" class="user-info">' +
			'<img src="">' +
			'<span>' +
			htmlEncode(name) +
			'</span>' +
			'</a>' +
			'<time>' +
			htmlEncode(dateTime) +
			'</time >' +
			'</div >' +
			'<div class="comment-message">' +
			htmlEncode(message) +
			'</div>' +
			'<div class="comment-footer">' +
			'</div>' +
			'</div >' +
			'</li>'
		);
	};

	chat.client.deleteCommentToPage = function (idComment) {
		var deleteComment = document.getElementById(idComment);
		deleteComment.parentNode.removeChild(deleteComment);
	}

	// Открываем соединение
	$.connection.hub.start().done(function () {

		$('#sendcomment').click(function () {
			// Вызываем у хаба метод Send
			chat.server.send($('#newsId').val(), $('#userId').val(), $('#comment').val(), $('#userName').val());
			$('#comment').val('').focus();
		});
		$('#deletecomment').click(function () {
			// Вызываем у хаба метод Delete
			chat.server.delete($('#commentId').val());
		});
	});
});
// Кодирование тегов
function htmlEncode(value) {
	var encodedValue = $('<div />').text(value).html();
	return encodedValue;
}