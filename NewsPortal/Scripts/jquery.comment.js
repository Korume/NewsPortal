$(function () {
	var chat = $.connection.commentHub;
	chat.client.addNewCommentToPage = function (idComment, name, message, dateTime) {
		$('#comment-list').append(
			'<li class="content-list-item" id="item-' + htmlEncode(idComment) + '" >' +
			'<div class="comment">' +
			'<div class="comment-head">' +
			'<a href="" class="user-info">' +
			'<div class="img-user-info"></div >' +
			'<span>' +
			htmlEncode(name) +
			'</span>' +
			'</a>' +
			'<time>' +
			htmlEncode(dateTime) +
			'</time>' +
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
		$("#item-" + idComment).remove();
	}

	$.connection.hub.start().done(function () {
		$('#sendcomment').click(function () {
			var check = checkComment();
			if (check) {
				chat.server.send($('#newsId').val(), $('#userId').val(), $('#comment').val(), $('#userName').val());
				$('#comment').val('').focus();
			}
			else {
				$('#comment').val('The field must be set!').focus();
			}		
		});

		$(document).on("click", ".deleteComment", function () {
			var id = $(this).parent().attr("id");
			chat.server.delete(id);
		});
	});
});

function checkComment() {
	var valueComment = $('#comment').val();
	if (valueComment != 0) {
		$('#sendcomment').removeAttr('disabled');
		return true;
	}
	else {
		$('#sendcomment').attr('disabled', 'disabled');
		return false;
	}
}

function htmlEncode(value) {
	var encodedValue = $('<div />').text(value).html();
	return encodedValue;
}
