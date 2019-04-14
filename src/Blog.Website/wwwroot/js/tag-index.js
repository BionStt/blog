var tagManager = {
    initialize: function () {
        $('.remove-tag-btn').click(
            function (event) {
                var isAccept = confirm("Удалить тег?");
                if(isAccept){
                    var button = $(event.currentTarget);
                    var tagId = button.data('tag');
                    tagManager.removeTag(tagId);
                }
            });

        $('#create-tag-btn').click(function () {
            var name = $('#newtag-name').val();
            var alias = $('#newtag-alias').val();
            var request = JSON.stringify({name: name, alias: alias});
            $.ajax({
                type: "POST",
                url: "/api/author/tags",
                data: request,
                contentType: "application/json",
                success: function (data) { location.reload()},
                error: function (data) { }
            });
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