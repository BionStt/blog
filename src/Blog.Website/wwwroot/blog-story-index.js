var blogStoryManager = {
    publishIcon: 'fa-eye',
    publishTitle: 'Publish',
    publishText: 'Published',
    publishClass: 'status-publish',
    unpublishIcon: 'fa-eye-slash',
    unpublishTitle: 'Unpublish',
    unpublishText: 'Draft',
    unpublishClass: 'status-draft',
    initialize: function () {
        var swithCtrl = function ($control, isPublish, $valueControl) {
            var puI = blogStoryManager.publishIcon;
            var unpI = blogStoryManager.unpublishIcon;
            var pucl = blogStoryManager.publishClass;
            var unpcl = blogStoryManager.unpublishClass;

            if (isPublish) {
                $control.attr('title', blogStoryManager.unpublishTitle);
                $control.attr('status', blogStoryManager.publishText);
                $control.find('i').removeClass(puI).addClass(unpI);
                if ($valueControl) {
                    $valueControl.html(blogStoryManager.publishText);
                    $valueControl.addClass(pucl).removeClass(unpcl);
                }
            } else {
                $control.attr('title', blogStoryManager.publishTitle);
                $control.attr('status', blogStoryManager.unpublishText);
                $control.find('i').removeClass(unpI).addClass(puI);
                if ($valueControl) {
                    $valueControl.html(blogStoryManager.unpublishText);
                    $valueControl.addClass(unpcl).removeClass(pucl);
                }

            }
        };

        var $stories = $('.publish-story');
        $stories.each(function (index) {
            var switchControl = swithCtrl;
            var $item = $(this);
            var $valueControl = $item.parents('.item-row').find('.item-col-stats .val');
            var value = $item.attr('status') === blogStoryManager.publishText;
            var storyId = $item.attr('storyId');
            swithCtrl($item, value);

            $item.click(function () {
                value = $item.attr('status') === blogStoryManager.publishText;
                $.ajax({
                    type: "PATCH",
                    url: "/author/stories/" + storyId + "?isPublished=" + !value,
                    contentType: "application/json",
                    success: function (data) {
                        switchControl($item, !value, $valueControl);
                    },
                    error: function (data) {
                        alert(data);
                    }
                });
            });
        });

        $('#confirm-modal').on('show.bs.modal',
            function (event) {
                var button = $(event.relatedTarget);
                var storyId = button.data('story');
                var modal = $(this);
                modal.find('.btn-primary').click(function () {
                    blogStoryManager.removeStory(storyId);
                });
            });
    },
    removeStory: function (storyId) {
        $.ajax({
            type: "DELETE",
            url: "/api/v1/author/stories/" + storyId,
            success: function (data) {
                location.reload();
            },
            error: function (data) {
                alert(data);
            }
        });
    }
};