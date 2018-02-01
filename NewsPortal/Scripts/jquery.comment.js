var chat = $.connection.commentHub;

chat.client.addNewCommentToPage = function (idComment, name, message, dateTime) {
	createComment(idComment, name, message, dateTime);
};
chat.client.deleteCommentToPage = function (idComment) {
	deleteComment(idComment);
};

$.connection.hub.start().done(function () {
	$('#sendComment').on('click', buttonSendComment);
	$(document).on("click", ".deleteComment", buttonDeleteComment);
});

function buttonSendComment() {
	var checked = checkedContent();
	if (checked) {
		chat.server.send($('#newsId').val(), $('#userId').val(),
			$('#comment').val(), $('#userName').val());

		$('#comment').val('').focus();
	}
	else {
		$('#comment').val('The field must be set!').focus();
	}
};

function buttonDeleteComment() {
	var id = $(this).parent().attr("id");
	chat.server.delete(id);
};

function createComment(idComment, nameUser, message, dateTime) {
	var comment = $('#template-comment').contents().clone();
	comment.attr('id', 'item-' + idComment);
	comment.find('a').children('span').text(nameUser);
	comment.find('time').text(dateTime);
	comment.find('div.comment-menu').attr('id', idComment);
	comment.find('div.comment-message').text(message);
	$('#comment-list').append(comment);
};

function deleteComment(idComment) {
	$("#item-" + idComment).remove();
};

function checkedContent() {
	var valueComment = $('#comment').val();
	var sendComment = $('#sendComment');

	if (valueComment != 0) {
		sendComment.removeAttr('disabled');
		return true;
	}
	else {
		sendComment.attr('disabled', 'disabled');
		return false;
	}
}
