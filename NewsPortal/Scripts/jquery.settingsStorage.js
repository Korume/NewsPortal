(function ($, undefined) {
	var firstElement = $('select').children('option:first-child');

	firstElement.attr('disabled', 'disabled');
	//firstElement.attr('selected', 'selected');
})(jQuery);