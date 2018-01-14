$(function () {
	var chat = $.connection.commentHub;
	chat.client.addNewCommentToPage = function (idComment, name, message, dateTime) {
		$('#comment-list').append(
			'<li class="content-list-item" id="item-' + htmlEncode(idComment) + '" >' +
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
			'<div class="comment-menu" id="' + htmlEncode(idComment) + '">' + 
			'<input class="deleteComment" type="button" />' +
			'</div>' +
			'</div>' +
			'<div class="comment-message">' +
			htmlEncode(message) +
			'</div>' +
			'<div class="comment-footer">' +
			'</div>' +
			'</div>' +
			'</li>'
		);
	};
	chat.client.deleteCommentToPage = function (idComment) {
		//$('#comment-list').removeClass("comment-" + idComment);
		//document.getElementById('#comment-list').parentNode.removeChild(idComment);
		var deleteComment = document.getElementById('item-' + idComment);
		deleteComment.parentNode.removeChild(deleteComment);
	}
	$.connection.hub.start().done(function () {

		$('#sendcomment').click(function () {
			chat.server.send($('#newsId').val(), $('#userId').val(), $('#comment').val(), $('#userName').val());
			$('#comment').val('').focus();
		});
		$('.deleteComment').click(function () {
			var id = $(this).parent().attr("id");
			chat.server.delete(id);
		});
	});
});

function htmlEncode(value) {
	var encodedValue = $('<div />').text(value).html();
	return encodedValue;
}