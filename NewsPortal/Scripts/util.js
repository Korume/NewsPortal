$(function () {
	var chat = $.connection.commentHub;
	chat.client.addNewMessageToPage = function (idNews, name, message, dateTime) { 
		$('#comment-list').append(
			'<li class="content-list-item" id="' + htmlEncode(idNews) + '" >' +
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

	// Открываем соединение
	$.connection.hub.start().done(function () {

		$('#sendcomment').click(function () {
			// Вызываем у хаба метод Send
			chat.server.send($('#newsId').val(), $('#userId').val(), $('#comment').val(), $('#userName').val());
			$('#comment').val('').focus();
		});
	});
});
// Кодирование тегов
function htmlEncode(value) {
	var encodedValue = $('<div />').text(value).html();
	return encodedValue;
}