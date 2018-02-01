var ARTICLE_SHOW_CHECK = false;
var MAIN_NEWS_CHECK = false;

(function ($, undefined) {

	var mainContent = {
		Title: $('#Title'),
		Content: "",
		Image: ""
	};
	var blockPreview = {
		Article: $('#Article + article'),
		MainNews: $('#MainContent + .main-news-items'),
	};
	var buttons = {
		article: $('#Article'),
		pageNews: $('#MainContent'),
		fileImage: $('input[type="file"]'),
	};
	var preview = {
		articleTitle: $('#Article + article .Title a'),
		mainNewsTitle: $('.main-news-items .main-news-items-head > h1'),
		mainNewsContent: $('.main-news-items .main-news-items-content .text'),
		mainNewsImage: $('.main-news-items .main-news-items-content .image')
	};

	focusMainBlocks();

	buttons.article.on('click', showArticle);
	buttons.pageNews.on('click', showPageNews);
	buttons.fileImage.on('change', showImage);

	function showArticle() {
		if (!ARTICLE_SHOW_CHECK) {
			ARTICLE_SHOW_CHECK = true;
			blockPreview.Article.toggleClass('preview-block preview-none');
			preview.articleTitle.text(mainContent.Title.val());
		}
		else {
			ARTICLE_SHOW_CHECK = false;
			blockPreview.Article.toggleClass('preview-none preview-block');
		}
	};

	function showPageNews() {
		if (!MAIN_NEWS_CHECK) {
			MAIN_NEWS_CHECK = true;
			blockPreview.MainNews.toggleClass('preview-none preview-block');
			blockPreview.MainNews.css('display', 'block');
			preview.mainNewsTitle.text(mainContent.Title.val());
			preview.mainNewsContent.html(CKEDITOR.instances.Content.getData());
		}
		else {
			MAIN_NEWS_CHECK = false;
			blockPreview.MainNews.toggleClass('preview-none preview-block');
		}
	};

	function showImage() {
		mainContent.Image = buttons.fileImage.val().replace(/.+[\\\/]/, "");
		if (mainContent.Image.length != 0) {
			if (!$('*').is(preview.mainNewsImage.children('img'))) {
				var imgElement = document.createElement('img');
				preview.mainNewsImage.append(imgElement);
			}
			preview.mainNewsImage.children('img').attr({ src: mainContent.Image, alt: 'imageMainNews' });
			disablePreview();
		}
	};

	function focusMainBlocks() {
		mainContent.Title.on('focus', disablePreview);

		CKEDITOR.on('instanceReady', function (event) {
			event.editor.on('focus', disablePreview);
		});
	};
	
	function disablePreview() {
		if (ARTICLE_SHOW_CHECK) {
			ARTICLE_SHOW_CHECK = false;
			blockPreview.Article.toggleClass('preview-none preview-block');
		}
		if (MAIN_NEWS_CHECK) {
			MAIN_NEWS_CHECK = false;
			blockPreview.MainNews.toggleClass('preview-none preview-block');
		}
	};

})(jQuery);
