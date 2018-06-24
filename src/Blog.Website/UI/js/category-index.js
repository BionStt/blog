var categoryManager = {
    initialize: function () {
        $('#confirm-modal').on('show.bs.modal',
            function (event) {
                var button = $(event.relatedTarget);
                var category = button.data('category');
                var modal = $(this);
                modal.find('.btn-primary').click(function () {
                    categoryManager.removeTag(category);
                });
            });
    },
    removeTag: function (categoryId) {
        $.ajax({
            type: "DELETE",
            url: "/author/categories/" + categoryId,
            success: function () { location.reload(); },
            error: function (data) { alert(data); }
        });
    }
};