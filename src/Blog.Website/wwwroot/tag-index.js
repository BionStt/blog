var tagManager = {
    initialize: function () {
        $('#remove-tag-btn').click(
            function (event) {
                var isAccept = confirm("Удалить тег?");
                if(isAccept){
                    var button = $(event.currentTarget);
                    var tagId = button.data('tag');
                    tagManager.removeTag(tagId);
                }
            });
    },
    removeTag: function (tagId) {
        $.ajax({
            type: "DELETE",
            url: "/api/author/tags/" + tagId,
            success: function () { document.location.replace("/author/tags"); },
            error: function (data) { alert(data); }
        });
    }
};