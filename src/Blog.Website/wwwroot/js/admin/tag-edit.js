var tagManager = {
    initialize: function () {
        $('#remove-tag-btn').click(
            function (event) {
                var isAccept = confirm("Удалить тег?");
                if (isAccept) {
                    var button = $(event.currentTarget);
                    var tagId = button.data('tag');
                    tagManager.removeTag(tagId);
                }
            });

        $('#publish-btn').click(function (event) {
            var $button = $(this);
            var tagId = $("#Id").val();
            var isPublished = $button.attr('set');

            var request = [{"op": "replace", "path": "/isPublished", "value": isPublished}];

            $.ajax({
                type: "PATCH",
                url: "/api/author/tags/" + tagId,
                data: JSON.stringify(request),
                contentType: "application/json",
                success: function () { location.reload(); },
                error: function (data) {}
            });
        });
    },
    removeTag: function (tagId) {
        $.ajax({
            type: "DELETE",
            url: "/api/author/tags/" + tagId,
            success: function () {
                document.location.replace("/author/tags");
            },
            error: function (data) {
                alert(data);
            }
        });
    },
    changeStatus: function (tagId) {

    }
};