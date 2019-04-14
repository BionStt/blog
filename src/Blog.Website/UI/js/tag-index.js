var tagManager = {
    initialize: function () {
        $('#confirm-modal').on('show.bs.modal',
            function (event) {
                var button = $(event.relatedTarget);
                var tag = button.data('tag');
                var modal = $(this);
                modal.find('.btn-primary').click(function () {
                    tagManager.removeTag(tag);
                });
            });
        
    },
    removeTag: function (tagId) {
        $.ajax({
            type: "DELETE",
            url: "/api/author/tags/" + tagId,
            success: function () { location.reload(); },
            error: function (data) { alert(data); }
        });
    }
};